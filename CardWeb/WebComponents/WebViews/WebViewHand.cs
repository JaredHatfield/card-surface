// <copyright file="WebViewHand.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>View for displaying the users hand.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using CardAccount;
    using CardGame;

    /// <summary>
    /// View for displaying the users hand.
    /// </summary>
    public class WebViewHand : WebView
    {
        /// <summary>
        /// HTTP Request that caused creation of this WebView
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// The server's IGameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewHand"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        public WebViewHand(CardWeb.WebRequest request, IGameController gameController)
        {
            this.request = request;
            this.gameController = gameController;
        } /* WebViewHand() */

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
                if (WebSessionController.Instance.GetSession(this.request.GetSessionId()).IsPlayingGame)
                {
                    /* If the request has not been authenticated and the user has joined a game, show their hand. */
                    responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += "Content-Type: " + this.GetContentType() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += "Content-Length: " + this.GetContentLength() + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += this.GetContent();
                }
                else
                {
                    responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                }
            }
            else
            {
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            }

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            if (EnableViewDebugData)
            {
                Debug.WriteLine("---------------------------------------------------------------------");
                Debug.WriteLine("WebViewHand: Sending HTTP response. (" + numBytesSent + " bytes)");
                Debug.WriteLine(responseBuffer);
            }

            this.request.Connection.Shutdown(SocketShutdown.Both);
            this.request.Connection.Close();  
        } /* SendResponse() */

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns>A string of the WebView's content.</returns>
        protected override string GetContent()
        {
            Game currentGame = this.gameController.GetGame(WebSessionController.Instance.GetSession(this.request.GetSessionId()).GameId);
            Player currentPlayer = currentGame.GetPlayer(WebSessionController.Instance.GetSession(this.request.GetSessionId()).Username);

            string content = "<html>\n";
            content += "<head>\n";
            content += "<title>CardSurface</title>\n";
            content += "</head>\n";
            content += "<body>\n";
            content += "<table border=\"0\">\n";
            content += "<tr><td>Game Balance:</td><td>$" + currentPlayer.Money + "</td></tr>\n";
            content += "<tr><td>Account Balance:</td><td>$" + AccountController.Instance.LookUpUser(WebSessionController.Instance.GetSession(this.request.GetSessionId()).Username).Balance + "</td></tr>\n";
            content += "</table><br/>\n";

            foreach (Card card in currentPlayer.Hand.Cards)
            {
                content += "<img src=\"http://" + this.request.RequestHost + "/resource?resid=" + card.Face.ToString() + card.Suit.ToString() + "\"/>&nbsp;\n";
            }

            content += "<br/><br/>\n";
            content += "<form name=\"leaveGameForm\" method=\"post\" action=\"/leavegame\">\n";
            content += "<input type=\"submit\" value=\"Leave Game\"/>\n";
            content += "</form>\n";
            content += "<a href=\"http://" + this.request.RequestHost + "\">Home</a>\n";
            content += "</body>\n";
            content += "</html>\n";

            return content;
        } /* GetContent() */
    }
}
