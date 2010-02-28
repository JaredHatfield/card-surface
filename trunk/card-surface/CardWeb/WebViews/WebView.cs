// <copyright file="WebView.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract view for displaying a web page.</summary>
namespace CardWeb.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An abstract view for displaying a web page.
    /// </summary>
    public abstract class WebView : IEquatable<WebView>
    {
        /// <summary>
        /// Gets the name of the web view.
        /// Comparisions against this value must be specified as case insensitive.
        /// </summary>
        /// <value>The name of the web view.</value>
        public abstract string WebViewName { get; } 

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebView's header.</returns>
        public abstract string GetHeader();

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        public abstract string GetContent();

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>A string of the WebView's content type.</returns>
        public abstract string GetContentType();

        /// <summary>
        /// Determines if a WebView is equal in name to another WebView.
        /// </summary>
        /// <param name="view">The WebView to test for equality.</param>
        /// <returns>True if the two WebViews are thes ame; otherwise, false.</returns>
        public bool Equals(WebView view)
        {
            /* No need to ignore case; WebViewNames are private and not changeable in the instance. */
            if (this.WebViewName.Equals(view.WebViewName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if a WebView may be represented by a given string.
        /// </summary>
        /// <param name="view">A string representation of the WebView to be tested for equality.</param>
        /// <returns>True if the WebView contains a matching name; otherwise, false.</returns>
        public bool Equals(string view)
        {
            if (this.WebViewName.Equals(view, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
