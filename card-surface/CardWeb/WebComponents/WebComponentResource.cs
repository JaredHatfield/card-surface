// <copyright file="WebComponentResource.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Handles HTTP requests for system resources.</summary>
namespace CardWeb.WebComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using CardGame;
    using WebViews;

    /// <summary>
    /// Handles HTTP requests for system resources.
    /// </summary>
    public class WebComponentResource : WebComponent
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
        private Thread webComponentResourceThread;

        /// <summary>
        /// Component URL prefix
        /// </summary>
        private string componentPrefix = "resource";

        /// <summary>
        /// References the server's GameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebComponentResource"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public WebComponentResource(IGameController gameController)
        {
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();
            this.gameController = gameController;

            this.webComponentResourceThread = new Thread(new ThreadStart(this.Run));
            this.webComponentResourceThread.Name = "WebComponentResourceThread";
            this.webComponentResourceThread.Start();
        } /* WebComponentResource() */

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

            Console.WriteLine("WebComponentResource: Added new HTTP request to WebComponentResource.");
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

                if (request.RequestMethod.Equals(WebRequestMethods.Http.Get))
                {
                    WebViewResource webViewResource = new WebViewResource(request, this.gameController);
                    webViewResource.SendResponse();
                }
                else
                {
                    /* TODO: What if this is a request method that the component doesn't support?  Just discard it? */
                }          
            } /* while(true) */
        } /* Run() */
    }
}
