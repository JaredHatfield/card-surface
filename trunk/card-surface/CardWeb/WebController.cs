// <copyright file="WebController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The WebController that hosts the internal web server.</summary>
namespace CardWeb
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;
    using CardGame;
    using CardWeb.WebActions;
    using CardWeb.WebViews;

    /// <summary>
    /// The WebController that hosts the internal web server.
    /// </summary>
    public class WebController
    {
        /// <summary>
        /// Constant byte array representing local IP address
        /// </summary>
        private static readonly byte[] localIPv4Byte = new byte[] { 127, 0, 0, 1 };

        /// <summary>
        /// Default listening port for web server
        /// </summary>
        private const int TcpLocalPort = 80;

        /// <summary>
        /// Maximum data bytes to read from an open socket at one time
        /// </summary>
        private const int SocketMaxRecvDataBytes = 1024;

        /// <summary>
        /// Carriage return character
        /// </summary>
        private const char CarriageReturn = '\r';

        /// <summary>
        /// Line feed character
        /// </summary>
        private const char LineFeed = '\n';

        /// <summary>
        /// Number of stack trace frames to skip when constructing a new StackTrace
        /// </summary>
        private const int StackTraceFramesToSkip = 1;

        /// <summary>
        /// StackTrace frame index number to retrieve
        /// </summary>
        private const int StackTraceFrameIndex = 0;

        /// <summary>
        /// Constant required for maintaining file information when constructing a new StackTrace
        /// </summary>
        private const bool StackTraceNeedFileInfoFlag = true;

        /// <summary>
        /// Index for the HTTP Request type in a tokenized string
        /// </summary>
        private const int HttpRequestMethodIndex = 0;

        /// <summary>
        /// Index for the HTTP Request resource in a tokenized string
        /// </summary>
        private const int HttpRequestResourceIndex = 1;

        /// <summary>
        /// Index for the HTTP Request version in a tokenized string
        /// </summary>
        private const int HttpRequestVersionIndex = 2;
        
        /// <summary>
        /// GameController that the web server is able to interact with.
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// Thread responsible for handling and responding to new HTTP requests.
        /// </summary>
        private Thread webServerThread;

        /// <summary>
        /// TcpListener responsible for listening for requests on a specified port.
        /// </summary>
        private TcpListener webListener;

        /// <summary>
        /// IPAddress representing the server's local address.
        /// </summary>
        private IPAddress localaddr;

        /// <summary>
        /// List of WebViews registered with this WebController.
        /// </summary>
        private List<WebView> registeredViews;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public WebController(IGameController gameController)
        {
            this.gameController = gameController;
            this.registeredViews = new List<WebView>();

            /* Generate Default WebViews */
            this.RegisterWebView(new WebViewLogin());

            try
            {
                this.localaddr = new IPAddress(localIPv4Byte);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("WebController: Unable to create web server IP address because of invalid address @ {0}.", this.GetCurrentLine());
                Console.WriteLine("-->" + ane.Message);

                /* Attempt recovery.  This should never happen. */
                this.localaddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
            }

            try
            {
                /* Setup the TCP Listener on port 80 to listen for standard HTTP traffic. */
                this.webListener = new TcpListener(this.localaddr, TcpLocalPort);
                this.webListener.Start();
                Console.WriteLine("WebController: Port listener started for web controller.");
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("WebController: Unable to create web server port listener because of invalid address @ {0}.", this.GetCurrentLine());
                Console.WriteLine("-->" + ane.Message);
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Console.WriteLine("WebController: Unable to create web server port listener because the port was out of range @ {0} (" + TcpLocalPort + ").", this.GetCurrentLine());
                Console.WriteLine("-->" + aoore.Message);
            }

            /* Initiate the web server thread to handle new requests. */
            this.webServerThread = new Thread(new ThreadStart(this.Run));
            this.webServerThread.Start();
        } /* WebController(IGameController gameController) */

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            Socket serverSocket;
            byte[] bytesReceived = new byte[SocketMaxRecvDataBytes];
            int numBytesReceived = 0;
            int numBytesSent = 0;
            string responseBuffer;
            string responseContent;

            string requestedResource;
            string requestMethod;

            WebView requestedView;
            
            while (true)
            {
                try
                {
                    requestMethod = string.Empty;
                    requestedResource = string.Empty;
                    numBytesReceived = 0;
                    numBytesSent = 0;

                    serverSocket = this.webListener.AcceptSocket();
                    Console.WriteLine("WebController: Received new connection request from " + serverSocket.RemoteEndPoint + ".");

                    if (serverSocket.Connected)
                    {
                        try
                        {
                            numBytesReceived = serverSocket.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                            if (numBytesReceived > 0)
                            {
                                Console.WriteLine(Encoding.ASCII.GetString(bytesReceived));
                                
                                requestMethod = this.GetHttpRequestMethod(bytesReceived);

                                /* If we've received a GET HTTP request... */
                                if (requestMethod.Equals(WebRequestMethods.Http.Get))
                                {
                                    Console.WriteLine("WebController: Received a GET HTTP request.");

                                    /* Determine which resource was requested. */
                                    requestedResource = this.GetHttpRequestResource(bytesReceived);

                                    if (this.IsRegisteredWebView(requestedResource))
                                    {
                                        Console.WriteLine("WebController: " + requestedResource + " has been registered!");

                                        /* TODO: Surround in try block to catch unregistered view exception?  Should be caught already by IsRegisteredWebView() though... */
                                        requestedView = this.GetRegisteredView(requestedResource);
                                        
                                        responseBuffer = this.GetHttpRequestVersion(bytesReceived) + " 200 OK" + CarriageReturn + LineFeed;
                                        responseBuffer += "Content-Type: " + requestedView.GetContentType() + CarriageReturn + LineFeed;

                                        /* TODO: How to provide <html>, <head>, and <body> through the WebView while allowing custom Content-Type? */
                                        responseContent = "<html>";
                                        responseContent += "<head>";
                                        responseContent += "<title>" + requestedView.WebViewName + " : Card Surface</title>";

                                        try
                                        {
                                            responseContent += requestedView.GetHeader();
                                        }
                                        catch (NotImplementedException nie)
                                        {
                                            Console.WriteLine("WebController: " + requestedView.WebViewName + " has no additional HTML header implemented. (" + nie.Message + ")");
                                        }

                                        responseContent += "</head><body>";

                                        try
                                        {
                                            responseContent += requestedView.GetContent();
                                        }
                                        catch (NotImplementedException nie)
                                        {
                                            Console.WriteLine("WebController: " + requestedView.WebViewName + " has no additional HTML content implemented. (" + nie.Message + ")");
                                        }

                                        responseContent += "</body></html>";

                                        byte[] responseContentBytes = Encoding.ASCII.GetBytes(responseContent);

                                        responseBuffer += "Content-Length: " + responseContentBytes.Length + CarriageReturn + LineFeed + CarriageReturn + LineFeed;

                                        responseBuffer += responseContent;
                                    }
                                    else
                                    {
                                        Console.WriteLine("WebController: " + requestedResource + " has NOT been registered!");

                                        responseBuffer = this.GetHttpRequestVersion(bytesReceived) + " 404 NOT FOUND" + CarriageReturn + LineFeed;
                                    }

                                    Console.WriteLine("WebController: Sending HTTP response.\n\n" + responseBuffer);

                                    byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);

                                    numBytesSent = serverSocket.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);
                                }
                                else if (requestMethod.Equals(WebRequestMethods.Http.Post))
                                {
                                    /* Size of this array limited by SocketMaxRecvDataBytes */
                                    string requestContent = this.GetHttpRequestContent(bytesReceived);

                                    Console.WriteLine("WebController: Received the following content from HTTP POST request...");
                                    Console.WriteLine(requestContent);
                                }
                                else
                                {
                                    Console.WriteLine("WebController: Unrecognized HTTP request (" + requestMethod + ").");
                                    /* TODO: What do we need to return to the user? */
                                }

                                Console.WriteLine("WebController: Sent " + numBytesSent + " bytes for HTTP response.");
                                Console.WriteLine("WebController: Shutting down and closing socket.");

                                serverSocket.Shutdown(SocketShutdown.Both);
                                serverSocket.Close();
                            }
                        }
                        catch (InvalidOperationException ioe)
                        {
                            /* We didn't recognize the HTTP request type. */
                            Console.WriteLine("WebController: " + ioe.Message + " @ " + this.GetCurrentLine() + ".");
                            /* TODO: What happens next? */
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("WebController: Unable to receive data from an accepted connection request @ {0}.", this.GetCurrentLine());
                            Console.WriteLine("-->" + e.Message);
                            /* TODO: What happens if we're no longer able to read form the socket?  How do we reset? */
                        }
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine("WebController: Unable to accept new connection requests because of an invalid listener @ {0}.", this.GetCurrentLine());
                    Console.WriteLine("-->" + ioe.Message);
                    /* TODO: Recovery? */
                }
            } /* while(true) */
        } /* run() */

        /// <summary>
        /// Registers the WebView if it is not already registered.
        /// In order for views to be accessible by this server, they must be registered.
        /// </summary>
        /// <param name="registrableView">The registrable view.</param>
        public void RegisterWebView(WebView registrableView)
        {
            bool alreadyRegistered = false;

            foreach (WebView view in this.registeredViews)
            {
                if (view.Equals(registrableView))
                {
                    alreadyRegistered = true;
                }
            }

            if (!alreadyRegistered)
            {
                this.registeredViews.Add(registrableView);
            }
        } /* RegsiterWebView() */

        /// <summary>
        /// Determines whether a particluar WebView has been registered with this WebController.
        /// </summary>
        /// <param name="query">A string representation of the WebView's name.</param>
        /// <returns>
        /// <c>true</c> if the WebView has been registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegisteredWebView(string query)
        {
            foreach (WebView view in this.registeredViews)
            {
                if (view.Equals(query))
                {
                    return true;
                }
            }

            return false;
        } /* IsRegisteredWebView() */

        /// <summary>
        /// Unregisters a WebView.
        /// </summary>
        /// <param name="registrableView">The WebView to unregsiter.</param>
        private void UnregisterWebView(WebView registrableView)
        {
            /* TODO: Keep this private for now.  How would making this public become a security issue? */

            foreach (WebView view in this.registeredViews)
            {
                if (view.Equals(registrableView))
                {
                    this.registeredViews.Remove(view);
                }
            }
        } /* UnregisterWebView() */

        /// <summary>
        /// Gets the registered WebView.
        /// </summary>
        /// <param name="query">A string representation of the WebView to return.</param>
        /// <returns>A WebView with the name equal to the string or throws if an Exception if one is not registered.</returns>
        private WebView GetRegisteredView(string query)
        {
            foreach (WebView view in this.registeredViews)
            {
                if (view.Equals(query))
                {
                    return view;
                }
            }

            throw new Exception("Requested an Unregistered WebView.");
        } /* GetRegisteredView() */

        /// <summary>
        /// Gets the content of the HTTP request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A byte array containing the request contents.</returns>
        private string GetHttpRequestContent(byte[] request)
        {
            int position = 0;
            int bytesCopied = 0;
            bool patternFound = false;
            byte[] pattern = { (byte)CarriageReturn, (byte)LineFeed, (byte)CarriageReturn, (byte)LineFeed };
            string content = String.Empty;

            for (int i = 0; i < request.Length - pattern.Length; i++)
            {
                if (request[i] == pattern[0])
                {
                    /* Assume the pattern has been found. */
                    patternFound = true;
                    for (int j = 0; j < pattern.Length; j++)
                    {
                        if (request[i + j] != pattern[j])
                        {
                            /* If there's not a match, we didn't really find it. */
                            patternFound = false;
                            break;
                        }
                    }

                    /* If all bytes in the pattern array matched, we really did find it. */
                    if (patternFound)
                    {
                        /* Start the copy position after the \r\n\r\n content initiation sequence. */
                        position = i + pattern.Length;
                        break;
                    }
                }
            }

            /* Copy the content portion of the HTTP request to position. */
            for (int i = position; i < request.Length; i++)
            {
                /* Skip null characters. */
                if (request[i] != 0x0)
                {
                    content += (char)request[i];
                    bytesCopied++;
                }
            }

            /* TODO: Verify that all the bytes specified in the Content-Length property have actually been captured from the port! */
            Console.WriteLine("GetHttpRequestContent@WebController: Copied " + bytesCopied + " bytes from the HTTP request content.");

            return content;
        } /* GetHttpRequestContent() */

        /// <summary>
        /// Gets the length of the HTTP request content.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The number of bytes specified in the Content-Length property of the request.</returns>
        private int GetHttpRequestContentLength(byte[] request)
        {
            throw new NotImplementedException();
        } /* GetHttpRequestContentLength() */

        /// <summary>
        /// Gets the HTTP request method.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string representation of the HTTP Request Method</returns>
        private string GetHttpRequestMethod(byte[] request)
        {
            byte requestLineTerminator = 0x0;
            string firstLineOfRequest = String.Empty;

            for (int i = 0; i < request.Length; i++)
            {
                if (request[i] != CarriageReturn && request[i] != LineFeed)
                {
                    firstLineOfRequest += (char)request[i];
                }
                else
                {
                    requestLineTerminator |= request[i];
                }

                if (requestLineTerminator == (CarriageReturn | LineFeed))
                {
                    /* We've captured the first line of the HTTP request. */
                    break;
                }
            }

            /* Tokenize first line of HTTP request. */
            string[] firstLineTokens = firstLineOfRequest.Split(new char[] { ' ' });

            if (firstLineTokens[HttpRequestMethodIndex].Equals(WebRequestMethods.Http.Get))
            {
                return WebRequestMethods.Http.Get;
            }
            else if (firstLineTokens[HttpRequestMethodIndex].Equals(WebRequestMethods.Http.Post))
            {
                return WebRequestMethods.Http.Post;
            }
            else
            {
                throw new InvalidOperationException("Unrecognized HTTP request method");
            }
        } /* GetHttpRequestMethod() */

        /// <summary>
        /// Gets the HTTP request resource.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string representation of the requested HTTP Resource.</returns>
        private string GetHttpRequestResource(byte[] request)
        {
            byte requestLineTerminator = 0x0;
            string firstLineOfRequest = String.Empty;

            for (int i = 0; i < request.Length; i++)
            {
                if (request[i] != CarriageReturn && request[i] != LineFeed)
                {
                    firstLineOfRequest += (char)request[i];
                }
                else
                {
                    requestLineTerminator |= request[i];
                }

                if (requestLineTerminator == (CarriageReturn | LineFeed))
                {
                    /* We've captured the first line of the HTTP request. */
                    break;
                }
            }

            /* Tokenize first line of HTTP request. */
            string[] firstLineTokens = firstLineOfRequest.Split(new char[] { ' ' });
            string[] resourceTokens = firstLineTokens[HttpRequestResourceIndex].Split(new char[] { '/' });

            /* Trim off leading '/' part of URI prefix */
            return resourceTokens[1];
        } /* GetHttpRequestResource() */

        /// <summary>
        /// Gets the HTTP request version.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A string representation of the HTTP request version.</returns>
        private string GetHttpRequestVersion(byte[] request)
        {
            byte requestLineTerminator = 0x0;
            string firstLineOfRequest = String.Empty;

            for (int i = 0; i < request.Length; i++)
            {
                if (request[i] != CarriageReturn && request[i] != LineFeed)
                {
                    firstLineOfRequest += (char)request[i];
                }
                else
                {
                    requestLineTerminator |= request[i];
                }

                if (requestLineTerminator == (CarriageReturn | LineFeed))
                {
                    /* We've captured the first line of the HTTP request. */
                    break;
                }
            }

            /* Tokenize first line of HTTP request. */
            string[] firstLineTokens = firstLineOfRequest.Split(new char[] { ' ' });

            return firstLineTokens[HttpRequestVersionIndex];
        } /* GetHttpRequestVersion() */

        /// <summary>
        /// Gets the current line.
        /// </summary>
        /// <returns>Line number from which GetCurrentLine() was called</returns>
        private int GetCurrentLine()
        {
            try
            {
                return (new StackTrace(StackTraceFramesToSkip, StackTraceNeedFileInfoFlag)).GetFrame(StackTraceFrameIndex).GetFileLineNumber();
            }
            catch (Exception e)
            {
                Console.WriteLine("WebController: Unable to calculate line number in GetCurrentLine().");
                Console.WriteLine("-->" + e.Message);
                return 0;
            }
        } /* GetCurrentLine() */
     }
}
