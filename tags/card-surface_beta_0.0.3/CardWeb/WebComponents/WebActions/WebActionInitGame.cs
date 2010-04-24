// <copyright file="WebActionInitGame.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action for handling InitGame requests.</summary>
namespace CardWeb.WebComponents.WebActions
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
    using CardWeb.WebComponents.WebViews;
    using WebExceptions;

    /// <summary>
    /// Action for handling InitGame requests.
    /// </summary>
    public class WebActionInitGame : WebAction
    {
        /// <summary>
        /// HTTP request.
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// The server's IGameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Minimum stake contained in the request
        /// </summary>
        private int minimumStake;

        /// <summary>
        /// GameId for the corresponding minimum stake submitted
        /// </summary>
        private Guid gameId;

        /// <summary>
        /// SeatCode for the game the user wants to join
        /// </summary>
        private string seatCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebActionInitGame"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        public WebActionInitGame(CardWeb.WebRequest request, IGameController gameController)
        {
            this.request = request;
            this.gameController = gameController;

            try
            {
                this.minimumStake = int.Parse(request.GetUrlParameter(WebViewInitGame.FormFieldNameMinimumStake));
            }
            catch (WebServerUrlParameterNotFoundException e)
            {
                Debug.WriteLine("WebActionInitGame: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                throw new WebServerException("Error determining minimum stake.");
            }
            catch (FormatException fe)
            {
                Debug.WriteLine("WebActionInitGame: " + fe.Message + " @ " + WebUtilities.GetCurrentLine());
                throw new WebServerException("Invalid initial game stake.");
            }

            try
            {
                this.gameId = new Guid(request.GetUrlParameter(WebViewInitGame.FormFieldNameGameId));
            }
            catch (WebServerUrlParameterNotFoundException e)
            {
                Debug.WriteLine("WebActionInitGame: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                throw new WebServerException("Error determining matching game.");
            }

            try
            {
                this.seatCode = request.GetUrlParameter(WebViewJoinTable.FormFieldNameSeatCode);
            }
            catch (WebServerUrlParameterNotFoundException e)
            {
                Debug.WriteLine("WebActionInitGame: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                throw new WebServerException("Tried to start a game without selecting a seat.");
            }
        } /* WebActionInitGame() */

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
                Game desiredGame = this.gameController.GetGame(this.gameId);
                GameAccount userAccount = AccountController.Instance.LookUpUser(WebSessionController.Instance.GetSession(this.request.GetSessionId()).Username);

                if (this.minimumStake >= desiredGame.MinimumStake)
                {
                    if (this.minimumStake <= userAccount.Balance)
                    {
                        /* The initial stake meet the game's minimum stake requirements; attempt to join the user to the game. */
                        if (desiredGame.SitDown(userAccount.Username, this.seatCode, this.minimumStake, userAccount.ProfileImage))
                        {
                            /* The user has successfully joined the game. */
                            WebSessionController.Instance.GetSession(this.request.GetSessionId()).JoinGame(desiredGame);

                            responseBuffer += this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                            responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/hand/" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                        }
                        else
                        {
                            throw new WebServerException("Failed to join the game.");
                        }
                    }
                    else
                    {
                        throw new WebServerException("Insufficient account funds!");
                    }
                }
                else
                {
                    throw new WebServerException("Initial stake did not meet game's minimum stake requirements.");
                }
            }
            else
            {
                /* TODO: Is this condition necessary?  Can someone send a POST request without being authenticated? */
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            }

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            Debug.WriteLine("---------------------------------------------------------------------");
            Debug.WriteLine("WebActionInitGame: Sending HTTP response (" + numBytesSent + " bytes).");
            Debug.WriteLine(responseBuffer);

            this.request.Connection.Shutdown(SocketShutdown.Both);
            this.request.Connection.Close();
        } /* Execute() */
    }
}
