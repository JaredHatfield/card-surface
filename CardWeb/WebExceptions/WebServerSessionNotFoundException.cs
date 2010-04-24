// <copyright file="WebServerSessionNotFoundException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown when the web server could not find the requested WebSession.</summary>
namespace CardWeb.WebExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown when the web server could not find the requested WebSession.
    /// </summary>
    public class WebServerSessionNotFoundException : WebServerException
    {
        /// <summary>
        /// The session ID that we attempted to find
        /// </summary>
        private Guid id;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerSessionNotFoundException"/> class.
        /// </summary>
        public WebServerSessionNotFoundException() : base()
        {
            this.id = Guid.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerSessionNotFoundException"/> class.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        public WebServerSessionNotFoundException(Guid sessionId) : base()
        {
            this.id = sessionId;
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
                return "WebSession (" + this.id + ") was not found";
            }
        }
    }
}
