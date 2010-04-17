// <copyright file="WebViewResource.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Responds to HTTP requests for system resources.</summary>
namespace CardWeb.WebComponents.WebViews
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Web;
    using CardGame;
    using WebExceptions;
    
    /// <summary>
    /// Responds to HTTP requests for system resources.
    /// </summary>
    public class WebViewResource : WebView
    {
        /// <summary>
        /// HTTP Request that caused creation of this WebView
        /// </summary>
        private CardWeb.WebRequest request;
        
        /// <summary>
        /// The server's IGameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewResource"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="gameController">The game controller.</param>
        public WebViewResource(CardWeb.WebRequest request, IGameController gameController)
        {
            this.request = request;
            this.gameController = gameController;
        } /* WebViewHand() */

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <returns>A string of the WebView's content type.</returns>
        public override string GetContentType()
        {
            /* TODO: How to generalize this content type? */
            return "image/jpg";
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
            byte[] responseBufferBytes;
            string responseBuffer = String.Empty;
            int numBytesSent = 0;

            if (this.request.IsAuthenticated())
            {
                MemoryStream ms = new MemoryStream();
                try
                {
                    Deck.CardImage(this.request.GetUrlParameter("resid")).Save(ms, ImageFormat.Jpeg);
                }
                catch (WebServerUrlParameterNotFoundException e)
                {
                    /* TODO: We should load the memory stream with a default image that says, "Image not found!" */
                    Debug.WriteLine("WebViewResource: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                }

                byte[] resourceBytes = ms.GetBuffer();
                ms.Close();

                /* If the request has not been authenticated, provide them with a list of available games. */
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Content-Type: " + this.GetContentType() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Content-Length: " + resourceBytes.Length + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;

                byte[] responseBufferBytesTemp = Encoding.ASCII.GetBytes(responseBuffer);

                responseBufferBytes = new byte[responseBufferBytesTemp.Length + resourceBytes.Length];
                responseBufferBytesTemp.CopyTo(responseBufferBytes, 0);
                resourceBytes.CopyTo(responseBufferBytes, responseBufferBytesTemp.Length);
            }
            else
            {
                responseBuffer = this.GetHeader() + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Refresh: 0; url=http://" + this.request.RequestHost + "/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;

                responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
            }

            numBytesSent = this.request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

            Debug.WriteLine("---------------------------------------------------------------------");
            Debug.WriteLine("WebViewResource: Sending HTTP response. (" + numBytesSent + " bytes)");
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
            throw new NotImplementedException();
        } /* GetContent() */
    }
}
