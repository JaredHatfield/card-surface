// <copyright file="WebViewLogin.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>View for displaying the login page.</summary>
namespace CardWeb.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// View for displaying the login page.
    /// </summary>
    public class WebViewLogin : WebView
    {
        /// <summary>
        /// A string representation of the WebView's name.
        /// </summary>
        private string webViewName = "Login";

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
            return "text/html";
        }

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebView's header.</returns>
        public override string GetHeader()
        {
            string header = "<style type=\"text/css\">\n";
            header += "td { font:Verdana; font-size:14; }\n";
            header += "input { font:Verdana; font-size:14; }\n";
            header += "</style>";

            return header;
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        public override string GetContent()
        {
            string content = "<form method=\"post\">\n";
            content += "<table>\n";
            content += "<tr><td>Username:</td><td><input type=\"text\"/></td></tr>\n";
            content += "<tr><td>Password:</td><td><input type=\"password\"></td></tr>\n";
            content += "<tr><td colspan=\"2\"><center><input type=\"submit\" value=\"Login\"/></center></td></tr>\n";
            content += "</table>\n";
            content += "</form>";

            return content;
        }      
    }
}
