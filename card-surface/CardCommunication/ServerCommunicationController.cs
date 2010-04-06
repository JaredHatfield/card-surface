// <copyright file="ServerCommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the server uses for communication with the table.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Xml;
    using CardGame;
    using Messages;

    /// <summary>
    /// The controller that the server uses for communication with the table.
    /// </summary>
    public class ServerCommunicationController : CommunicationController
    {        
        /// <summary>
        /// list of all clients and their game guids that have communicated with the server.
        /// </summary>
        private Collection<ClientObject> clientList = new Collection<ClientObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCommunicationController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public ServerCommunicationController(IGameController gameController)
            : base(gameController)
        {
        }

        /// <summary>
        /// Sends the state of the game.
        /// </summary>
        /// <param name="game">The game to be transformed to XML.</param>
        /// <returns>whether the message was sent.</returns>
        public bool SendGameStateMessage(Game game)
        {
            bool success = true;

            try
            {
                this.TransportCommunication(game);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending message", e);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Sends the list of playable games.
        /// </summary>
        /// <returns>whether the message was sent.</returns>
        public bool SendGameListMessage()
        {
            bool success = true;

            try
            {
                MessageGameList messageGameList = new MessageGameList();

                messageGameList.BuildMessage(this.GameController.GameTypes);

                this.TransportCommunication(messageGameList.MessageDocument);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending message", e);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Initializes the IP address ports.
        /// </summary>
        protected override void InitializeIPAddressPorts()
        {
            HostReceiveEndPoint = new IPEndPoint(BaseIPAddress, ServerListenerPortNumber);
            HostSendEndPoint = new IPEndPoint(BaseIPAddress, ServerSendPortNumber);
        }

        /// <summary>
        /// Transports the communication.
        /// </summary>
        /// <param name="game">The game to be sent.</param>
        protected override void TransportCommunication(Game game)
        {
            MemoryStream gameStream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            gameStream.Write(GameHeader, 0, GameHeader.Length);
            bf.Serialize(gameStream, game);
            byte[] data = gameStream.ToArray();

            try
            {
                foreach (ClientObject co in this.clientList)
                {
                    if (co.Game == game.Id)
                    {
                        this.StartTransporter();
                        SocketTransporter.Poll(10, SelectMode.SelectWrite);
                        SocketTransporter.Connect(this.RemoteEndPoint);
                        SocketTransporter.Send(data, 0, data.Length, SocketFlags.None);
                        this.SuccessfulTransport();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Sending Communication.", e);
            }
        }

        /// <summary>
        /// Processes the client communication.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected override void ProcessCommunication(IAsyncResult asyncResult)
        {
            try
            {
                Socket socketWorker = (Socket)asyncResult.AsyncState;
                Socket socketProcessor = socketWorker.EndAccept(asyncResult);
                bool found = false;

                foreach (ClientObject co in this.clientList)
                {
                    if (socketProcessor.RemoteEndPoint == co.ClientIPEndPoint)
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    ClientObject co = new ClientObject((IPEndPoint)socketProcessor.RemoteEndPoint);

                    this.clientList.Add(co);
                }

                try
                {
                    CommunicationObject commObject = new CommunicationObject();
                    byte[] data = { };
                    
                    commObject.WorkSocket = socketProcessor;
                    commObject.Data = data;
                    commObject.RemoteIPAddress = GetIPAddress((IPEndPoint)socketProcessor.RemoteEndPoint);

                    ////socketProcessor.Receive(data, 0, CommunicationObject.BufferSize, SocketFlags.None);
                    ////this.ProcessCommunicationData(data);
                    ////this.ProcessCommunicationData(asyncResult);
                    socketProcessor.BeginReceive(
                        commObject.Buffer,
                        0,
                        CommunicationObject.BufferSize,
                        SocketFlags.None,
                        new AsyncCallback(this.ProcessCommunicationData),
                        commObject);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error Receiving Data from client", e);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Receiving Data from client", e);
            }
        }

        /// <summary>
        /// Updates the state of the game.
        /// </summary>
        /// <param name="game">The game update.</param>
        protected override void UpdateGameState(Game game)
        {
            // Should not be implemented because code will never run.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts from XML to message.
        /// </summary>
        /// <param name="messageDoc">The message document.</param>
        /// <param name="remoteIPAddress">The remote IP address.</param>
        protected override void ConvertFromXMLToMessage(XmlDocument messageDoc, IPAddress remoteIPAddress)
        {
            XmlElement message = messageDoc.DocumentElement;
            XmlAttribute messageType;
            IPEndPoint remoteIPEndPoint = new IPEndPoint(remoteIPAddress, ClientListenerPortNumber);
            Guid gameGuid = this.GetGameGuid(remoteIPEndPoint);
            string mt;

            this.RemoteEndPoint = remoteIPEndPoint;
            messageType = message.Attributes[0];

            mt = messageType.Value;

            if (mt == Message.MessageType.RequestGameList.ToString())
            {
                MessageRequestGameList messageRequestGameList = new MessageRequestGameList();

                messageRequestGameList.ProcessMessage(messageDoc);

                MessageGameList messageGameList = new MessageGameList();
                
                messageGameList.BuildMessage(GameController.GameTypes);

                this.TransportCommunication(messageGameList.MessageDocument);
            }
            else if (mt == Message.MessageType.Action.ToString())
            {
                Collection<string> action;
                MessageAction messageAction = new MessageAction();

                messageAction.ProcessMessage(messageDoc);
                action = messageAction.Action;

                if (action[0] == MessageAction.ActionType.Move.ToString())
                {
                    Guid physicalObject = new Guid(action[1]);
                    Guid destinationPile = new Guid(action[2]);

                    // Executes Move
                    GameController.GetGame(gameGuid).MoveAction(physicalObject, destinationPile);
                }
                else if (action[0] == MessageAction.ActionType.Custom.ToString())
                {
                    string actionName = action[1];
                    string playerName = action[2];

                    // Executes Custum Action
                    GameController.GetGame(gameGuid).ExecuteAction(actionName, playerName);
                }
            }
        }

        /// <summary>
        /// Gets the game GUID.
        /// </summary>
        /// <param name="remoteIPEndPoint">The remote IP end point.</param>
        /// <returns>the guid of the game.</returns>
        protected Guid GetGameGuid(IPEndPoint remoteIPEndPoint)
        {
            Guid gameGuid = Guid.Empty;
            bool found = false;

            foreach (ClientObject co in this.clientList)
            {
                if (co.ClientIPEndPoint == remoteIPEndPoint && !found)
                {
                    gameGuid = co.Game;
                    found = true;
                }
            }

            return gameGuid;
        }
    }
}