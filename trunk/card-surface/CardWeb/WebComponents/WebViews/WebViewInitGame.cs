// <copyright file="WebViewInitGame.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>View for displaying the form requiring minimum game stake.</summary>
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
    using WebExceptions;

    /// <summary>
    /// View for displaying the form requiring minimum game stake.
    /// </summary>
    public class WebViewInitGame : WebView
    {
        /// <summary>
        /// Name of HTML form field associated with the minimum stake needed for joining a game.
        /// </summary>
        public const string FormFieldNameMinimumStake = "minimumStake";

        /// <summary>
        /// Name of HTML form field associated with the minimum stake's corresponding game.
        /// </summary>
        public const string FormFieldNameGameId = "gid";

        /// <summary>
        /// Name of the HTML form for initializing a game
        /// </summary>
        public const string FormName = "frmMinimumStake";

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
        /// The game that the player wants to join corresponding to the initial stake
        /// </summary>
        private Game desiredGame;

        /// <summary>
        /// The seatCode for the game that the player wants to join
        /// </summary>
        private string seatCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewInitGame"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        public WebViewInitGame(CardWeb.WebRequest request, IGameController gameController)
        {
            this.request = request;
            this.gameController = gameController;
            this.errorMessage = String.Empty;
        } /* WebViewInitGame() */

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewInitGame"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        /// <param name="errorMessage">The error message.</param>
        public WebViewInitGame(CardWeb.WebRequest request, IGameController gameController, string errorMessage)
        {
            this.request = request;
            this.gameController = gameController;
            this.errorMessage = errorMessage;
        } /* WebViewInitGame() */

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
        /// Gets the error header.
        /// </summary>
        /// <returns>A string of the WebView's error header.</returns>
        public string GetErrorHeader()
        {
            return this.request.RequestVersion + " 500 Internal Server Error";
        } /* GetErrorHeader() */

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
                try
                {
                    /* TODO: What if this authenticated user has already joined a game? */
                    this.desiredGame = this.gameController.GetGame(new Guid(this.request.GetUrlParameter(FormFieldNameGameId)));
                    this.seatCode = this.request.GetUrlParameter(WebViewJoinTable.FormFieldNameSeatCode);

                    /* If the request has not been authenticated, provide them with a list of available games. */
                    responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += "Content-Type: " + this.GetContentType() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += "Content-Length: " + this.GetContentLength() + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += this.GetContent();
                }
                catch (WebServerUrlParameterNotFoundException wsupnfe)
                {
                    /* A required parameter for this view was not found. */
                    /* TODO: Recover gracefully?  Implement custon 500 error message? */
                    responseBuffer = this.GetErrorHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    Debug.WriteLine("WebViewInitGame: " + wsupnfe.Message + " @ " + WebUtilities.GetCurrentLine());
                }             
            }
            else
            {
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            }

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            Debug.WriteLine("---------------------------------------------------------------------");
            Debug.WriteLine("WebViewInitGame: Sending HTTP response. (" + numBytesSent + " bytes)");
            Debug.WriteLine(responseBuffer);

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
            content += "<title>Join Game : CardSurface</title>\n";
            content += "<script language=\"JavaScript\">\n";
            content += "function verify()\n";
            content += "{\n";
            /* TODO: Also verify that minimumStake is above the account balance. */
            content += "if(" + FormName + "." + FormFieldNameMinimumStake + ".value < " + this.desiredGame.MinimumStake + ")\n";
            content += "{\n";
            content += "alert(\"Your initial stake must be greater than or equal to $" + this.desiredGame.MinimumStake + "!\");\n";
            content += "return false;\n";
            content += "}\n";
            content += "else\n";
            content += "{\n";
            content += "return true;\n";
            content += "}\n";
            content += "}\n";
            content += "</script>\n";
            content += "</head>\n";
            content += "<body onLoad=\"document." + FormName + "." + FormFieldNameMinimumStake + ".focus();\">\n";

            if (!this.errorMessage.Equals(String.Empty))
            {
                content += "<font color=\"red\"><b>" + this.errorMessage + "</b></font><br/>\n";
            }

            content += "This game requires a minimum stake of $" + this.desiredGame.MinimumStake + " to join.<br/><br/>";
            content += "Enter Initial Stake (must be greater than $" + this.desiredGame.MinimumStake + "):\n";
            content += "<form name=\"" + FormName + "\" method=\"POST\" onSubmit=\"return verify();\">\n";
            content += "<input type=\"hidden\" name=\"" + WebViewJoinTable.FormFieldNameSeatCode + "\" value=\"" + this.seatCode + "\"/>\n"; 
            content += "<input type=\"hidden\" name=\"" + FormFieldNameGameId + "\" value=\"" + this.desiredGame.Id + "\"/>\n";
            content += "$<input type=\"text\" name=\"" + FormFieldNameMinimumStake + "\"/><br/>\n";
            content += "(Your Account Balance: $" + AccountController.Instance.LookUpUser(WebSessionController.Instance.GetSession(this.request.GetSessionId()).Username).Balance + ")\n";
            content += "<input type=\"submit\" value=\"Start Game\"/>\n";
            content += "</form>\n";
            content += "</body>\n";
            content += "</html>\n";

            return content;
        } /* GetContent() */
    }
}
