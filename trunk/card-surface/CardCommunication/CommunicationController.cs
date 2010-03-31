// <copyright file="CommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The base class for the communication controller.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Xml;
    using CardGame;
    ////using GameObject;
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
        protected const int ListenerPortNumber = 4;

        /// <summary>
        /// The Port number of the transporter socket.
        /// </summary>
        protected const int SendPortNumber = 6;

        /// <summary>
        /// The listening socket.
        /// </summary>
        private Socket socketListener = null;

        /// <summary>
        /// The transporter socket.
        /// </summary>
        private Socket socketTransporter = null;

        /// <summary>
        /// The IPEndPoint of the listening socket.
        /// </summary>
        private IPEndPoint hostReceiveEndPoint;

        /// <summary>
        /// The IPEndPoint of the trasnporter socket.
        /// </summary>
        private IPEndPoint hostSendEndPoint;

        /// <summary>
        /// The GameController that the communication controller is able to interact with.
        /// </summary>
        private IGameController gameController;

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
        /// Gets the socket listener.
        /// </summary>
        /// <value>The socket listener.</value>
        protected Socket SocketListener
        {
            get { return this.socketListener; }
        }

        /// <summary>
        /// Sends the state of the game.
        /// </summary>
        /// <param name="game">The game to be transformed to XML.</param>
        /// <param name="type">The type of message to be sent.</param>
        /// <returns>whether the message was sent.</returns>
        public bool SendMessage(Game game, Message.MessageType type)
        {
            bool success = true;

            try
            {
                switch (type)
                {
                    case Message.MessageType.Action:
                        MessageAction messageAction = new MessageAction();
                        messageAction.BuildMessage(game);
                        this.TransportCommunication(messageAction.MessageDocument);
                        break;
                    case Message.MessageType.GameState:
                        MessageGameState messageGameState = new MessageGameState();
                        messageGameState.BuildMessage(game);
                        this.TransportCommunication(messageGameState.MessageDocument);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending message", e);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Close(object sender, EventArgs args)
        {
            // TODO: Close all open connections because the program is about to close
        }

        /// <summary>
        /// Initializes the communication controller.
        /// </summary>
        protected void InitializeCommunicationController()
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

            this.hostReceiveEndPoint = new IPEndPoint(hostAddress, ListenerPortNumber);
            this.hostSendEndPoint = new IPEndPoint(hostAddress, SendPortNumber);

            if (this.socketListener == null)
            {
                this.StartListener(this.hostReceiveEndPoint);
            }

            if (this.socketTransporter == null)
            {
                this.StartTransporter();
            }
        }

        /// <summary>
        /// Processes the client communication.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected abstract void ProcessClientCommunication(IAsyncResult asyncResult);

        /// <summary>
        /// Starts the listener socket.
        /// </summary>
        /// <param name="hostEndPoint">The host end point.</param>
        /// <returns>whether the listner socket was started.</returns>
        protected bool StartListener(IPEndPoint hostEndPoint)
        {
            bool success = true;

            try
            {
                if (this.socketListener == null)
                {
                    ////SocketInformation socketInfo = new SocketInformation();

                    ////socketInfo.Options = SocketInformationOptions.Listening;

                    ////socketInfo.ProtocolInformation =
                    this.socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.socketListener.Bind(hostEndPoint);
                    this.socketListener.Listen(10);
                    this.socketListener.BeginAccept(new AsyncCallback(this.ProcessClientCommunication), null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error starting socket listener.", e);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Starts the transporter socket.
        /// </summary>
        protected void StartTransporter()
        {
            this.socketTransporter = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socketTransporter.Bind(this.hostSendEndPoint);
            ////socketTransporter.EnableBroadcast = true;
            this.socketTransporter.NoDelay = true;
        }

        /// <summary>
        /// Processes the communication data.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected void ProcessCommunicationData(IAsyncResult asyncResult)
        {
            byte[] data = (byte[])asyncResult.AsyncState;
            MemoryStream ms = new MemoryStream(data);
            XmlDocument messageDoc = new XmlDocument();
            int countRx = this.socketListener.EndReceive(asyncResult);

            messageDoc.Load(ms);
            Message m = null;
            Game game = m.ConvertToGame(messageDoc);
            ////Update the game using the gameController.
            ////gameController.Games.
        }

        /// <summary>
        /// Transports the communication.
        /// </summary>
        /// <param name="message">The message document to be transported.</param>
        protected void TransportCommunication(XmlDocument message)
        {            
            MemoryStream ms = new MemoryStream();
            message.Save(ms);
            byte[] data = ms.ToArray();

            this.socketTransporter.Poll(10, SelectMode.SelectWrite);
            this.socketTransporter.Connect(this.hostReceiveEndPoint);
            this.socketTransporter.BeginSend(
                data,
                0, 
                data.Length, 
                SocketFlags.None, 
                new AsyncCallback(this.SuccessfulTransport), 
                data);
        }

        /// <summary>
        /// Successfuls the transport.
        /// </summary>
        /// <param name="asynchResult">The asynch result.</param>
        protected void SuccessfulTransport(IAsyncResult asynchResult)
        {
            bool reuse = true;

            this.socketTransporter.EndSend(asynchResult);
            this.socketTransporter.Disconnect(reuse);
        }
    }
}
