// <copyright file="WebServerActiveSessionAlreadyExistsException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Exception thrown when the user attempted to create a session but already had an active session in existence.</summary>
namespace CardWeb.WebExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Exception thrown when the user attempted to create a session but already had an active session in existence.
    /// </summary>
    public class WebServerActiveSessionAlreadyExistsException : WebServerException
    {
        /// <summary>
        /// The session that the user attempted to create
        /// </summary>
        private WebSession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerActiveSessionAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="session">The session that the user attempted to create.</param>
        public WebServerActiveSessionAlreadyExistsException(WebSession session) : base()
        {
            this.session = session;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return "Attempted to add a session (" + this.session.SessionId + ") for a user (" + this.session.Username + "), but the user already has an active session available!";
            }
        }
    }
}
