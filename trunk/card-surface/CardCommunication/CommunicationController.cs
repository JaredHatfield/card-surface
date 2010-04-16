﻿// <copyright file="CommunicationController.cs" company="University of Louisville Speed School of Engineering">
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
        /// The listening socket.
        /// </summary>
        private Socket socketListener = null;

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
        /// The header to the serialized game object.
        /// </summary>
        private byte[] gameHeader = { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255 };

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationController"/> class.
        /// </summary>
        internal CommunicationController()
        {
            this.InitializeCommunicationController();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        internal CommunicationController(IGameController gameController)
        {
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
        /// Gets the socket listener.
        /// </summary>
        /// <value>The socket listener.</value>
        protected Socket SocketListener
        {
            get { return this.socketListener; }
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
        /// Gets the game header.
        /// </summary>
        /// <value>The game header.</value>
        protected byte[] GameHeader
        {
            get { return this.gameHeader; }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Close(object sender, EventArgs args)
        {
            /* TODO: This should close the listener! */
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

                if (this.socketListener == null)
                {
                    this.StartListener();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error initializing communication controller. " + e.ToString());
                throw new MessageTransportException("Error initializing communication controller. ", e);
            }          
        }

        /// <summary>
        /// Updates the state of the game.
        /// </summary>
        /// <param name="game">The updated game state.</param>
        protected abstract void UpdateGameState(Game game);

        /// <summary>
        /// Processes the client communication.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected abstract void ProcessCommunication(IAsyncResult asyncResult);

        /// <summary>
        /// Starts the listener socket.
        /// </summary>
        /// <returns>whether the listner socket was started.</returns>
        protected bool StartListener()
        {
            bool success = true;

            try
            {
                if (this.socketListener == null)
                {
                    this.socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.socketListener.Bind(this.hostReceiveEndPoint);
                    this.socketListener.Listen(10);
                }

                this.socketListener.BeginAccept(new AsyncCallback(this.ProcessCommunication), this.socketListener);
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
        /// Starts the transporter socket.
        /// </summary>
        /// <returns>A new Socket</returns>
        protected Socket StartTransporter()
        {
            try
            {
                Socket socketTransporter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socketTransporter.Connect(this.RemoteEndPoint);
                socketTransporter.NoDelay = true;

                return socketTransporter;
            }
            catch (Exception e)
            {
                throw new SocketBindingException("Error binding to socket.", e);
            }
        }

        /// <summary>
        /// Processes the communication data.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected void ProcessCommunicationData(IAsyncResult asyncResult)
        {
            try
            {
                CommunicationObject commObject = (CommunicationObject)asyncResult.AsyncState;
                Socket socketWorker = commObject.WorkSocket;
                int read = socketWorker.EndReceive(asyncResult);

                if (read > 0)
                {
                    MemoryStream ms = new MemoryStream();
                    //// Appends Buffer to Data in commObject.
                    ms.Write(commObject.Data, 0, commObject.Data.Length);
                    ms.Write(commObject.Buffer, 0, commObject.Buffer.Length);

                    if (commObject.FirstKB)
                    {
                        byte[] header = new byte[this.gameHeader.Length];
                        byte[] buffer = new byte[this.gameHeader.Length];

                        Array.Copy(this.gameHeader, header, this.gameHeader.Length);
                        Array.Copy(commObject.Buffer, 0, buffer, 0, this.gameHeader.Length);

                        bool same = true;

                        for (int i = 0; i < header.Length && same; i++)
                        {
                            if (header[i] != buffer[i])
                            {
                                same = false;
                            }
                        }

                        if (same)
                        {
                            byte[] temp = new byte[ms.ToArray().Length];
                            byte[] newMS = new byte[temp.Length - this.gameHeader.Length];
                            
                            //// Needs to check if first bytes are equal to the header and remove
                            temp = ms.ToArray();

                            for (int i = 0; i < newMS.Length; i++)
                            {
                                newMS[i] = temp[i + 15];
                            }

                            ms = new MemoryStream();

                            ms.Write(newMS, 0, newMS.Length);

                            commObject.GameState = true;
                        }

                        commObject.FirstKB = false;
                    }

                    commObject.Data = ms.ToArray();

                    socketWorker.BeginReceive(
                        commObject.Buffer,
                        0,
                        CommunicationObject.BufferSize,
                        0,
                        new AsyncCallback(this.ProcessCommunicationData),
                        commObject);
                }
                else
                {
                    if (commObject.Data.Length > 1)
                    {
                        byte[] message = commObject.Data;

                        MemoryStream ms = new MemoryStream(message);

                        if (commObject.GameState)
                        {
                            Game game;
                            try
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                ////bf.Binder = new AllowAllVersionsDeserializationBinder();

                                game = (Game)bf.Deserialize(ms);
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine("Error deserializing game. " + e.ToString());
                                throw new MessageTransportException("Error deserializing game. ", e);
                            }

                            this.UpdateGameState(game);
                        }
                        else
                        {
                            XmlDocument messageDoc = new XmlDocument();

                            messageDoc.Load(ms);

                            this.ConvertFromXMLToMessage(messageDoc, commObject.RemoteIPAddress);
                        }

                        this.SetCommunicationCompleted();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
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
                MemoryStream ms = new MemoryStream();
                message.Save(ms);
                byte[] data = ms.ToArray();

                Socket transporter = this.StartTransporter();
                transporter.Poll(10, SelectMode.SelectWrite);

                transporter.Send(data, 0, data.Length, SocketFlags.None);
                this.SuccessfulTransport(transporter);
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
            IPAddress ip = ep.Address;

            return ip;
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
