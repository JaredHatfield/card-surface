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
        /// Initializes a new instance of the <see cref="WebController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public WebController(IGameController gameController)
        {
            this.gameController = gameController;

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
            string requestType;
            string responseBuffer;
            string responseContent;
            string[] requestTypeSplit;
            byte requestLineTerminator = 0x0;

            while (true)
            {
                try
                {
                    requestType = string.Empty;
                    requestLineTerminator = 0x0;
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

                                for (int i = 0; i < numBytesReceived; i++)
                                {
                                    if (bytesReceived[i] != CarriageReturn && bytesReceived[i] != LineFeed)
                                    {
                                        requestType += (char)bytesReceived[i];
                                    }
                                    else
                                    {
                                        requestLineTerminator |= bytesReceived[i];
                                    }

                                    if (requestLineTerminator == 0xF)
                                    {
                                        /* We've captured the first line of the HTTP request. */
                                        break;
                                    }
                                }

                                requestTypeSplit = requestType.Split(new char[] { ' ' });

                                if (requestTypeSplit[0].Equals("GET"))
                                {
                                    Console.WriteLine("WebController: Received a GET HTTP request.");
                                    responseBuffer = requestTypeSplit[2] + " 200 OK" + CarriageReturn + LineFeed;
                                    responseBuffer += "Content-Type: text/html" + CarriageReturn + LineFeed;

                                    responseContent = "<html><head><title>Card Surface</title></head><body>You have reached the Card Surface Web Server.<br/>";
                                    responseContent += DateTime.Now.ToString();
                                    responseContent += "</body></html>";
                                    byte[] responseContentBytes = Encoding.ASCII.GetBytes(responseContent);

                                    responseBuffer += "Content-Length: " + responseContentBytes.Length + CarriageReturn + LineFeed + CarriageReturn + LineFeed;

                                    responseBuffer += responseContent;

                                    Console.WriteLine("WebController: Sending HTTP response.\n\n" + responseBuffer);

                                    byte[] responseBufferBytes = Encoding.ASCII.GetBytes(responseBuffer);

                                    numBytesSent = serverSocket.Send(responseBufferBytes, responseBufferBytes.Length, SocketFlags.None);

                                    Console.WriteLine("WebController: Sent " + numBytesSent + " bytes for HTTP response.");
                                    Console.WriteLine("WebController: Shutting down and closing socket.");

                                    serverSocket.Shutdown(SocketShutdown.Both);
                                    serverSocket.Close();
                                }
                                else
                                {
                                    Console.WriteLine("WebController: Unrecognized HTTP request (" + requestTypeSplit[0] + ").");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("WebController: Unable to receive data from an accepted connection request @ {0}.", this.GetCurrentLine());
                            Console.WriteLine("-->" + e.Message);
                            /* What happens if we're no longer able to read form the socket?  How do we reset? */
                        }
                    }
                }
                catch (InvalidOperationException ioe)
                {
                    Console.WriteLine("WebController: Unable to accept new connection requests because of an invalid listener @ {0}.", this.GetCurrentLine());
                    Console.WriteLine("-->" + ioe.Message);
                }
            } /* while(true) */
        } /* run() */

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
