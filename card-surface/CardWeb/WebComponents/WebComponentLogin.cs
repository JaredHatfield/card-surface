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

    public class WebComponentLogin : WebComponent
    {
        private Object mailboxQueueSemaphore;
        private Queue<CardWeb.WebRequest> mailboxQueue;

        private Thread webComponentLoginThread;

        private string componentPrefix = "login";

        public override string ComponentPrefix
        {
            get { return componentPrefix; }
        }

        public WebComponentLogin()
        {
            /* TODO: This is bad. */
            mailboxQueue = new Queue<CardWeb.WebRequest>();
            mailboxQueueSemaphore = new Object();

            webComponentLoginThread = new Thread(new ThreadStart(this.Run));
            webComponentLoginThread.Start();
        }

        public override void PostRequest(CardWeb.WebRequest request)
        {
            lock (mailboxQueueSemaphore)
            {
                mailboxQueue.Enqueue(request);
                Monitor.Pulse(mailboxQueueSemaphore);
            }
            Console.WriteLine("WebComponentLogin: Added new HTTP request to WebComponentLogin.");
        }

        public override void Run()
        {
            CardWeb.WebRequest request;
            string responseBuffer;
            string responseContent = String.Empty;
            int numBytesSent;

            Socket serverSocket;

            while (true)
            {
                lock (mailboxQueueSemaphore)
                {
                    if (mailboxQueue.Count == 0)
                    {
                        Console.WriteLine("WebComponentLogin: Waiting for HTTP requests to be posted to WebComponentLogin's mailbox.");
                        Monitor.Wait(mailboxQueueSemaphore);
                    }

                    /* A request has become available. */
                    request = mailboxQueue.Dequeue();
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
        } /* run() */
    }
}
