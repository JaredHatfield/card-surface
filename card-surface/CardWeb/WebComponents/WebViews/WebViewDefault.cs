// <copyright file="WebViewDefault.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>WebView representing default URL request.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// WebView representing default URL request
    /// </summary>
    public class WebViewDefault : WebView
    {
        /// <summary>
        /// HTTP Request that caused creation of this WebView
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewDefault"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public WebViewDefault(WebRequest request)
        {
            this.request = request;
        } /* WebViewDefault() */

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
        /// Sends the HTTP response.
        /// </summary>
        public override void SendResponse()
        {
            string responseBuffer = String.Empty;
            int numBytesSent = 0;

            if (this.request.IsAuthenticated())
            {
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Content-Type: " + this.GetContentType() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Content-Length: " + this.GetContentLength() + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += this.GetContent();
            }
            else
            {
                /* If the request has not been authenticated, send the user to a login page. */
                /* TODO: Automatically determine Refresh URL */
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://localhost/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            }

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("WebViewDefault: Sending HTTP response (" + numBytesSent + ").");
            Console.WriteLine(responseBuffer);

            this.request.Connection.Shutdown(SocketShutdown.Both);
            this.request.Connection.Close();
        } /* SendResponse() */

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        protected override string GetContent()
        {
            /* TODO: Automatically determine domain name for server. */
            /* TODO: Utilize WebComponent prefix in URL generation. */
            /* This content should only be displayed if an authenticated session has requested the view. */
            string content = "<html>\n";
            content += "<head>\n";
            content += "<title>CardSurface</title>\n";
            content += "</head>\n";
            content += "<body>\n";

            try
            {
                content += "Welcome, " + WebSessionController.Instance.GetSession(this.request.GetSessionId()).Username + "!<br/>\n";
                content += "<br/>\n";
                content += "<a href=\"http://localhost/JoinTable/\">Join Table</a><br/>\n";
                content += "<a href=\"http://localhost/ManageAccount/\">Manage Account</a><br/>\n";
                content += "Logout</a><br/>";
            }
            catch (Exception e)
            {
                Console.WriteLine("WebViewDefault: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                /* TODO: Should we just cancel the login process? */
            }

            content += "</body>\n";
            content += "</html>";

            return content;
        } /* GetContent() */
    }
}
