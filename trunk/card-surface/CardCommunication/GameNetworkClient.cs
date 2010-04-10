// <copyright file="GameNetworkClient.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An instance of a Game that connects to a server.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Text;
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
        /// Initializes a new instance of the <see cref="GameNetworkClient"/> class.
        /// </summary>
        /// <param name="server">The server to connect to.</param>
        public GameNetworkClient(string server)
        {
            this.tableCommunicationController = new TableCommunicationController();
            this.SubscribeEvents();

            // TODO: Connect to the game and do lots of awesome stuff
        }

        /// <summary>
        /// Prevents a default instance of the GameNetworkClient class from being created.
        /// </summary>
        private GameNetworkClient()
            : base()
        {
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
        protected void UpdateExistingGames(Collection<object> existingGames)
        {
            //// Function to update list of existing games.
        }

        /// <summary>
        /// Updates the state of the game.
        /// </summary>
        /// <param name="game">The game update.</param>
        protected void UpdateGameState(Game game)
        {
            //// This is where the FUNCTION OF DOOM goes that updates the game.
        }
    }
}