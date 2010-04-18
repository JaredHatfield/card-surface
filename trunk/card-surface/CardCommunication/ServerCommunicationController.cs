// <copyright file="ServerCommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the server uses for communication with the table.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using CardGame;
    using CardGame.GameException;
    using CommunicationException;
    using GameObject;
    using Messages;

    /// <summary>
    /// The controller that the server uses for communication with the table.
    /// </summary>
    public class ServerCommunicationController : CommunicationController
    {
        /// <summary>
        /// The GameController.
        /// </summary>
        private IGameController gameController;

        /// <summary>
        /// The thread that is responsible for listening for new connections.
        /// </summary>
        private Thread serverListenerLoopThread;

        /// <summary>
        /// The set of threads that represent connected clients.
        /// </summary>
        private Collection<Thread> connectedClients;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCommunicationController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public ServerCommunicationController(IGameController gameController)
            : base()
        {
            this.gameController = gameController;
            this.connectedClients = new Collection<Thread>();

            // Set up the server
            TcpListener tcpServerListener = new TcpListener(IPAddress.Any, CommunicationController.ServerListenerPortNumber);
            tcpServerListener.ExclusiveAddressUse = false;
            Console.WriteLine("Server Started");
            tcpServerListener.Start(10);

            this.serverListenerLoopThread = new Thread(this.ServerListenerLoop);
            this.serverListenerLoopThread.Name = "Server Listener Loop";
            this.serverListenerLoopThread.Start(tcpServerListener);
        }

        /// <summary>
        /// Gets the IP address.
        /// </summary>
        /// <value>The IP address.</value>
        public string IP
        {
            get { return "This should return the IP address."; }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void Close(object sender, EventArgs args)
        {
            this.serverListenerLoopThread.Abort();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the game state message.
        /// </summary>
        /// <param name="game">The game to send.</param>
        /// <returns>True if the message was sent; otherwise false</returns>
        public bool SendGameStateMessage(Game game)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This is the infinite loop that listens for new clients that connect to the server.
        /// When a new client connects it gets its own thread.
        /// </summary>
        /// <param name="listener">The listener.</param>
        private void ServerListenerLoop(object listener)
        {
            TcpListener tcpServerListener = listener as TcpListener;
            int id = 0;
            while (true)
            {
                // Wait for someone to connect
                TcpClient c = tcpServerListener.AcceptTcpClient();
                Debug.WriteLine("Someone connected to the server...");

                // Create an object to manage the connection and start 
                ConnectedClient cc = new ConnectedClient(c, id++);
                Thread t = new Thread(this.ClientProcessor);
                this.connectedClients.Add(t);
                t.Name = "Connected Client " + id;
                t.Start(cc);
            }
        }

        /// <summary>
        /// Clients the processor.
        /// </summary>
        /// <param name="connectedClient">The connected client.</param>
        private void ClientProcessor(object connectedClient)
        {
            ConnectedClient cc = connectedClient as ConnectedClient;
            
            // TODO: this is where all of the magic happens.
            while (true)
            {
                Debug.WriteLine("Server: Client " + cc.Id + " is waiting for a message");
                string input = cc.GetNextMessage();
                Debug.WriteLine("Server: Client " + cc.Id + " has a message to process");
                byte[] byteArray = Encoding.ASCII.GetBytes(input);
                MemoryStream ms = new MemoryStream(byteArray);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);

                XmlElement message = messageDoc.DocumentElement;
                string mt = message.Attributes[0].Value;

                if (mt == Message.MessageType.RequestGameList.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestGameListMessage");
                    MessageRequestGameList messageRequestGameList = new MessageRequestGameList();
                    messageRequestGameList.ProcessMessage(messageDoc);
                    MessageGameList messageGameList = new MessageGameList();
                    messageGameList.BuildMessage(this.gameController.GameTypes);
                    Debug.WriteLine("Server: Client " + cc.Id + " returned the list of games");
                    cc.SendMessage(messageGameList.MessageDocument.InnerXml);
                }
            }
        }
    }
}