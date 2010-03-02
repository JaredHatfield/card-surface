// <copyright file="WebActionLogin.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Action to login to the server.</summary>
namespace CardWeb.WebComponents.WebActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardAccount;
    using CardWeb.WebComponents.WebViews;

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
        /// Initializes a new instance of the <see cref="WebActionLogin"/> class.
        /// </summary>
        /// <param name="postData">The HTTP POST data.</param>
        public WebActionLogin(string postData)
        {
            if (postData != String.Empty)
            {
                string[] postTokens = postData.Split(new char[] { '&' });

                foreach (string token in postTokens)
                {
                    string[] tokenData = token.Split(new char[] { '=' });
                    if (tokenData[WebView.PostRequestTokenNameIndex].Equals(WebViewLogin.FormFieldNameUsername))
                    {
                        this.username = tokenData[WebView.PostRequestTokenValueIndex];
                    }
                    else if (tokenData[WebView.PostRequestTokenNameIndex].Equals(WebViewLogin.FormFieldNamePassword))
                    {
                        this.password = tokenData[WebView.PostRequestTokenValueIndex];
                    }
                }
            }
            else
            {
                throw new Exception("Login failed.");
            }
        } /* WebActionLogin() */

        /// <summary>
        /// Executes this instance.
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (AccountController.Instance.Authenticate(this.username, this.password))
                {
                }
                else
                {
                    throw new Exception("Invalid Username\\Password.");
                }
            }
            catch (Exception e)
            {
                /* If no users exist, the AccountController.Instance.Authenticate() might throw NullReferenceException. */
                Console.WriteLine("WebActionLogin: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                throw new Exception("Login failed.");
            }
        } /* Execute() */
    }
}
