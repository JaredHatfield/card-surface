// <copyright file="WebServerActiveSessionNotFoundException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Exception thrown when an active session was not found for a given user.</summary>
namespace CardWeb.WebExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Exception thrown when an active session was not found for a given user.
    /// </summary>
    public class WebServerActiveSessionNotFoundException : WebServerException
    {
        /// <summary>
        /// The username for whom the web server attempted to locate an active session
        /// </summary>
        private string username;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerActiveSessionNotFoundException"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        public WebServerActiveSessionNotFoundException(string username) : base()
        {
            this.username = username;
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
                return "An active session for " + this.username + " was not found.";
            }
        }
    }
}
