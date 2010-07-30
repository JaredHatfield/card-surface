// <copyright file="WebActionLeaveGame.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action responsible for removing a user from a game.</summary>
namespace CardWeb.WebComponents.WebActions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using CardGame;
    using WebExceptions;

    /// <summary>
    /// Action responsible for removing a user from a game.
    /// </summary>
    public class WebActionLeaveGame : WebAction
    {
        /// <summary>
        /// HTTP request that caused creation of this WebAction
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// The GameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebActionLeaveGame"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        public WebActionLeaveGame(CardWeb.WebRequest request, IGameController gameController)
        {
            this.request = request;
            this.gameController = gameController;
        } /* WebActionLeaveGame() */

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
            WebSession authenticatedSession;

            if (this.request.RequestMethod.Equals(WebRequestMethods.Http.Post))
            {
                /* Has this user's session been authenticated? */
                if (this.request.IsAuthenticated())
                {
                    /* Does the user have an active WebSession? */
                    if (WebSessionController.Instance.IsUserSessionActive(WebSessionController.Instance.GetSession(this.request.GetSessionId()).Username))
                    {
                        try
                        {
                            /* If the user already has an active sesion, try to reassign the session to this user by
                             * reusing its SessionId. */
                            authenticatedSession = WebSessionController.Instance.GetActiveSession(WebSessionController.Instance.GetSession(this.request.GetSessionId()).Username);

                            if (authenticatedSession.IsPlayingGame)
                            {
                                this.gameController.GetGame(authenticatedSession.GameId).Leave(authenticatedSession.Username);
                            }
                        }
                        catch (WebServerActiveSessionNotFoundException wsasnfe)
                        {
                            Debug.WriteLine("WebActionLogin: WebSessionController said the user had an active exception, but we couldn't find it!");
                            Debug.WriteLine("WebActionLogin: " + wsasnfe.Message);
                            Debug.WriteLine("WebActionLogin: Creating a new WebSession for the user to recover.");

                            /* If we weren't able to retrieve this user's active session, just send them back to the login page. */
                            responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                            responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                        }

                        /* They've left the game or they weren't playing a game, so send them back to their home page. */
                        responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                        responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    }
                    else
                    {
                        /* If the user doesn't have an active session, they shouldn't need to access this action.
                         * Redirect them to the login page. */
                        responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                        responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    }
                }
                else
                {
                    /* If the user hasn't logged in, send them to the login page. */
                    responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                    responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                }
            }
            else
            {
                /* This WebAction doesn't support anything but POST requests. */
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            }

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            Debug.WriteLine("---------------------------------------------------------------------");
            Debug.WriteLine("WebActionLeaveGame: Sending HTTP response (" + numBytesSent + " bytes).");
            Debug.WriteLine(responseBuffer);

            this.request.Connection.Shutdown(SocketShutdown.Both);
            this.request.Connection.Close();
        } /* Execute() */
    }
}
