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
        /// Name of HTML form field associated with the username needed for login
        /// </summary>
        public const string FormFieldNameUsername = "username";

        /// <summary>
        /// Name of HTML form field associated with the password needed for login
        /// </summary>
        public const string FormFieldNamePassword = "password";

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
            content += "<title>Login : CardSurface</title>\n";
            content += "<style type=\"text/css\">\n";
            content += "td { font:Verdana; font-size:14; }\n";
            content += "input { font:Verdana; font-size:14; }\n";
            content += "</style>\n";
            content += "</head>\n";
            content += "<body>\n";
            content += "<form method=\"post\">\n";
            content += "<table>\n";
            content += "<tr><td>Username:</td><td><input name=\"" + FormFieldNameUsername + "\" type=\"text\"/></td></tr>\n";
            content += "<tr><td>Password:</td><td><input name=\"" + FormFieldNamePassword + "\" type=\"password\"></td></tr>\n";
            content += "<tr><td colspan=\"2\"><center><input type=\"submit\" value=\"Login\"/></center></td></tr>\n";
            content += "</table>\n";
            content += "</form>\n";
            content += "</body>\n";
            content += "</html>";

            return content;
        }      
    }
}
