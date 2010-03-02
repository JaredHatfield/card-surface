// <copyright file="WebViewLogin.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>View for displaying the login page.</summary>
namespace CardWeb.WebComponents.WebViews
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        public override string GetContent()
        {
            /* TODO: What if the user is already authenticated? */
            string content = "<html>\n";
            content += "<head>\n";
            content += "<title>" + this.WebViewName + " : CardSurface</title>\n";
            content += "<style type=\"text/css\">\n";
            content += "td { font:Verdana; font-size:14; }\n";
            content += "input { font:Verdana; font-size:14; }\n";
            content += "</style>\n";
            content += "</head>\n";
            content += "<body>\n";
            content += "<form method=\"post\">\n";
            content += "<table>\n";
            content += "<tr><td>Username:</td><td><input name=\"username\" type=\"text\"/></td></tr>\n";
            content += "<tr><td>Password:</td><td><input name=\"password\" type=\"password\"></td></tr>\n";
            content += "<tr><td colspan=\"2\"><center><input type=\"submit\" value=\"Login\"/></center></td></tr>\n";
            content += "</table>\n";
            content += "</form>\n";
            content += "</body>\n";
            content += "</html>";

            return content;
        }      
    }
}
