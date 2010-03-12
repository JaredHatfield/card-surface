// <copyright file="WebComponentJoinTable.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Component for handling JoinTable requests.</summary>
namespace CardWeb.WebComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Component for handling JoinTable requests.
    /// </summary>
    public class WebComponentJoinTable : WebComponent
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
        private Thread webComponentJoinTableThread;

        /// <summary>
        /// Component URL prefix
        /// </summary>
        private string componentPrefix = "jointable";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebComponentJoinTable"/> class.
        /// </summary>
        public WebComponentJoinTable()
        {
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();

            this.webComponentJoinTableThread = new Thread(new ThreadStart(this.Run));
            this.webComponentJoinTableThread.Name = "WebComponentJoinTableThread";
            this.webComponentJoinTableThread.Start();
        } /* WebComponentDefault() */

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

            Console.WriteLine("WebComponentJoinTable: Added new HTTP request to WebComponentJoinTable.");
        } /* PostRequest() */

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public override void Run()
        {
            CardWeb.WebRequest request;

            while (true)
            {
                lock (this.mailboxQueueSemaphore)
                {
                    if (this.mailboxQueue.Count == 0)
                    {
                        Monitor.Wait(this.mailboxQueueSemaphore);
                    }

                    /* A request has become available. */
                    request = this.mailboxQueue.Dequeue();
                }

                string responseBuffer = String.Empty;
                int numBytesSent = 0;

                responseBuffer = request.RequestVersion + " 200 OK" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                /* TODO: Automatically determine Refresh URL */
                responseBuffer += "Refresh: 0; url=http://localhost/login" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;

                byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
                numBytesSent = request.Connection.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine("WebComponentJoinTable: Sending HTTP response. (" + numBytesSent + " bytes)");
                Console.WriteLine(responseBuffer);

                request.Connection.Shutdown(SocketShutdown.Both);
                request.Connection.Close();             
            } /* while(true) */
        } /* Run() */
    }
}
