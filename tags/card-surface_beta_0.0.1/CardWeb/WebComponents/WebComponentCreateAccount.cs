// <copyright file="WebComponentCreateAccount.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Component for managing HTTP requests from CreateAccount/ URL.</summary>
namespace CardWeb.WebComponents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using CardWeb.WebComponents.WebActions;
    using CardWeb.WebComponents.WebViews;
    using WebExceptions;

    /// <summary>
    /// Component for managing HTTP requests from CreateAccount/ URL.
    /// </summary>
    public class WebComponentCreateAccount : WebComponent
    {
        /// <summary>
        /// Semaphore that regulates access to the mailboxQueue
        /// </summary>
        private object mailboxQueueSemaphore;

        /// <summary>
        /// Queue responsible for holding HTTP requests
        /// </summary>
        private Queue<CardWeb.WebRequest> mailboxQueue;

        /// <summary>
        /// Thread responsible for processing incoming HTTP requests
        /// </summary>
        private Thread webComponentCreateAccountThread;

        /// <summary>
        /// Component URL prefix
        /// </summary>
        private string componentPrefix = "createaccount";

        /// <summary>
        /// Initializes a new instance of the <see cref="WebComponentCreateAccount"/> class.
        /// </summary>
        public WebComponentCreateAccount()
        {
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();

            this.webComponentCreateAccountThread = new Thread(new ThreadStart(this.Run));
            this.webComponentCreateAccountThread.Name = "WebComponentCreateAccountThread";
            this.webComponentCreateAccountThread.Start();
        }

        /// <summary>
        /// Gets the component prefix.
        /// </summary>
        /// <value>The component prefix.</value>
        public override string ComponentPrefix
        {
            get { return this.componentPrefix; }
        }

        /// <summary>
        /// Posts the request.
        /// </summary>
        /// <param name="request">The request.</param>
        public override void PostRequest(CardWeb.WebRequest request)
        {
            lock (this.mailboxQueueSemaphore)
            {
                this.mailboxQueue.Enqueue(request);
                Monitor.Pulse(this.mailboxQueueSemaphore);
            }

            Debug.WriteLine("WebComponentLogin: Added new HTTP request to WebComponentLogin.");
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
                        WebViewCreateAccount webViewCreateAccount = new WebViewCreateAccount(request);
                        webViewCreateAccount.SendResponse();
                    }
                    else if (request.RequestMethod.Equals(WebRequestMethods.Http.Post))
                    {
                        try
                        {
                            WebActionCreateAccount webActionCreateAccount = new WebActionCreateAccount(request);
                            webActionCreateAccount.Execute();
                        }
                        catch (WebServerException e)
                        {
                            WebViewCreateAccount webViewCreateAccount = new WebViewCreateAccount(request, e.Message);
                            webViewCreateAccount.SendResponse();
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
                Debug.WriteLine("WebComponentCreateAccount: " + tae.Message + " @ " + WebUtilities.GetCurrentLine());
            }
        } /* Run() */

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            this.webComponentCreateAccountThread.Abort();
        } 
    }
}
