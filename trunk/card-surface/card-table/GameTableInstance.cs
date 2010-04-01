// <copyright file="GameTableInstance.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The game instance on the table.</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardCommunication;
    using CardGame;

    /// <summary>
    /// The game instance on the table.
    /// </summary>
    internal class GameTableInstance : GameNetworkClient
    {
        /// <summary>
        /// The TableCommunicationController that coordinates the communication to the server.
        /// </summary>
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTableInstance"/> class.
        /// </summary>
        /// <param name="server">The server to connect to.</param>
        internal GameTableInstance(string server)
            : base(server)
        {
            this.tableCommunicationController = new TableCommunicationController();

            // TODO: Customize this class so that it does something useful.
        }

        /// <summary>
        /// Gets the name of the game.
        /// </summary>
        /// <value>The game's name.</value>
        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a value indicating the minimum amount of money required to join a game.
        /// </summary>
        /// <value>The minimum stake for the game.</value>
        public override int MinimumStake
        {
            get { throw new NotImplementedException(); }
        }
    }
}
