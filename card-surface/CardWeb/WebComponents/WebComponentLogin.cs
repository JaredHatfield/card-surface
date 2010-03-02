// <copyright file="WebComponentLogin.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Component for managing HTTP requests from login/ URL.</summary>
namespace CardWeb.WebComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using CardWeb.WebComponents.WebActions;
    using CardWeb.WebComponents.WebViews;

    /// <summary>
    /// Responsible for managing login related HTTP requests.
    /// </summary>
    public class WebComponentLogin : WebComponent
    {
        /// <summary>
        /// Semaphore that regulates access to the mailboxQueue
        /// </summary>
        private object mailboxQueueSemaphore;

        /// <summary>
        /// Queue responsible for holding HTTP requests.
        /// </summary>
        private Queue<CardWeb.WebRequest> mailboxQueue;

        /// <summary>
        /// Thread responsible for processing incoming HTTP requests.
        /// </summary>
        private Thread webComponentLoginThread;

        /// <summary>
        /// Component URL prefix
        /// </summary>
        private string componentPrefix = "login";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebComponentLogin"/> class.
        /// </summary>
        public WebComponentLogin()
        {
            /* TODO: This is bad. */
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();

            this.webComponentLoginThread = new Thread(new ThreadStart(this.Run));
            this.webComponentLoginThread.Start();
        } /* WebComponentLogin() */

        /// <summary>
        /// Gets the component prefix.
        /// </summary>
        /// <value>The component prefix.</value>
        public override string ComponentPrefix
        {
            get { return this.componentPrefix; }
        }

        /// <summary>
        /// Posts the request to this component's mailbox queue.
        /// </summary>
        /// <param name="request">The request.</param>
        public override void PostRequest(CardWeb.WebRequest request)
        {
            lock (this.mailboxQueueSemaphore)
            {
                this.mailboxQueue.Enqueue(request);
                Monitor.Pulse(this.mailboxQueueSemaphore);
            }

            Console.WriteLine("WebComponentLogin: Added new HTTP request to WebComponentLogin.");
        } /* PostRequest() */

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public override void Run()
        {
            CardWeb.WebRequest request;
            string responseBuffer;
            string responseContent = String.Empty;
            int numBytesSent;

            Socket serverSocket;

            while (true)
            {
                lock (this.mailboxQueueSemaphore)
                {
                    if (this.mailboxQueue.Count == 0)
                    {
                        Console.WriteLine("WebComponentLogin: Waiting for HTTP requests to be posted to WebComponentLogin's mailbox.");
                        Monitor.Wait(this.mailboxQueueSemaphore);
                    }

                    /* A request has become available. */
                    request = this.mailboxQueue.Dequeue();
                    serverSocket = request.Connection;
                }

                responseBuffer = request.RequestVersion + " 200 OK" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += "Content-Type: text/html" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;

                if (request.RequestMethod.Equals(WebRequestMethods.Http.Get))
                {
                    try
                    {
                        responseContent = (new WebViewLogin()).GetContent();
                    }
                    catch (NotImplementedException nie)
                    {
                        responseContent = String.Empty;
                        Console.WriteLine("WebController" + this.ComponentPrefix + " has no view content implemented (" + nie.Message + ")");
                    }
                }
                else if (request.RequestMethod.Equals(WebRequestMethods.Http.Post))
                {
                    /* TODO: Create new WebAction response for this WebComponent and add it to the ThreadPool. */
                    Console.WriteLine("WebController: Received the following content from HTTP POST request...");
                    Console.WriteLine(request.RequestContent);

                    try
                    {
                        responseContent = "Thank you! (" + request.RequestContent + ")";
                    }
                    catch (NotImplementedException nie)
                    {
                        responseContent = String.Empty;
                        Console.WriteLine("WebController: " + this.ComponentPrefix + " has no action response implemented. (" + nie.Message + ")");
                    }
                }
                else
                {
                    /* TODO: What if this is a request method that the component doesn't support? */
                }

                byte[] responseContentBytes = Encoding.ASCII.GetBytes(responseContent);
                responseBuffer += "Content-Length: " + responseContentBytes.Length + WebUtilities.CarriageReturn + WebUtilities.LineFeed + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                responseBuffer += responseContent;

                Console.WriteLine("WebController: Sending HTTP response.\n\n" + responseBuffer);
                byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
                numBytesSent = serverSocket.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

                serverSocket.Shutdown(SocketShutdown.Both);
                serverSocket.Close();
            }   
        } /* Run() */
    }
}
