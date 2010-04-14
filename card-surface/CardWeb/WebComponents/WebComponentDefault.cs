// <copyright file="WebComponentDefault.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>WebComponent that handles HTTP requests for the default URL.</summary>
namespace CardWeb.WebComponents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using WebViews;

    /// <summary>
    /// Default WebComponent for handling HTTP requests for the system URL.
    /// </summary>
    public class WebComponentDefault : WebComponent
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
        private Thread webComponentDefaultThread;

        /// <summary>
        /// Component URL prefix
        /// </summary>
        private string componentPrefix = String.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebComponentDefault"/> class.
        /// </summary>
        public WebComponentDefault()
        {
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();

            this.webComponentDefaultThread = new Thread(new ThreadStart(this.Run));
            this.webComponentDefaultThread.Name = "WebComponentDefaultThread";
            this.webComponentDefaultThread.Start();
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

            Debug.WriteLine("WebComponentDefault: Added new HTTP request to WebComponentDefault.");
        } /* PostRequest() */

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public override void Run()
        {
            CardWeb.WebRequest request;

            try
            {
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

                    if (request.RequestMethod.Equals(WebRequestMethods.Http.Get))
                    {
                        WebViewDefault webViewDefault = new WebViewDefault(request);
                        webViewDefault.SendResponse();
                    }
                    else
                    {
                        /* TODO: What if it's not an HTTP GET request?  Return 404? */
                    }
                } /* while(true) */
            }
            catch (ThreadAbortException tae)
            {
                Debug.WriteLine("WebComponentDefault: " + tae.Message + " @ " + WebUtilities.GetCurrentLine());
            }
        } /* Run() */

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            this.webComponentDefaultThread.Abort();
        }
    }
}
