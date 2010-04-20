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
    using CardWeb.WebComponents;
    using CardWeb.WebComponents.WebActions;
    using CardWeb.WebComponents.WebViews;
    using CardWeb.WebExceptions;

    /// <summary>
    /// The WebController that hosts the internal web server.
    /// </summary>
    public class WebController
    {
        /// <summary>
        /// Default listening port for web server
        /// </summary>
        private const int TcpLocalPort = 80;

        /// <summary>
        /// Maximum data bytes to read from an open socket at one time
        /// </summary>
        private const int SocketMaxRecvDataBytes = 2048;
        
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
        /// List of WebComponents.WebViews registered with this WebController.
        /// </summary>
        private List<WebComponent> registeredComponents;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="WebController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public WebController(IGameController gameController)
        {
            this.gameController = gameController;
            this.registeredComponents = new List<WebComponent>();

            /* Generate Default WebComponents */
            this.RegisterWebComponent(new WebComponentDefault());
            this.RegisterWebComponent(new WebComponentLogin());
            this.RegisterWebComponent(new WebComponentCreateAccount());
            this.RegisterWebComponent(new WebComponentJoinTable(gameController));
            this.RegisterWebComponent(new WebComponentHand(gameController));
            this.RegisterWebComponent(new WebComponentResource(gameController));
            this.RegisterWebComponent(new WebComponentInitGame(gameController));
            this.RegisterWebComponent(new WebComponentLeaveGame(gameController));

            this.localaddr = IPAddress.Any;

            try
            {
                /* Setup the TCP Listener on port 80 to listen for standard HTTP traffic. */
                this.webListener = new TcpListener(this.localaddr, TcpLocalPort);
                this.webListener.Start();

                Debug.WriteLine("WebController: Port listener started for web controller (" + this.localaddr.ToString() + ").");
            }
            catch (ArgumentNullException ane)
            {
                Debug.WriteLine("WebController: Unable to create web server port listener because of invalid address @ " + WebUtilities.GetCurrentLine());
                Debug.WriteLine("-->" + ane.Message);
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Debug.WriteLine("WebController: Unable to create web server port listener because the port was out of range @ " + WebUtilities.GetCurrentLine() + " (" + TcpLocalPort + ")");
                Debug.WriteLine("-->" + aoore.Message);
            }
            catch (SocketException se)
            {
                Debug.WriteLine("WebController: Unable to start the web controller @ " + WebUtilities.GetCurrentLine());
                Debug.WriteLine("-->" + se.Message);

                throw new WebServerCouldNotLaunchException();
            }
            
            /* Initiate the web server thread to handle new requests. */
            this.webServerThread = new Thread(this.Run);
            this.webServerThread.Name = "WebServerThread";
            this.webServerThread.Start(this.webListener);
        } /* WebController(IGameController gameController) */

        /// <summary>
        /// Runs this instance.
        /// </summary>
        /// <param name="tcpListener">The TCP listener.</param>
        public void Run(object tcpListener)
        {
            Socket serverSocket;
            byte[] bytesReceived = new byte[SocketMaxRecvDataBytes];
            byte[] contentReceived = new byte[SocketMaxRecvDataBytes];
            int numBytesReceived = 0, contentBytesReceived = 0;
            TcpListener incomingRequests = (TcpListener)tcpListener;

            WebRequest request;
            WebComponent requestedComponent;
            
            while (true)
            {
                try
                {
                    numBytesReceived = 0;
                    contentBytesReceived = 0;
                    bytesReceived = new byte[SocketMaxRecvDataBytes];
                    contentReceived = new byte[SocketMaxRecvDataBytes];

                    serverSocket = incomingRequests.AcceptSocket();

                    Debug.WriteLine("---------------------------------------------------------------------");
                    Debug.WriteLine("WebController: Received new connection request from " + serverSocket.RemoteEndPoint + ".");

                    if (serverSocket.Connected)
                    {
                        try
                        {
                            numBytesReceived = serverSocket.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                            if (numBytesReceived > 0)
                            {
                                Debug.WriteLine(Encoding.ASCII.GetString(bytesReceived));
                                request = new WebRequest(bytesReceived, serverSocket);

                                if (request.RequestContentLength != request.RequestContent.Length)
                                {
                                    /* If we didn't get all of the content on the first go-around, then we need to wait
                                     * for the rest of it to come it.  We'll have to parse the parameters after we've received it all. */
                                    while (request.RequestContentLength != request.RequestContent.Length)
                                    {
                                        /* If we didn't get all of the request content we expected, we need to wait for it
                                         * then attempt to append it to the WebRequest. */
                                        Debug.WriteLine("WebController: The entire HTTP Request was not received!  We're waiting on the content for this request...");

                                        contentBytesReceived = serverSocket.Receive(contentReceived, contentReceived.Length, SocketFlags.None);
                                        if (contentBytesReceived > 0)
                                        {
                                            Debug.WriteLine("WebController: New content received! (" + Encoding.ASCII.GetString(contentReceived).Substring(0, contentBytesReceived) + ")");
                                            /* TODO: We should verify that the received content only contains URL formatted request data. */
                                            request.AddContent(contentReceived, contentBytesReceived);
                                        }
                                        else
                                        {
                                            Debug.WriteLine("WebController: No additional content was received even though we expected it to receive more!");
                                        }
                                    }

                                    /* Now that we've received all of our content data, parse it! */
                                    request.ParseContent();
                                }

                                if (this.IsRegisteredWebComponent(request.RequestResource))
                                {
                                    /* TODO: Create new WebView response for this WebComponent and add it to the ThreadPool. 
                                             Surround in try block to catch unregistered component exception?  Should be caught already by IsRegisteredWebComponent() though... */
                                    requestedComponent = this.GetRegisteredComponent(request.RequestResource);

                                    /* TODO: Is passing the socket instance the best way to handle this? */
                                    requestedComponent.PostRequest(request);
                                }
                                else
                                {
                                    string responseBuffer = request.RequestVersion + " 404 NOT FOUND" + WebUtilities.CarriageReturn + WebUtilities.LineFeed;
                                    
                                    byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);
                                    int numBytesSent = serverSocket.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

                                    serverSocket.Shutdown(SocketShutdown.Both);
                                    serverSocket.Close();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("WebController: Unable to receive data from an accepted connection request @ " + WebUtilities.GetCurrentLine() + " (" + e.Message + ")");
                            Debug.WriteLine(e.StackTrace);
                            /* TODO: What happens if we're no longer able to read form the socket?  How do we reset? */
                        }
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    Debug.WriteLine("WebController: Unable to accept new connection requests because of an invalid listener @ " + WebUtilities.GetCurrentLine());
                    Debug.WriteLine("-->" + ioe.Message);
                    /* TODO: Recovery? */
                }
            } /* while(true) */
        } /* Run() */

        /// <summary>
        /// Registers the WebComponent if it is not already registered.
        /// In order for components to be accessible by this server, they must be registered.
        /// </summary>
        /// <param name="registrableComponent">The registrable component.</param>
        public void RegisterWebComponent(WebComponent registrableComponent)
        {
            bool alreadyRegistered = false;

            foreach (WebComponent component in this.registeredComponents)
            {
                if (component.Equals(registrableComponent))
                {
                    alreadyRegistered = true;
                }
            }

            if (!alreadyRegistered)
            {
                this.registeredComponents.Add(registrableComponent);
            }
        } /* RegisterWebComponent() */

        /// <summary>
        /// Determines whether a particluar WebComponent has been registered with this WebController.
        /// </summary>
        /// <param name="prefix">The WebComponent's prefix.</param>
        /// <returns>
        /// <c>true</c> if the WebComponent has been registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegisteredWebComponent(string prefix)
        {
            foreach (WebComponent component in this.registeredComponents)
            {
                if (component.Equals(prefix))
                {
                    return true;
                }
            }

            return false;
        } /* IsRegisteredWebComponent() */

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Close(object sender, EventArgs args)
        {
            foreach (WebComponent component in this.registeredComponents)
            {
                component.Stop();
            }
        }

        /// <summary>
        /// Unregisters a WebComponent.
        /// </summary>
        /// <param name="registrableComponent">The WebComponent to unregsiter.</param>
        private void UnregisterWebComponent(WebComponent registrableComponent)
        {
            /* TODO: Keep this private for now.  How would making this public become a security issue? */

            foreach (WebComponent component in this.registeredComponents)
            {
                if (component.Equals(registrableComponent))
                {
                    this.registeredComponents.Remove(component);
                }
            }
        } /* UnregisterWebComponent() */

        /// <summary>
        /// Gets the registered WebComponent.
        /// </summary>
        /// <param name="prefix">The WebComponent's prefix.</param>
        /// <returns>A WebComponent with a prefix that matches the string.</returns>
        /// <exception cref="WebServerException"></exception>
        private WebComponent GetRegisteredComponent(string prefix)
        {
            foreach (WebComponent component in this.registeredComponents)
            {
                if (component.Equals(prefix))
                {
                    return component;
                }
            }

            throw new WebServerException("Requested an Unregistered WebComponent.");
        } /* GetRegisteredComponent() */
    }
}
