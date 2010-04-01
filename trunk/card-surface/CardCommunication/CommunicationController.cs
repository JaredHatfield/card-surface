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
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
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
        protected const int ListenerPortNumber = 30567;

        /// <summary>
        /// The Port number of the transporter socket.
        /// </summary>
        protected const int SendPortNumber = 30568;

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
        /// The IPEndPoint of the listnening socket of the remote machine.
        /// </summary>
        private IPEndPoint remoteEndPoint;

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
        /// Gets the game controller.
        /// </summary>
        /// <value>The game controller.</value>
        protected IGameController GameController
        {
            get { return this.gameController; }
        }

        /// <summary>
        /// Gets or sets the remote end point.
        /// </summary>
        /// <value>The remote end point.</value>
        protected IPEndPoint RemoteEndPoint
        {
            get { return this.hostReceiveEndPoint; }
            set { this.remoteEndPoint = value; }
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
                        ////this.TransportCommunication(messageGameState.MessageDocument);
                        this.TransportCommunication(game);
                        break;
                    case Message.MessageType.GameList:
                        MessageGameList messageGameList = new MessageGameList();
                        messageGameList.BuildMessage(this.gameController.GameTypes);
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
            this.SuccessfulTransport();
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
                this.StartListener();
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

                this.socketListener.BeginAccept(new AsyncCallback(this.ProcessClientCommunication), this.socketListener);
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
            ////Use these calls in debug mode before you stop debugging or you will not be able
            ////to bind to the port without restarting your computer.
            ////this.socketTransporter.Shutdown(SocketShutdown.Both);
            ////this.socketTransporter.Close();
            this.socketTransporter.NoDelay = true;
        }

        /// <summary>
        /// Processes the communication data.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected void ProcessCommunicationData(IAsyncResult asyncResult)
        {
            CommunicationObject commObject = (CommunicationObject)asyncResult.AsyncState;
            Socket socketWorker = commObject.WorkSocket;
            int read = socketWorker.EndReceive(asyncResult);

            if (read > 0)
            {
                MemoryStream ms = new MemoryStream();

                // Appends Buffer to Data in commObject.
                ms.Write(commObject.Data, 0, commObject.Data.Length);
                ms.Write(commObject.Buffer, 0, commObject.Buffer.Length);
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
                    
                    BinaryFormatter bf = new BinaryFormatter();
                    Game game = (Game)bf.Deserialize(ms);

                    ////XmlDocument messageDoc = new XmlDocument();

                    ////messageDoc.Load(ms);

                    ////Message m = null;
                    ////Game game = m.ConvertToGame(messageDoc);
                }
            }
            ////Update the game using the gameController.
            ////gameController.Games.
        }

        ////protected void ProcessCommunicationData(byte[] data)
        ////{
        ////    CommunicationObject commObject = (CommunicationObject)asyncResult.AsyncState;
        ////    Socket socketWorker = commObject.workSocket;
        ////    int read = socketWorker.EndReceive(asyncResult);
        ////    byte[] data = commObject.data;// may need to change.

        ////    if (read > 0)
        ////    {
        ////        commObject.data.Concat(data);
        ////        socketWorker.BeginReceive(
        ////            commObject.buffer,
        ////            0,
        ////            CommunicationObject.BufferSize,
        ////            0,
        ////            new AsyncCallback(ProcessCommunicationData),
        ////            commObject);
        ////    }
        ////    else
        ////    {
        ////        if (commObject.data.Length > 1)
        ////        {
        ////            byte[] message = commObject.data;

        ////            MemoryStream ms = new MemoryStream(message);
        ////            XmlDocument messageDoc = new XmlDocument();

        ////            messageDoc.Load(ms);
        ////            Message m = null;
        ////            ////Game game = m.ConvertToGame(messageDoc);
        ////        }
        ////    }
        ////    ////Update the game using the gameController.
        ////    ////gameController.Games.
        ////}

        /// <summary>
        /// Transports the communication.
        /// </summary>
        /// <param name="message">The message document to be transported.</param>
        protected void TransportCommunication(XmlDocument message)
        {            
            MemoryStream ms = new MemoryStream();
            message.Save(ms);
            byte[] data = ms.ToArray();

            try
            {
                this.StartTransporter();
                this.socketTransporter.Poll(10, SelectMode.SelectWrite);
                this.socketTransporter.Connect(this.RemoteEndPoint);
                this.socketTransporter.Send(data, 0, data.Length, SocketFlags.None);
                this.SuccessfulTransport();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Sending Communication.", e);
            }
                ////this.socketTransporter.BeginSend(
                ////data,
                ////0, 
                ////data.Length, 
                ////SocketFlags.None, 
                ////new AsyncCallback(this.SuccessfulTransport), 
                ////data);
        }

        /// <summary>
        /// Transports the communication.
        /// </summary>
        /// <param name="game">The game to be sent.</param>
        protected void TransportCommunication(Game game)
        {
            MemoryStream gameStream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(gameStream, game);
            byte[] data = gameStream.ToArray();

            try
            {
                this.StartTransporter();
                this.socketTransporter.Poll(10, SelectMode.SelectWrite);
                this.socketTransporter.Connect(this.RemoteEndPoint);
                this.socketTransporter.Send(data, 0, data.Length, SocketFlags.None);
                this.SuccessfulTransport();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Sending Communication.", e);
            }
            ////this.socketTransporter.BeginSend(
            ////data,
            ////0, 
            ////data.Length, 
            ////SocketFlags.None, 
            ////new AsyncCallback(this.SuccessfulTransport), 
            ////data);
        }

        /// <summary>
        /// Ends the Connection after a successful transport.
        /// </summary>
        protected void SuccessfulTransport()
        {
            bool reuse = true;

            if (this.socketTransporter != null)
            {
                this.socketTransporter.Shutdown(SocketShutdown.Both);
                this.socketTransporter.Disconnect(reuse);
                this.socketTransporter.Close(30);
            }
        }
    }
}
