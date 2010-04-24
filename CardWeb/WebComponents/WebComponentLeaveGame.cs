// <copyright file="WebComponentLeaveGame.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The WebComponent responsible for handling LeaveGame requests.</summary>
namespace CardWeb.WebComponents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using CardGame;
    using WebActions;

    /// <summary>
    /// The WebComponent responsible for handling LeaveGame requests.
    /// </summary>
    public class WebComponentLeaveGame : WebComponent
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
        private Thread webComponentLeaveGameThread;

        /// <summary>
        /// Component URL prefix
        /// </summary>
        private string componentPrefix = "leavegame";

        /// <summary>
        /// References the server's GameController
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebComponentLeaveGame"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public WebComponentLeaveGame(IGameController gameController)
        {
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();
            this.gameController = gameController;

            this.webComponentLeaveGameThread = new Thread(new ThreadStart(this.Run));
            this.webComponentLeaveGameThread.Name = "WebComponentLeaveGameThread";
            this.webComponentLeaveGameThread.Start();
        } /* WebComponentLeaveGame() */

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

            Debug.WriteLine("WebComponentLeaveGame: Added new HTTP request to WebComponentLeaveGame.");
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

                    WebActionLeaveGame webActionLeaveGame = new WebActionLeaveGame(request, this.gameController);
                    webActionLeaveGame.Execute();
                } /* while(true) */
            }
            catch (ThreadAbortException tae)
            {
                Debug.WriteLine("WebComponentLeaveGame: " + tae.Message + " @ " + WebUtilities.GetCurrentLine());
            }
        } /* Run() */

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            this.webComponentLeaveGameThread.Abort();
        }
    }
}
