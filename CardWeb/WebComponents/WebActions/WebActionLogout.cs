// <copyright file="WebActionLogout.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action to logout of the server.</summary>
namespace CardWeb.WebComponents.WebActions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using CardGame;

    /// <summary>
    /// Action to logout of the server.
    /// </summary>
    public class WebActionLogout : WebAction
    {
        /// <summary>
        /// HTTP request that caused creation of this WebAction
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// The game controller
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebActionLogout"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        public WebActionLogout(CardWeb.WebRequest request, IGameController gameController)
        {
            this.request = request;
            this.gameController = gameController;
        } /* WebActionLogout() */

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

            WebSession authenticatedSession = WebSessionController.Instance.GetSession(this.request.GetSessionId());
            
            if (authenticatedSession.IsPlayingGame)
            {
                /* If the user is playing a game, then leave. */
                this.gameController.GetGame(authenticatedSession.GameId).Leave(authenticatedSession.Username);
            }

            /* Set a new expiration time so the browser destroys the session. */
            authenticatedSession.Destroy();

            /* Refresh the browser's cookie with a new expiration time. */
            responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            responseBuffer += "Set-Cookie: " + WebCookie.CsidIdentifier + "=" + authenticatedSession.SessionId + "; expires=" + authenticatedSession.Expires + "; httponly" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;

            /* Remove the session. */
            WebSessionController.Instance.RemoveSession(authenticatedSession);

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            Debug.WriteLine("---------------------------------------------------------------------");
            Debug.WriteLine("WebActionLogout: Sending HTTP response (" + numBytesSent + " bytes).");
            Debug.WriteLine(responseBuffer);

            this.request.Connection.Shutdown(SocketShutdown.Both);
            this.request.Connection.Close();
        } /* Execute() */
    }
}
