// <copyright file="PlayerLeaveGameEventArgs.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The arguments for the event that is trigered when a player leaves a game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The arguments for the event that is trigered when a player leaves a game.
    /// </summary>
    public class PlayerLeaveGameEventArgs : EventArgs
    {
        /// <summary>
        /// The game that the user left.
        /// </summary>
        private Guid gameId;

        /// <summary>
        /// The username of the player that left.
        /// </summary>
        private string username;

        /// <summary>
        /// The amount of moeny the player had left on the table;
        /// </summary>
        private int money;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerLeaveGameEventArgs"/> class.
        /// </summary>
        /// <param name="gameId">The game id.</param>
        /// <param name="username">The username of the player that left.</param>
        /// <param name="money">The money the player left on the table.</param>
        internal PlayerLeaveGameEventArgs(Guid gameId, string username, int money)
        {
            this.gameId = gameId;
            this.username = username;
            this.money = money;
        }

        /// <summary>
        /// Gets the game id.
        /// </summary>
        /// <value>The game id.</value>
        public Guid GameId
        {
            get { return this.gameId; }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return this.username; }
        }

        /// <summary>
        /// Gets the money the player left on the table.
        /// </summary>
        /// <value>The amount of money.</value>
        public int Money
        {
            get { return this.money; }
        }
    }
}
