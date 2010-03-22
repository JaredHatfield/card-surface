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

            try
            {
                Console.WriteLine("WebController: Starting for " + Dns.GetHostName());
                foreach (IPAddress addr in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    /* TODO: What if the server has more than one IPv4 address?  Just use the first one? */
                    /* Find an IPv4 address for this server. */
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        this.localaddr = addr;
                    }
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("WebController: Unable to create web server IP address because of invalid address @ {0}.", WebUtilities.GetCurrentLine());
                Console.WriteLine("-->" + ane.Message);

                /* Attempt recovery.  This should never happen. */
                this.localaddr = new IPAddress(new byte[] { 127, 0, 0, 1 });
            }

            try
            {
                /* Setup the TCP Listener on port 80 to listen for standard HTTP traffic. */
                this.webListener = new TcpListener(this.localaddr, TcpLocalPort);
                this.webListener.Start();
                Console.WriteLine("WebController: Port listener started for web controller (" + this.localaddr.ToString() + ").");
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("WebController: Unable to create web server port listener because of invalid address @ {0}.", WebUtilities.GetCurrentLine());
                Console.WriteLine("-->" + ane.Message);
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                Console.WriteLine("WebController: Unable to create web server port listener because the port was out of range @ {0} (" + TcpLocalPort + ").", WebUtilities.GetCurrentLine());
                Console.WriteLine("-->" + aoore.Message);
            }

            /* Initiate the web server thread to handle new requests. */
            this.webServerThread = new Thread(new ThreadStart(this.Run));
            this.webServerThread.Name = "WebServerThread";
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

            WebRequest request;
            WebComponent requestedComponent;
            
            while (true)
            {
                try
                {
                    numBytesReceived = 0;
                    bytesReceived = new byte[SocketMaxRecvDataBytes];

                    serverSocket = this.webListener.AcceptSocket();

                    Console.WriteLine("---------------------------------------------------------------------");
                    Console.WriteLine("WebController: Received new connection request from " + serverSocket.RemoteEndPoint + ".");

                    if (serverSocket.Connected)
                    {
                        try
                        {
                            numBytesReceived = serverSocket.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                            if (numBytesReceived > 0)
                            {
                                Console.WriteLine(Encoding.ASCII.GetString(bytesReceived));
                                request = new WebRequest(bytesReceived, serverSocket);

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
                        catch (InvalidOperationException ioe)
                        {
                            /* We didn't recognize the HTTP request type. */
                            Console.WriteLine("WebController: " + ioe.Message + " @ " + WebUtilities.GetCurrentLine() + ".");
                            /* TODO: What happens next? */
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("WebController: Unable to receive data from an accepted connection request @ {0}.", WebUtilities.GetCurrentLine());
                            Console.WriteLine("-->" + e.Message);
                            /* TODO: What happens if we're no longer able to read form the socket?  How do we reset? */
                        }
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine("WebController: Unable to accept new connection requests because of an invalid listener @ {0}.", WebUtilities.GetCurrentLine());
                    Console.WriteLine("-->" + ioe.Message);
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
        /// <returns>A WebComponent with a prefix that matches the string or throws if an Exception if one is not registered.</returns>
        private WebComponent GetRegisteredComponent(string prefix)
        {
            foreach (WebComponent component in this.registeredComponents)
            {
                if (component.Equals(prefix))
                {
                    return component;
                }
            }

            throw new Exception("Requested an Unregistered WebComponent.");
        } /* GetRegisteredComponent() */
    }
}
