// <copyright file="GameNetworkClient.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An instance of a Game that connects to a server.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
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
        /// Initializes a new instance of the <see cref="GameNetworkClient"/> class.
        /// </summary>
        /// <param name="server">The server to connect to.</param>
        public GameNetworkClient(string server)
        {
            this.tableCommunicationController = new TableCommunicationController();

            // TODO: Connect to the game and do lots of awesome stuff
        }

        /// <summary>
        /// Prevents a default instance of the GameNetworkClient class from being created.
        /// </summary>
        private GameNetworkClient()
            : base()
        {
        } 
    }
}