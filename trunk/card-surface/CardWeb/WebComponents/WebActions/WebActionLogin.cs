// <copyright file="WebActionLogin.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action to login to the server.</summary>
namespace CardWeb.WebComponents.WebActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using CardAccount;
    using CardWeb.WebComponents.WebViews;

    /// <summary>
    /// Action to login to the server.
    /// </summary>
    public class WebActionLogin : WebAction
    {
        /// <summary>
        /// User's username
        /// </summary>
        private string username;

        /// <summary>
        /// User's password
        /// </summary>
        private string password;

        /// <summary>
        /// HTTP request that caused creation of this WebAction
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebActionLogin"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public WebActionLogin(CardWeb.WebRequest request)
        {
            this.request = request;

            if (this.request.RequestContent != String.Empty)
            {
                string postData = this.request.RequestContent;
                string[] postTokens = postData.Split(new char[] { '&' });

                foreach (string token in postTokens)
                {
                    string[] tokenData = token.Split(new char[] { '=' });
                    if (tokenData[WebView.PostRequestTokenNameIndex].Equals(WebViewLogin.FormFieldNameUsername))
                    {
                        this.username = tokenData[WebView.PostRequestTokenValueIndex];
                    }
                    else if (tokenData[WebView.PostRequestTokenNameIndex].Equals(WebViewLogin.FormFieldNamePassword))
                    {
                        this.password = tokenData[WebView.PostRequestTokenValueIndex];
                    }
                }
            }
            else
            {
                throw new Exception("Login failed.");
            }
        } /* WebActionLogin() */

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebAction's header.</returns>
        public override string GetHeader()
        {
            return this.request.RequestVersion + " 200 OK";
        } /* GetHeader() */

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>A string of the WebAction's content type.</returns>
        public override string GetContentType()
        {
            return "text/html";
        } /* GetContentType() */

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebAction's content.</returns>
        public override string GetContent()
        {
            /* TODO: Automatically determine domain name for server. */
            /* TODO: Utilize WebComponent prefix in URL generation. */
            string content = "<html>\n";
            content += "<head>\n";
            content += "<title>Main Menu : CardSurface</title>\n";
            content += "</head>\n";
            content += "<body>\n";
            content += "Welcome, " + this.username + "!\n";
            content += "<br/>\n";
            content += "<a href=\"http://localhost/JoinTable/\">Join Table</a><br/>\n";
            content += "<a href=\"http://localhost/ManageAccount/\">Manage Account</a><br/>\n";
            content += "</body>\n";
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
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            int numBytesSent = 0;

            try
            {
                if (AccountController.Instance.Authenticate(this.username, this.password))
                {
                    string responseBuffer = String.Empty;

                    responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    /* TODO: (new Guid).ToString() should lookup a user's GUID */
                    responseBuffer += "Set-Cookie: csuid=" + (new Guid()).ToString() + "; httponly" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += "Content-Type: " + this.GetContentType() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += "Content-Length: " + this.GetContentLength() + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += this.GetContent();

                    Console.WriteLine("WebController: Sending HTTP response.\n\n" + responseBuffer);

                    byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
                    numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

                    this.request.Connection.Shutdown(SocketShutdown.Both);
                    this.request.Connection.Close();
                }
                else
                {
                    throw new Exception("Invalid Username\\Password.");
                }
            }
            catch (Exception e)
            {
                /* If no users exist, the AccountController.Instance.Authenticate() might throw NullReferenceException. */
                Console.WriteLine("WebActionLogin: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                throw new Exception("Login failed.");
            }
        } /* Execute() */
    }
}
