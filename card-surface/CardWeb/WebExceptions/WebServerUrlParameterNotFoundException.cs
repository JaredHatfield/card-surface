// <copyright file="WebServerUrlParameterNotFoundException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Exception thrown when the requested URL parameter was not found</summary>
namespace CardWeb.WebExceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Exception thrown when the requested URL parameter was not found
    /// </summary>
    public class WebServerUrlParameterNotFoundException : WebServerException
    {
        /// <summary>
        /// The parameter that was attempted to be passed
        /// </summary>
        private string attemptedParameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerUrlParameterNotFoundException"/> class.
        /// </summary>
        public WebServerUrlParameterNotFoundException() : base()
        {
            this.attemptedParameter = "unknown";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServerUrlParameterNotFoundException"/> class.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public WebServerUrlParameterNotFoundException(string parameter) : base()
        {
            this.attemptedParameter = parameter;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception.</returns>
        public override string Message
        {
            get
            {
                return "URL Parameter (" + this.attemptedParameter + ") requested by the WebView was not found";
            }
        }
    }
}
