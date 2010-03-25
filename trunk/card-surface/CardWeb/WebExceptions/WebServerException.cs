// <copyright file="WebServerException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by the web server.</summary>
namespace CardWeb.WebExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by the web server.
    /// </summary>
    public class WebServerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerException"/> class.
        /// </summary>
        public WebServerException()
        {
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
                return "Web server exception thrown";
            }
        }
    }
}
