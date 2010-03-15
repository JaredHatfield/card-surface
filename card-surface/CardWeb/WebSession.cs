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
    }
}