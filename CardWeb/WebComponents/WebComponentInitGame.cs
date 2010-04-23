// <copyright file="WebComponentInitGame.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Component responsible for handling InitGame requests.</summary>
namespace CardWeb.WebComponents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using CardGame;
    using CardWeb.WebComponents.WebActions;
    using CardWeb.WebComponents.WebViews;
    using WebExceptions;

    /// <summary>
    /// Component responsible for handling InitGame requests
    /// </summary>
    public class WebComponentInitGame : WebComponent
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
        private Thread webComponentInitGameThread;

        /// <summary>
        /// Component URL prefix
        /// </summary>
        private string componentPrefix = "initgame";

        /// <summary>
        /// References the server's GameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebComponentInitGame"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public WebComponentInitGame(IGameController gameController)
        {
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();
            this.gameController = gameController;

            this.webComponentInitGameThread = new Thread(new ThreadStart(this.Run));
            this.webComponentInitGameThread.Name = "WebComponentInitGameThread";
            this.webComponentInitGameThread.Start();
        } /* WebComponentInitGame() */

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

            Debug.WriteLine("WebComponentInitGame: Added new HTTP request to WebComponentInitGame.");
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
                        WebViewInitGame webViewInitGame = new WebViewInitGame(request, this.gameController);
                        webViewInitGame.SendResponse();
                    }
                    else if (request.RequestMethod.Equals(WebRequestMethods.Http.Post))
                    {
                        try
                        {
                            WebActionInitGame webActionInitGame = new WebActionInitGame(request, this.gameController);
                            webActionInitGame.Execute();
                        }
                        catch (WebServerException e)
                        {
                            Debug.WriteLine("WebComponentInitGame: " + e.Message + " @ " + WebUtilities.GetCurrentLine());
                            WebViewInitGame webViewInitGame = new WebViewInitGame(request, this.gameController, e.Message);
                            webViewInitGame.SendResponse();
                        }
                    }
                    else
                    {
                        /* TODO: What if this is a request method that the component doesn't support?  Just discard it? */
                    }
                } /* while(true) */
            }
            catch (ThreadAbortException tae)
            {
                Debug.WriteLine("WebComponentInitGame: " + tae.Message + " @ " + WebUtilities.GetCurrentLine());
            }
        } /* Run() */

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            this.webComponentInitGameThread.Abort();
        }
    }
}
