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
    internal class GameTableInstance : Game
    {
        /// <summary>
        /// The TableCommunicationController that coordinates the communication to the server.
        /// </summary>
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameTableInstance"/> class.
        /// </summary>
        internal GameTableInstance()
            : base()
        {
            this.tableCommunicationController = new TableCommunicationController();

            // TODO: Customize this class so that it does something useful.
        }

        /// <summary>
        /// Gets a value indicating whether betting is enabled.
        /// </summary>
        /// <value><c>true</c> if betting is enabled; otherwise, <c>false</c>.</value>
        public override bool BettingEnabled
        {
            get { throw new NotImplementedException(); }
        }
    }
}
