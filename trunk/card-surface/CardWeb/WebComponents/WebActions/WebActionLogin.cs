// <copyright file="WebActionLogin.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action to login to the server.</summary>
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
    using CardWeb.WebComponents.WebViews;
    using WebExceptions;

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
                try
                {
                    this.username = request.GetUrlParameter(WebViewLogin.FormFieldNameUsername);
                    this.password = request.GetUrlParameter(WebViewLogin.FormFieldNamePassword);
                }
                catch (WebServerUrlParameterNotFoundException e)
                {
                    Debug.WriteLine("WebActionLogin: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                    throw new WebServerException("Login attempt failed.");
                }
            }
            else
            {
                Debug.WriteLine("WebActionLogin: WebActionLogin did not receive any POST content @ " + WebUtilities.GetCurrentLine());
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
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            int numBytesSent = 0;
            string responseBuffer = String.Empty;
            WebSession authenticatedSession;

            /* Did the user enter the correct login credentials? */
            if (AccountController.Instance.Authenticate(this.username, this.password))
            {
                /* Does the user already have an active WebSession? */
                if (WebSessionController.Instance.IsUserSessionActive(this.username))
                {
                    try
                    {
                        /* If the user already has an active sesion, try to reassign the session to this user by
                         * reusing its SessionId. */
                        authenticatedSession = WebSessionController.Instance.GetActiveSession(this.username);
                    }
                    catch (WebServerActiveSessionNotFoundException wsasnfe)
                    {
                        Debug.WriteLine("WebActionLogin: WebSessionController said the user had an active exception, but we couldn't find it!");
                        Debug.WriteLine("WebActionLogin: " + wsasnfe.Message);
                        Debug.WriteLine("WebActionLogin: Creating a new WebSession for the user to recover.");

                        /* If we weren't able to retrieve this user's active session, just create a new one.
                         * Although, this should not have happened. */
                        authenticatedSession = new WebSession(this.username);
                        WebSessionController.Instance.AddSession(authenticatedSession);
                    }
                }
                else
                {
                    /* If the user doesn't have an active session, create a new session for the user. */
                    authenticatedSession = new WebSession(this.username);
                    WebSessionController.Instance.AddSession(authenticatedSession);
                }

                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Set-Cookie: " + WebCookie.CsidIdentifier + "=" + authenticatedSession.SessionId + "; expires=" + authenticatedSession.Expires + "; httponly" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;

                byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
                numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

                Debug.WriteLine("---------------------------------------------------------------------");
                Debug.WriteLine("WebActionLogin: Sending HTTP response (" + numBytesSent + " bytes).");
                Debug.WriteLine(responseBuffer);

                this.request.Connection.Shutdown(SocketShutdown.Both);
                this.request.Connection.Close();
            }
            else
            {
                throw new WebServerException("Invalid Username\\Password");
            }
        } /* Execute() */
    }
}
