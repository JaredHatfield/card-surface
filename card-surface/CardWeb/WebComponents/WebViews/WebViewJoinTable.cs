// <copyright file="WebViewJoinTable.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>View for displaying the for joining a table.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using CardGame;

    /// <summary>
    /// View for displaying the for joining a table.
    /// </summary>
    public sealed class WebViewJoinTable : WebView
    {
        /// <summary>
        /// Name of HTML form field associated with the seat code needed for joining a game.
        /// </summary>
        public const string FormFieldNameSeatCode = "seatCode";

        /// <summary>
        /// HTTP Request that caused creation of this WebView
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// The server's IGameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Error message to display
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewJoinTable"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        public WebViewJoinTable(CardWeb.WebRequest request, IGameController gameController)
        {
            this.request = request;
            this.gameController = gameController;
            this.errorMessage = String.Empty;
        } /* WebViewJoinTable() */

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewJoinTable"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        /// <param name="errorMessage">The error message.</param>
        public WebViewJoinTable(CardWeb.WebRequest request, IGameController gameController, string errorMessage)
        {
            this.request = request;
            this.gameController = gameController;
            this.errorMessage = errorMessage;
        } /* WebViewJoinTable() */

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

            if (this.request.IsAuthenticated())
            {
                /* If the request has not been authenticated, provide them with a list of available games. */
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Content-Type: " + this.GetContentType() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Content-Length: " + this.GetContentLength() + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += this.GetContent();
            }
            else
            {
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                /* TODO: Automatically determine Refresh URL */
                responseBuffer += "Refresh: 0; url=http://localhost/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            }

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("WebViewJoinTable: Sending HTTP response. (" + numBytesSent + " bytes)");
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
            string content = "<html>\n";
            content += "<head>\n";
            content += "<title>Seat Code : CardSurface</title>\n";
            content += "</head>\n";
            content += "<body onLoad=\"document.frmSeatCode." + FormFieldNameSeatCode + ".focus();\">\n";

            if (!this.errorMessage.Equals(String.Empty))
            {
                content += "<font color=\"red\"><b>" + this.errorMessage + "</b></font><br/>\n";
            }

            content += "Enter Seat Code:<br/>\n";
            content += "<form name=\"frmSeatCode\" method=\"POST\">\n";
            content += "<input type=\"text\" name=\"" + FormFieldNameSeatCode + "\"/><br/>\n";
            content += "<input type=\"submit\" value=\"Join Game\"/>\n";
            content += "</form>\n";
            content += "</body>\n";
            content += "</html>\n";

            return content;
        } /* GetContent() */
    }
}
