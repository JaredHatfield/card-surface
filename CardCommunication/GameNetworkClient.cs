// <copyright file="GameNetworkClient.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An instance of a Game that connects to a server.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Windows.Threading;
    using CardCommunication.CommunicationException;
    using CardGame;
    
    /// <summary>
    /// An instance of a Game that connects to a server
    /// </summary>
    public abstract class GameNetworkClient : Game
    {
        /// <summary>
        /// This talks to the server.
        /// </summary>
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// list of all games that can be played on the server.
        /// </summary>
        private Collection<string> availiableGameList;

        /// <summary>
        /// The name of the game.
        /// </summary>
        private string name;

        /// <summary>
        /// The minimum stake.
        /// </summary>
        private int minimumStake;

        /// <summary>
        /// The updater that will keep this version of the game up to date.
        /// </summary>
        private GameUpdater gameUpdater;

        /// <summary>
        /// A semaphore that allows only one thread to update the game at a time.
        /// </summary>
        private object updateSemaphore;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameNetworkClient"/> class.
        /// </summary>
        /// <param name="tableCommunicationController">The table communication controller.</param>
        /// <param name="game">The game to join.</param>
        public GameNetworkClient(TableCommunicationController tableCommunicationController, ActiveGameStruct game)
        {
            this.updateSemaphore = new object();
            this.gameUpdater = new GameUpdater(this);
            this.tableCommunicationController = tableCommunicationController;
            this.name = game.GameType;
            this.minimumStake = 0;
            this.SubscribeEvents();
            Game gameMessage = this.tableCommunicationController.SendRequestGameMessage(game);
            this.UpdateGameState(gameMessage);
        }

        /// <summary>
        /// Prevents a default instance of the GameNetworkClient class from being created.
        /// </summary>
        private GameNetworkClient()
            : base()
        {
            throw new CardCommunicationException("GameNetworkClient can not be contructed without a TabletCommunicationController.");
        }

        /// <summary>
        /// The deleagete for when the game did finish updating.
        /// </summary>
        public delegate void GameStateDidFinishUpdateDelegate();

        /// <summary>
        /// Occurs when the game state did finish updating.
        /// </summary>
        public event GameStateDidFinishUpdateDelegate GameStateDidFinishUpdate;

        /// <summary>
        /// Gets the name of the game.
        /// </summary>
        /// <value>The game's name.</value>
        public override string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets a value indicating the minimum amount of money required to join a game.
        /// </summary>
        /// <value>The minimum stake for the game.</value>
        public override int MinimumStake
        {
            get { return this.minimumStake; }
        }

        /// <summary>
        /// Function where all event that Update the game are subscribed to.
        /// </summary>
        protected void SubscribeEvents()
        {
            this.tableCommunicationController.OnUpdateGameState += new TableCommunicationController.UpdateGameStateHandler(this.UpdateGameState);
        }

        /// <summary>
        /// Updates the game list.
        /// </summary>
        /// <param name="gameList">The game list.</param>
        protected void UpdateGameList(Collection<string> gameList)
        {
            this.availiableGameList = gameList;
        }

        /// <summary>
        /// Updates the existing games.
        /// </summary>
        /// <param name="existingGames">The existing games.</param>
        protected void UpdateExistingGames(Collection<ActiveGameStruct> existingGames)
        {
            // TODO: Function to update list of existing games.
        }

        /// <summary>
        /// Tests if a move of a IPhysicalObject to a specified Pile is valid for the specific game.
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <param name="destinationPile">The destination pile.</param>
        /// <returns>
        /// True if the move if valid; otherwise false.
        /// </returns>
        protected override bool MoveTest(Guid physicalObject, Guid destinationPile)
        {
            // Send the request for a move to the server
            Game game = this.tableCommunicationController.SendMoveActionMessage(physicalObject.ToString(), destinationPile.ToString());

            // Update the game state
            this.UpdateGameState(game);

            // We always return false because we want to let the server update the game
            return false;
        }

        /// <summary>
        /// Test if an action can be performed.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        /// <param name="player">The player's username.</param>
        /// <returns>True if the action can be executed.</returns>
        protected override bool ActionTest(string name, string player)
        {
            // Lets do a test to see if the action is valid
            Player p = this.GetPlayer(player);
            if (!p.Actions.Contains(name))
            {
                return false;
            }

            // Send the request for the action to the server
            Game game = this.tableCommunicationController.SendCustomActionMessage(name, player);

            // Update the game state
            this.UpdateGameState(game);

            // We always return false because we want to let the server update the game
            return true;
        }

        /// <summary>
        /// Updates the state of the game.
        /// </summary>
        /// <param name="game">The game update.</param>
        protected void UpdateGameState(Game game)
        {
            lock (this.updateSemaphore)
            {
                Debug.WriteLine("Entered UpdateGameState");

                // This will make this game look like the message that was received.
                this.gameUpdater.Update(game);

                Console.WriteLine("Game State updated!");

                Debug.WriteLine("Exited UpdateGameState");
            }

            // Tell everyone that the game finished updating.
            if (this.GameStateDidFinishUpdate != null)
            {
                this.GameStateDidFinishUpdate();
            }
        }
    }
}