// <copyright file="WebViewManageAccount.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>View for displaying the manage account page.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// View for displaying the manage account page.
    /// </summary>
    public class WebViewManageAccount : WebView
    {
        /// <summary>
        /// A string representation of the WebView's name.
        /// </summary>
        private string webViewName = "ManageAccount";

        /// <summary>
        /// Gets the name of the web view.
        /// </summary>
        /// <value>The name of the web view.</value>
        public override string WebViewName
        {
            get { return this.webViewName; }
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>A string of the WebView's content type.</returns>
        public override string GetContentType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebView's header.</returns>
        public override string GetHeader()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        public override string GetContent()
        {
            throw new NotImplementedException();
        }
    }
}
