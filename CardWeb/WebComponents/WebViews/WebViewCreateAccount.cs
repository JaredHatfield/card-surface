// <copyright file="WebViewCreateAccount.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>View for displaying the account creation page.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// View for displaying the account creation page.
    /// </summary>
    public class WebViewCreateAccount : WebView
    {
        /// <summary>
        /// Name of HTML form field associated with the username needed for account creation
        /// </summary>
        public const string FormFieldNameUsername = "username";

        /// <summary>
        /// Name of HTML form field associated with the password needed for account creation
        /// </summary>
        public const string FormFieldNamePassword = "password";

        /// <summary>
        /// Name of HTML form field associated with the password verification needed for account creation
        /// </summary>
        public const string FormFieldNamePasswordVerification = "verifyPassword";

        /// <summary>
        /// Name of HTML form field associated with the e-mail address
        /// </summary>
        public const string FormFieldNameEmailAddress = "emailAddress";

        /// <summary>
        /// Name of the HTML form for account creation
        /// </summary>
        public const string FormName = "createAccount";

        /// <summary>
        /// HTTP request
        /// </summary>
        private CardWeb.WebRequest request;

        /// <summary>
        /// Error message string from WebActionCreateAccount
        /// </summary>
        private string errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewCreateAccount"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public WebViewCreateAccount(CardWeb.WebRequest request)
        {
            this.request = request;
            this.errorMessage = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewCreateAccount"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="message">The error message.</param>
        public WebViewCreateAccount(CardWeb.WebRequest request, string message)
        {
            this.request = request;
            this.errorMessage = message;
        }

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

            responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            responseBuffer += "Content-Type: " + this.GetContentType() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            responseBuffer += "Content-Length: " + this.GetContentLength() + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
            responseBuffer += this.GetContent();

            byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            if (EnableViewDebugData)
            {
                Debug.WriteLine("---------------------------------------------------------------------");
                Debug.WriteLine("WebViewCreateAccount: Sending HTTP response (" + numBytesSent + " bytes).");
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
            string content = "<html>\n";
            content += "<head>\n";
            content += "<title>Create Account : CardSurface</title>\n";
            content += "<style type=\"text/css\">\n";
            content += "td { font:Verdana; font-size:14; }\n";
            content += "input { font:Verdana; font-size:14; }\n";
            content += "</style>\n";
            content += "<script language=\"JavaScript\">\n";
            content += "function verify()\n";
            content += "{\n";
            content += "if(" + FormName + "." + FormFieldNamePassword + ".value != " + FormName + "." + FormFieldNamePasswordVerification + ".value)\n";
            content += "{\n";
            content += "alert(\"Passwords do not match!\");\n";
            content += "return false;\n";
            content += "}\n";
            content += "else if(" + FormName + "." + FormFieldNameUsername + ".value == \"\")\n";
            content += "{\n";
            content += "alert(\"Username must not be blank!\");\n";
            content += "return false;\n";
            content += "}\n";
            content += "else if(" + FormName + "." + FormFieldNamePassword + ".value == \"\")\n";
            content += "{\n";
            content += "alert(\"Password must not be blank!\");\n";
            content += "return false;\n";
            content += "}\n";
            content += "else\n";
            content += "{\n";
            content += "return true;\n";
            content += "}\n";
            content += "}\n";
            content += "</script>\n";
            content += "</head>\n";
            content += "<body onLoad=\"document." + FormName + "." + FormFieldNameUsername + ".focus();\">\n";

            if (!this.errorMessage.Equals(String.Empty))
            {
                content += "<font color=\"red\"><b>" + this.errorMessage + "</b></font><br/>\n";
            }

            content += "<form name=\"" + FormName + "\" method=\"post\" onSubmit=\"return verify();\">\n";
            content += "<table>\n";
            content += "<tr><td>Username:</td><td><input name=\"" + FormFieldNameUsername + "\" type=\"text\"/></td></tr>\n";
            content += "<tr><td>Password:</td><td><input name=\"" + FormFieldNamePassword + "\" type=\"password\"></td></tr>\n";
            content += "<tr><td>Verify Password:</td><td><input name=\"" + FormFieldNamePasswordVerification + "\" type=\"password\"></td></tr>\n";
            content += "<tr><td>E-mail Address:</td><td><input name=\"" + FormFieldNameEmailAddress + "\" type=\"text\"/></td>";
            content += "<tr><td colspan=\"2\"><center><input type=\"submit\" value=\"Create Account\"/></center></td></tr>\n";
            content += "</table>\n";
            content += "</form>\n";
            content += "<a href=\"http://" + this.request.RequestHost + "\">Home</a>\n";
            content += "</body>\n";
            content += "</html>";

            return content;
        } /* GetContent() */
    }
}
