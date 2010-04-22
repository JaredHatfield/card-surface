// <copyright file="WebView.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract view for displaying a web page.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// An abstract view for displaying a web page.
    /// </summary>
    public abstract class WebView
    {
        /// <summary>
        /// Index position for the name of a token name in HTTP POST data
        /// </summary>
        public const int PostRequestTokenNameIndex = 0;

        /// <summary>
        /// Index position for the name of a token value in HTTP POST data
        /// </summary>
        public const int PostRequestTokenValueIndex = 1;

        /// <summary>
        /// Enables the WebView to output debug data
        /// </summary>
        private bool enableViewDebugData = false;

        /// <summary>
        /// Gets a value indicating whether [enable view debug data].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable view debug data]; otherwise, <c>false</c>.
        /// </value>
        protected bool EnableViewDebugData
        {
            get { return this.enableViewDebugData; }
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebView's header.</returns>
        public abstract string GetHeader();

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>A string of the WebView's content type.</returns>
        public abstract string GetContentType();

        /// <summary>
        /// Gets the length of the content.
        /// </summary>
        /// <returns>An integer representing the number of bytes in the reponse content.</returns>
        public abstract int GetContentLength();

        /// <summary>
        /// Sends the HTTP response.
        /// </summary>
        public abstract void SendResponse();

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        protected abstract string GetContent();
    }
}
