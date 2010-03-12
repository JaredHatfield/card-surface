// <copyright file="WebViewLogin.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>View for displaying the login page.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
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
        /// HTTP Request that caused creation of this WebView
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// Error message passed from failed corresponding WebAction.
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewLogin"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public WebViewLogin(CardWeb.WebRequest request)
        {
            this.request = request;
            this.errorMessage = String.Empty;
        } /* WebViewLogin() */

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewLogin"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="errorMessage">The error message.</param>
        public WebViewLogin(CardWeb.WebRequest request, string errorMessage)
        {
            this.request = request;
            this.errorMessage = errorMessage;
        } /* WebViewLogin() */

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>A string of the WebView's content type.</returns>
        public override string GetContentType()
        {
            return "text/html";
        } /* GetContentType() */

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebView's header.</returns>
        public override string GetHeader()
        {
            return this.request.RequestVersion + " 200 OK";
        } /* GetHeader() */

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        public override string GetContent()
        {
            string content = "<html>\n";

            /* TODO: Automatically determine domain name for server. */
            /* TODO: Utilize WebComponent prefix in URL generation. */
            /* Check to see if an authenticated cookie is present. */
            if (!this.IsAuthenticated())
            {
                content += "<head>\n";
                content += "<title>Login : CardSurface</title>\n";
                content += "<style type=\"text/css\">\n";
                content += "td { font:Verdana; font-size:14; }\n";
                content += "input { font:Verdana; font-size:14; }\n";
                content += "</style>\n";
                content += "</head>\n";
                content += "<body>\n";

                if (!this.errorMessage.Equals(String.Empty))
                {
                    content += "<font color=\"red\"><b>" + this.errorMessage + "</b></font><br/>\n";
                }

                content += "<form method=\"post\">\n";
                content += "<table>\n";
                content += "<tr><td>Username:</td><td><input name=\"" + FormFieldNameUsername + "\" type=\"text\"/></td></tr>\n";
                content += "<tr><td>Password:</td><td><input name=\"" + FormFieldNamePassword + "\" type=\"password\"></td></tr>\n";
                content += "<tr><td colspan=\"2\"><center><input type=\"submit\" value=\"Login\"/></center></td></tr>\n";
                content += "</table>\n";
                content += "</form>\n";
                content += "</body>\n";
            }
            else
            {
                content += "<head>\n";
                content += "<title>CardSurface</title>\n";
                content += "</head>\n";
                content += "<body>\n";

                try
                {
                    content += "Welcome, " + WebSessionController.Instance.FindSession(this.request.ExtractCookie().Csid).Username + "!<br/>\n";
                    content += "<br/>\n";
                    content += "<a href=\"http://localhost/JoinTable/\">Join Table</a><br/>\n";
                    content += "<a href=\"http://localhost/ManageAccount/\">Manage Account</a><br/>\n";
                    content += "Logout</a><br/>";
                }
                catch (Exception e)
                {
                    Console.WriteLine("WebViewLogin: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                    /* TODO: Should we just cancel the login process? */
                }

                content += "</body>\n";
            }

            content += "</html>";

            return content;
        } /* GetContent() */

        /// <summary>
        /// Gets the length of the content.
        /// </summary>
        /// <returns>
        /// An integer representing the number of bytes in the reponse content.
        /// </returns>
        public override int GetContentLength()
        {
            byte[] responseContentBytes = Encoding.ASCII.GetBytes(this.GetContent());
            return responseContentBytes.Length;
        } /* GetContentLength() */

        /// <summary>
        /// Sends the HTTP response.
        /// </summary>
        public override void SendResponse()
        {
            string responseBuffer = String.Empty;
            int numBytesSent = 0;

            responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            responseBuffer += "Content-Type: " + this.GetContentType() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            responseBuffer += "Content-Length: " + this.GetContentLength() + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            responseBuffer += this.GetContent();

            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("WebController: Sending HTTP response.");
            Console.WriteLine(responseBuffer);

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            this.request.Connection.Shutdown(SocketShutdown.Both);
            this.request.Connection.Close();
        } /* SendResponse() */

        /// <summary>
        /// Determines whether this instance is authenticated.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAuthenticated()
        {
            if (this.request.ContainsCookie())
            {
                WebCookie requestCookie = this.request.ExtractCookie();

                foreach (WebSession session in WebSessionController.Instance.Sessions)
                {
                    if (session.SessionId == requestCookie.Csid)
                    {
                        return true;
                    }
                }
            }

            return false;
        } /* IsAuthenticated() */
    }
}
