// <copyright file="WebComponentLogin.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Component for managing HTTP requests from login/ URL.</summary>
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
            this.mailboxQueue = new Queue<CardWeb.WebRequest>();
            this.mailboxQueueSemaphore = new object();

            this.webComponentLoginThread = new Thread(new ThreadStart(this.Run));
            this.webComponentLoginThread.Name = "WebComponentLoginThread";
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

            Debug.WriteLine("WebComponentLogin: Added new HTTP request to WebComponentLogin.");
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
                    WebViewLogin webViewLogin = new WebViewLogin(request);
                    webViewLogin.SendResponse();
                }
                else if (request.RequestMethod.Equals(WebRequestMethods.Http.Post))
                {
                    try
                    {
                        WebActionLogin webActionLogin = new WebActionLogin(request);
                        webActionLogin.Execute();
                    }
                    catch (Exception e)
                    {
                        /* Proccesing HTTP POST command failed. */
                        WebViewLogin webViewLogin = new WebViewLogin(request, e.Message);
                        webViewLogin.SendResponse();
                    }
                }
                else
                {
                    /* TODO: What if this is a request method that the component doesn't support?  Just discard it? */
                }                
            } /* while(true) */
        } /* Run() */
    }
}
