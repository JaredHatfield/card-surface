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
                    throw new Exception("Login attempt failed.");
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

            if (AccountController.Instance.Authenticate(this.username, this.password))
            {
                WebSession authenticatedSession = new WebSession(this.username);
                WebSessionController.Instance.Sessions.Add(authenticatedSession);

                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://" + Dns.GetHostName() + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
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
                throw new Exception("Invalid Username\\Password");
            }
        } /* Execute() */
    }
}
