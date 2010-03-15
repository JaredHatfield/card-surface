// <copyright file="WebActionJoinTable.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action to join a table.</summary>
namespace CardWeb.WebComponents.WebActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using CardGame;
    using WebViews;

    /// <summary>
    /// Action to join a table.
    /// </summary>
    public class WebActionJoinTable : WebAction
    {
        /// <summary>
        /// HTTP request.
        /// </summary>
        private WebRequest request;

        /// <summary>
        /// The server's IGameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Seat code contained in the request
        /// </summary>
        private string seatCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebActionJoinTable"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        public WebActionJoinTable(WebRequest request, IGameController gameController)
        {
            this.request = request;
            this.gameController = gameController;

            try
            {
                this.seatCode = request.GetUrlParameter(WebViewJoinTable.FormFieldNameSeatCode);
            }
            catch (Exception e)
            {
                Console.WriteLine("WebActionJoinTable: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                throw new Exception("Error validating seat code.");
            }
        } /* WebActionJoinTable() */

        /// <summary>
        /// Gets the header.
        /// </summary>
        /// <returns>A string of the WebAction's header.</returns>
        public override string GetHeader()
        {
            return this.request.RequestVersion + " 200 OK";
        } /* GetHeader() */

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            int numBytesSent = 0;
            string responseBuffer = String.Empty;

            if (this.request.IsAuthenticated())
            {
                if (this.gameController.PasswordPeek(this.seatCode))
                {
                    Game gameContainingSeat = this.gameController.GetGame(this.seatCode);
                    if (gameContainingSeat.SitDown(WebSessionController.Instance.GetSession(this.request.GetSessionId()).Username, this.seatCode))
                    {
                        /* The user has successfully joined the game. */
                        WebSessionController.Instance.GetSession(this.request.GetSessionId()).JoinGame(this.seatCode, gameContainingSeat.Id);

                        responseBuffer += this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                        /* TODO: Automatically determine Refresh URL */
                        responseBuffer += "Refresh: 0; url=http://localhost/hand" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    }
                }
                else
                {
                    throw new Exception("Invalid seat code.");
                }
            }
            else
            {
                /* TODO: Is this condition necessary?  Can someone send a POST request without being authenticated? */
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                /* TODO: Automatically determine Refresh URL */
                responseBuffer += "Refresh: 0; url=http://localhost/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            }

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            Console.WriteLine("---------------------------------------------------------------------");
            Console.WriteLine("WebActionLogin: Sending HTTP response (" + numBytesSent + ").");
            Console.WriteLine(responseBuffer);

            this.request.Connection.Shutdown(SocketShutdown.Both);
            this.request.Connection.Close();
        } /* Execute() */
    }
}
