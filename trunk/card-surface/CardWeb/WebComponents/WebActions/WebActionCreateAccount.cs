﻿// <copyright file="WebActionCreateAccount.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action to create an account on the server.</summary>

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

    /// <summary>
    /// Action to create an account on the server.
    /// </summary>
    public class WebActionCreateAccount : WebAction
    {
        /// <summary>
        /// Username for the new account
        /// </summary>
        private string username;

        /// <summary>
        /// Password for the new account
        /// </summary>
        private string password;

        /// <summary>
        /// Verified password for the new account
        /// </summary>
        private string verifiedPassword;

        /// <summary>
        /// HTTP request
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebActionCreateAccount"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public WebActionCreateAccount(CardWeb.WebRequest request)
        {
            this.request = request;

            if (this.request.RequestContent != String.Empty)
            {
                try
                {
                    this.username = this.request.GetUrlParameter(WebViewCreateAccount.FormFieldNameUsername);
                    /* TODO: HTML Encode these strings? */
                    this.password = this.request.GetUrlParameter(WebViewCreateAccount.FormFieldNamePassword);
                    this.verifiedPassword = this.request.GetUrlParameter(WebViewCreateAccount.FormFieldNamePasswordVerification);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("WebActionCreateAccount: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                    throw new Exception("Unable to create account.");
                }
            }
            else
            {
                Debug.WriteLine("WebActionCreateAccount: WebActionCreateAccount did not receive any POST content @ " + WebUtilities.GetCurrentLine());
                throw new Exception("Invalid account parameters.");
            }
        }

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

            bool passwordsMatched = this.password.Equals(this.verifiedPassword);
            bool accountDoesNotAlreadyExist = AccountController.Instance.CreateAccount(this.username, this.password);

            if (passwordsMatched && accountDoesNotAlreadyExist)
            {
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://" + Dns.GetHostName() + "/createaccount" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;

                byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
                numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

                Debug.WriteLine("---------------------------------------------------------------------");
                Debug.WriteLine("WebActionCreateAccount: Sending HTTP response (" + numBytesSent + " bytes).");
                Debug.WriteLine(responseBuffer);

                this.request.Connection.Shutdown(SocketShutdown.Both);
                this.request.Connection.Close();
            }
            else
            {
                if (!passwordsMatched)
                {
                    throw new Exception("Passwords do not match.");
                }
                else if (!accountDoesNotAlreadyExist)
                {
                    throw new Exception("Account already exists.");
                }
                else
                {
                    throw new Exception("Account creation failed.");
                }
            }
        } /* Execute() */
    }
}