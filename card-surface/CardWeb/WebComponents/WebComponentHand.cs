// <copyright file="WebComponentHand.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Component for handling game display for web.</summary>
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

    public class WebComponentHand : WebComponent
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
        private Thread webComponentHandThread;

        /// <summary>
        /// Component URL prefix
        /// </summary>
        private string componentPrefix = "hand";

        /// <summary>
        /// References the server's GameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebComponentHand"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public WebComponentHand(IGameController gameController)
        {
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();
            this.gameController = gameController;

            this.webComponentHandThread = new Thread(new ThreadStart(this.Run));
            this.webComponentHandThread.Name = "WebComponentHandThread";
            this.webComponentHandThread.Start();
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

            Console.WriteLine("WebComponentHand: Added new HTTP request to WebComponentHand.");
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
                    WebViewHand webViewHand = new WebViewHand(request, this.gameController);
                    webViewHand.SendResponse();
                }
                else
                {
                    /* TODO: What if this is a request method that the component doesn't support?  Just discard it? */
                }          
            } /* while(true) */
        } /* Run() */
    }
}
