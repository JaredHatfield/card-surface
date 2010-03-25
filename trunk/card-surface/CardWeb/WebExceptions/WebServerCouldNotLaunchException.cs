// <copyright file="WebServerCouldNotLaunchException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown when the web server could not launch.</summary>
namespace CardWeb.WebExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown when the web server could not launch.
    /// </summary>
    public class WebServerCouldNotLaunchException : WebServerException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerCouldNotLaunchException"/> class.
        /// </summary>
        public WebServerCouldNotLaunchException()
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
                return "Could not launch the web server";
            }
        }
    }
}
