// <copyright file="WebView.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract view for displaying a web page.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An abstract view for displaying a web page.
    /// </summary>
    public abstract class WebView
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
    }
}
