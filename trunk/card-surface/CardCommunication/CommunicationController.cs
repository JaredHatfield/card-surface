// <copyright file="CommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The base class for the communication controller.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Formatters.Soap;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using System.Xml.Serialization;
    using CardGame;
    using CommunicationException;
    using Messages;

    /// <summary>
    /// The base class for the communication controller.
    /// Create sockets using networkstreams to communicate to certain ports.
    /// Use listeners to handle incoming xml.  Write on command.
    /// </summary>
    public abstract class CommunicationController
    {
        /// <summary>
        /// The Port number of the listening socket.
        /// </summary>
        protected const int ServerListenerPortNumber = 30565;

        /// <summary>
        /// The Port number of the listening socket.
        /// </summary>
        protected const int ClientListenerPortNumber = 30567;

        /// <summary>
        /// The IP Address of the system.
        /// </summary>
        private IPAddress baseIPAddress;

        /// <summary>
        /// The IPEndPoint of the listening socket.
        /// </summary>
        private IPEndPoint hostReceiveEndPoint;

        /// <summary>
        /// The IPEndPoint of the listnening socket of the remote machine.
        /// </summary>
        private IPEndPoint remoteEndPoint;

        /// <summary>
        /// The GameController that the communication controller is able to interact with.
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// If the main thread is not waiting on a response from the server.
        /// </summary>
        private bool communicationCompleted = true;

        /// <summary>
        /// The process communication semaphore.
        /// </summary>
        private object processCommSephamore;

        /// <summary>
        /// The thread that handles incoming communications.
        /// </summary>
        private Thread processingThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationController"/> class.
        /// </summary>
        internal CommunicationController()
        {
            this.processCommSephamore = new object();
            this.InitializeCommunicationController();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        internal CommunicationController(IGameController gameController)
        {
            this.processCommSephamore = new object();
            this.gameController = gameController;
            this.InitializeCommunicationController();
        }

        /// <summary>
        /// Gets or sets a value indicating whether [communication completed].
        /// </summary>
        /// <value>
        /// <c>true</c> if [communication completed]; otherwise, <c>false</c>.
        /// </value>
        public bool CommunicationCompleted
        {
            get { return this.communicationCompleted; }
            set { this.communicationCompleted = value; }
        }

        /// <summary>
        /// Gets the IP.
        /// </summary>
        /// <value>The local IP address.</value>
        public string IP
        {
            get { return this.baseIPAddress.ToString(); }
        }

        /// <summary>
        /// Gets the game controller.
        /// </summary>
        /// <value>The game controller.</value>
        protected IGameController GameController
        {
            get { return this.gameController; }
        }

        /// <summary>
        /// Gets or sets the base IP address.
        /// </summary>
        /// <value>The base IP address.</value>
        protected IPAddress BaseIPAddress
        {
            get { return this.baseIPAddress; }
            set { this.baseIPAddress = value; }
        }

        /// <summary>
        /// Gets or sets the host receive end point of the system.
        /// </summary>
        /// <value>The host receive end point.</value>
        protected IPEndPoint HostReceiveEndPoint
        {
            get { return this.hostReceiveEndPoint; }
            set { this.hostReceiveEndPoint = value; }
        }

        /// <summary>
        /// Gets or sets the remote end point.
        /// </summary>
        /// <value>The remote end point.</value>
        protected IPEndPoint RemoteEndPoint
        {
            get { return this.remoteEndPoint; }
            set { this.remoteEndPoint = value; }
        }

        /// <summary>
        /// Gets the process comm sephamore.
        /// </summary>
        /// <value>The process comm sephamore.</value>
        protected object ProcessCommSephamore
        {
            get { return this.processCommSephamore; }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Close(object sender, EventArgs args)
        {
            this.processingThread.Abort();
        }

        /// <summary>
        /// Initializes the IP address ports.
        /// </summary>
        protected abstract void InitializeIPAddressPorts();

        /// <summary>
        /// Initializes the communication controller.
        /// </summary>
        protected void InitializeCommunicationController()
        {
            try
            {
                IPHostEntry hostIP = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress hostAddress = null;
                SocketAddress hostSocketAddress = new SocketAddress(AddressFamily.InterNetwork);
                bool found = false;

                //// Get the ip address for internetwork transportation
                for (int i = 0; i < hostIP.AddressList.Length && !found; ++i)
                {
                    if (hostIP.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        hostAddress = hostIP.AddressList[i];
                        found = true;
                    }
                }
                
                this.baseIPAddress = hostAddress;
                this.InitializeIPAddressPorts();

                // Initialize thread that listens to listener port.
                this.StartListener();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error initializing communication controller. " + e.ToString());
                throw new MessageTransportException("Error initializing communication controller. ", e);
            }          
        }

        /// <summary>
        /// Starts the listener socket.
        /// </summary>
        /// <returns>whether the listner socket was started.</returns>
        protected bool StartListener()
        {
            bool success = true;

            try
            {
                TcpListener tcpListener = new TcpListener(this.hostReceiveEndPoint);
                tcpListener.ExclusiveAddressUse = true;
                tcpListener.Start(10);

                this.processingThread = new Thread(this.ProcessComm);
                this.processingThread = new Thread(this.ProcessComm);
                this.processingThread.Name = "TcpListener";
                this.processingThread.Start(tcpListener);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error starting socket listener." + e);
                success = false;
                throw new SocketBindingException("StartListener Socket Binding Exception.", e);
            }

            return success;
        }

        /// <summary>
        /// Updates the state of the game.
        /// </summary>
        /// <param name="game">The updated game state.</param>
        protected abstract void UpdateGameState(Game game);

        /// <summary>
        /// Processes the communication.
        /// </summary>
        /// <param name="listener">The listener.</param>
        protected void ProcessComm(object listener)
        {
            TcpListener tcpListener = (TcpListener)listener;
            int counter = 0;

            while (true)
            {
                if (tcpListener.Pending())
                {
                    Socket socketListener;
                    IPEndPoint ip;
                    MemoryStream ms = new MemoryStream();
                    byte[] buffer = new byte[2048];

                    int size = 0;

                    counter++;

                    Debug.WriteLine("Attempting to connect thread #" + counter + ". " + this.ToString());
                    socketListener = tcpListener.AcceptSocket();
                    Debug.WriteLine("Thread #" + counter + " connected" + ". " + this.ToString());

                    this.ProcessRemoteClientInfo(socketListener);

                    ip = (IPEndPoint)socketListener.RemoteEndPoint;

                    if (socketListener.Connected)
                    {
                        do
                        {
                            Debug.WriteLine("Receiving On Thread #" + counter + ". " + this.ToString());
                            size = socketListener.Receive(buffer, 0, buffer.Length, SocketFlags.None);

                            if (size > 0)
                            {
                                Debug.WriteLine("Packet size: " + size);
                                ms.Write(buffer, 0, size);
                            }
                        }
                        while (socketListener.Available != 0);
                        Debug.WriteLine("Finished Receiving On Thread #" + counter + ". " + this.ToString());

                        CommunicationObject co = new CommunicationObject();
                        co.TcpListener = tcpListener;
                        co.Data = ms.ToArray();
                        co.RemoteIPAddress = ip.Address;
                        co.Buffer = new byte[] { (byte)counter }; 

                        Thread responder = new Thread(this.ProcessCommData);
                        responder.Name = "Thread Responder";
                        responder.Start(co);
                    }
                }
            }
        }

        /// <summary>
        /// Processes the remote client info.
        /// </summary>
        /// <param name="remoteSocket">The remote socket.</param>
        protected abstract void ProcessRemoteClientInfo(Socket remoteSocket);

        /// <summary>
        /// Processes the comm data.
        /// </summary>
        /// <param name="communicationObject">The communication object.</param>
        protected void ProcessCommData(object communicationObject)
        {
            try
            {
                CommunicationObject co = (CommunicationObject)communicationObject;
                MemoryStream ms = new MemoryStream(co.Data);
                int counter = (int)co.Buffer[0];

                try
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    Game game;

                    try
                    {
                        Debug.WriteLine("Attempting to convert to game on Thread #" + counter + ". " + this.ToString());
                        game = (Game)bf.Deserialize(ms);
                    }
                    catch (SerializationException e)
                    {
                        Debug.WriteLine("Error deserializing game. ");
                        throw new MessageDeserializationException("Error deserializing the game. " + e.ToString());
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Error. " + e.ToString());
                        throw new CardCommunicationException("Error. " + e.ToString());
                    }

                    Debug.WriteLine("Success converting to game on Thread #" + counter + ". " + this.ToString());

                    this.UpdateGameState(game);
                }
                catch (MessageDeserializationException)
                { 
                    // This exception is expected.
                    Debug.WriteLine("Attempting to convert to message on Thread #" + counter + ". " + this.ToString());

                    XmlDocument messageDoc = new XmlDocument();

                    ms.Position = 0;
                    messageDoc.Load(ms);

                    this.ConvertFromXMLToMessage(messageDoc, co.RemoteIPAddress);
                    Debug.WriteLine("Success converting to message on Thread #" + counter + ". " + this.ToString());
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Fatal Error processing data. " + e.ToString());
                    throw new MessageTransportException("Fatal Error processing data. ", e);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error processing and responding to request." + e.ToString());
                throw new CardCommunicationException("Error processing and responding to request." + e.ToString());
            }

            this.SetCommunicationCompleted();
        }

        /// <summary>
        /// Starts the transporter.
        /// </summary>
        /// <returns>The socket that is connected.</returns>
        protected Socket StartTransporter()
        {
            try
            {            
                TcpClient tcpClient = new TcpClient();

                tcpClient.Connect(this.remoteEndPoint);

                return tcpClient.Client;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error starting transporter. " + e.ToString());
                throw new SocketBindingException("Error starting transporter. " + e.ToString());
            }
        }

        /// <summary>
        /// Sets the communication completed.
        /// </summary>
        protected abstract void SetCommunicationCompleted();

        /// <summary>
        /// Converts from XML to message.
        /// </summary>
        /// <param name="messageDoc">The message document.</param>
        /// <param name="remoteIP">The remote IPEndPoint.</param>
        protected abstract void ConvertFromXMLToMessage(XmlDocument messageDoc, IPAddress remoteIP);

        /// <summary>
        /// Transports the communication.
        /// </summary>
        /// <param name="game">The game to be transported.</param>
        protected abstract void TransportCommunication(Game game);

        /// <summary>
        /// Transports the communication.
        /// </summary>
        /// <param name="message">The message document to be transported.</param>
        protected void TransportCommunication(XmlDocument message)
        {            
            try
            {
                lock (this.processCommSephamore)
                {
                    MemoryStream ms = new MemoryStream();
                    message.Save(ms);
                    byte[] data = ms.ToArray();

                    Socket transporter = this.StartTransporter();
                    transporter.Poll(10, SelectMode.SelectWrite);

                    Debug.WriteLine(this.GetType().ToString());

                    transporter.Send(data, 0, data.Length, SocketFlags.None);
                    this.SuccessfulTransport(transporter);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Sending Communication." + e.ToString());
                throw new MessageTransportException("Comm.TransportCommunication Exception.", e);
            }
        }

        /// <summary>
        /// Gets the IP address.
        /// </summary>
        /// <param name="ep">The IPEndPoint.</param>
        /// <returns>The IPAddress.</returns>
        protected IPAddress GetIPAddress(IPEndPoint ep)
        {
            return ep.Address;
        }

        /// <summary>
        /// Ends the Connection after a successful transport.
        /// </summary>
        /// <param name="transporter">The transport socket.</param>
        protected void SuccessfulTransport(Socket transporter)
        {
            try
            {
                if (transporter != null)
                {
                    transporter.Shutdown(SocketShutdown.Both);
                    transporter.Close();
                }
            }
            catch (Exception e)
            {
                throw new SocketBindingException("SuccessfulTransport Exception.", e);
            }
        }
    }
}
