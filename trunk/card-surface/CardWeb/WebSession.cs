﻿// <copyright file="WebSession.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Represents an authenticated WebSession on the server.</summary>
namespace CardWeb
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An object that represents a CardSurface WebSession
    /// </summary>
    public class WebSession
    {
        /// <summary>
        /// The CardSurface Session ID
        /// </summary>
        private Guid sessionId;

        /// <summary>
        /// Username of the authenticated user
        /// </summary>
        private string username;

        /// <summary>
        /// Time the session was created
        /// </summary>
        private DateTime created;

        /// <summary>
        /// Time the session was last modified
        /// </summary>
        private DateTime lastModified;

        /// <summary>
        /// Time the session expires
        /// </summary>
        private DateTime expires;

        /// <summary>
        /// The Game Guid that a user joined with the seat code
        /// </summary>
        private Guid gameId;

        /// <summary>
        /// Indicates whether or not this session has joined a game
        /// </summary>
        private bool isPlayingGame;

        /// <summary>
        /// The seat code a user used to join a game.  Used for historical information.
        /// The Game generates a new seat code for that seat once a valid one has been used to join the game.
        /// </summary>
        private string seatCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSession"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        public WebSession(string username)
        {
            this.sessionId = Guid.NewGuid();
            this.username = username;
            this.created = DateTime.Now;
            this.lastModified = DateTime.Now;
            this.expires = DateTime.Now.AddMinutes(5.0);
        }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        /// <value>The session id.</value>
        public Guid SessionId
        {
            get { return this.sessionId; }
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
        /// Gets the created timestamp.
        /// </summary>
        /// <value>The created timestamp.</value>
        public DateTime Created
        {
            get { return this.created; }
        }

        /// <summary>
        /// Gets or sets the last modified timestamp.
        /// </summary>
        /// <value>The last modified timestamp.</value>
        public DateTime LastModified
        {
            get { return this.lastModified; }
            set { this.lastModified = value; }
        }

        /// <summary>
        /// Gets the expiration timestamp (in HTTP cookie format).
        /// </summary>
        /// <value>The timestamp this session expires (properly formatted for HTTP Set-Cookie).</value>
        public string Expires
        {
            get { return this.expires.ToUniversalTime().ToString(WebUtilities.DateTimeCookieFormat) + " GMT"; }
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
        /// Gets the historical seat code.
        /// </summary>
        /// <value>The seat code.</value>
        public string SeatCode
        {
            get { return this.seatCode; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is playing game.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is playing a game; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlayingGame
        {
            get { return this.isPlayingGame; }
        }

        /// <summary>
        /// Joins this WebSession to a game.
        /// </summary>
        /// <param name="seatCode">The seat code.</param>
        /// <param name="gameId">The game id.</param>
        public void JoinGame(string seatCode, Guid gameId)
        {
            this.seatCode = seatCode;
            this.gameId = gameId;
            this.isPlayingGame = true;
        } /* JoinGame() */

        /// <summary>
        /// Leaves the game the session is currently joined to.
        /// </summary>
        public void LeaveGame()
        {
            this.isPlayingGame = false;
            this.gameId = Guid.Empty;
        } /* LeaveGame() */
    }
}
