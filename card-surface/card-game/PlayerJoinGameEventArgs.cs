// <copyright file="PlayerJoinGameEventArgs.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The arguments for the event that is trigered when a player joins a game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The arguments for the event that is trigered when a player joins a game.
    /// </summary>
    public class PlayerJoinGameEventArgs : EventArgs
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
        /// Initializes a new instance of the <see cref="PlayerJoinGameEventArgs"/> class.
        /// </summary>
        /// <param name="gameId">The game id.</param>
        /// <param name="username">The username of the player that left.</param>
        internal PlayerJoinGameEventArgs(Guid gameId, string username)
        {
            this.gameId = gameId;
            this.username = username;
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
    }
}
