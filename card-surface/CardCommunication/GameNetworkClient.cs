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
        /// A flag that indicates that the game has updated at least once.
        /// </summary>
        private bool gameDidInitialize;

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
        /// Initializes a new instance of the <see cref="GameNetworkClient"/> class.
        /// </summary>
        /// <param name="tableCommunicationController">The table communication controller.</param>
        /// <param name="game">The game to join.</param>
        public GameNetworkClient(TableCommunicationController tableCommunicationController, ActiveGameStruct game)
        {
            this.gameUpdater = new GameUpdater(this);
            this.tableCommunicationController = tableCommunicationController;
            this.gameDidInitialize = false;
            this.name = game.GameType;
            this.minimumStake = 0;
            this.SubscribeEvents();
            this.tableCommunicationController.SendRequestGameMessage(game);
            while (!this.gameDidInitialize)
            {
                // We are not going to return from our constructor until the game updates at least once;
            }
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
            this.tableCommunicationController.OnUpdateGameList += new TableCommunicationController.UpdateGameListHandler(this.UpdateGameList);
            this.tableCommunicationController.OnUpdateGameState += new TableCommunicationController.UpdateGameStateHandler(this.UpdateGameState);
            this.tableCommunicationController.OnUpdateExistingGames += new TableCommunicationController.UpdateExistingGamesHandler(this.UpdateExistingGames);
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
        protected void UpdateExistingGames(Collection<string> existingGames)
        {
            // TODO: Function to update list of existing games.
        }

        /// <summary>
        /// Updates the state of the game.
        /// </summary>
        /// <param name="game">The game update.</param>
        protected void UpdateGameState(Game game)
        {
            Debug.WriteLine("Entered UpdateGameState");

            // This will make this game look like the message that was received.
            this.gameUpdater.Update(game);
            
            // Flag the game as being updated
            if (!this.gameDidInitialize)
            {
                this.gameDidInitialize = true;
            }

            Debug.WriteLine("Exited UpdateGameState");
        }
    }
}