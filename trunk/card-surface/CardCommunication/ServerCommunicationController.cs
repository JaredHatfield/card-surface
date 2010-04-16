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
    using CommunicationException;
    using GameObject;
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
                Debug.WriteLine("Error sending message" + e.ToString());
                success = false;
                throw new MessageProcessException("Server.SendGameStateMessage Exception.", e);
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

                base.TransportCommunication(messageGameList.MessageDocument);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending message" + e.ToString());
                success = false;
                throw new MessageProcessException("Server.SendGameListMessage Exception.", e);
            }

            return success;
        }

        /// <summary>
        /// Sends the existing games message.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        /// <returns>whether the message was sent.</returns>
        public bool SendExistingGamesMessage(string gameType)
        {
            bool success = true;

            try
            {
                MessageExistingGames messageExistingGames = new MessageExistingGames();
                Collection<Collection<string>> existingGames = new Collection<Collection<string>>();
                Collection<string> newGame = new Collection<string>();

                newGame.Add("New Game");
                newGame.Add(Guid.Empty.ToString());
                newGame.Add(String.Empty);

                existingGames.Add(newGame);

                foreach (Game game in this.GameController.Games)
                {
                    Collection<string> gameObject = new Collection<string>();

                    if (game.Name == gameType)
                    {
                        gameObject.Add(game.Name);
                        gameObject.Add(game.Id.ToString());
                        gameObject.Add(game.NumberOfPlayers + "/" + game.NumberOfSeats);
                        //// gameObject.Add(game.location);
                    }

                    existingGames.Add(gameObject);
                }

                messageExistingGames.BuildMessage(existingGames);

                base.TransportCommunication(messageExistingGames.MessageDocument);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending message" + e.ToString());
                success = false;
                throw new MessageProcessException("Server.SendGameListMessage Exception.", e);
            }

            return success;
        }

        /// <summary>
        /// Initializes the IP address ports.
        /// </summary>
        protected override void InitializeIPAddressPorts()
        {
            HostReceiveEndPoint = new IPEndPoint(BaseIPAddress, ServerListenerPortNumber);
        }

        /// <summary>
        /// Transports the communication.
        /// </summary>
        /// <param name="game">The game to be sent.</param>
        protected override void TransportCommunication(Game game)
        {
            try
            {
                MemoryStream gameStream = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                Game gameNetworkClient = new GameMessage(game);
                byte[] data;

                try
                {
                    gameStream.Write(GameHeader, 0, GameHeader.Length);
                    bf.Serialize(gameStream, gameNetworkClient);
                    data = gameStream.ToArray();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error serializing game. " + e.ToString());
                    throw new MessageTransportException("Error serializing game. ", e);
                }

                /* Send Game State to every table that are playing this game. */
                foreach (ClientObject co in this.clientList)
                {
                    if (co.Game == game.Id)
                    {
                        RemoteEndPoint = new IPEndPoint(co.ClientIPAddress, ClientListenerPortNumber);

                        Socket transporter = this.StartTransporter();
                        transporter.Poll(10, SelectMode.SelectWrite);
                        transporter.Send(data, 0, data.Length, SocketFlags.None);
                        this.SuccessfulTransport(transporter);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Sending Communication." + e.ToString());
                throw new MessageProcessException("Server.TransportCommunication Exception.", e);
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
                IPEndPoint remoteEP = (IPEndPoint)socketProcessor.RemoteEndPoint;
                IPAddress address = remoteEP.Address;

                foreach (ClientObject co in this.clientList)
                {
                    if (address.Equals(co.ClientIPAddress))
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    ClientObject co = new ClientObject(address);

                    this.clientList.Add(co);
                }

                CommunicationObject commObject = new CommunicationObject();
                byte[] data = { };
                
                commObject.WorkSocket = socketProcessor;
                commObject.Data = data;
                commObject.RemoteIPAddress = GetIPAddress((IPEndPoint)socketProcessor.RemoteEndPoint);

                Monitor.Enter(this.ProcessCommSephamore);

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
                Debug.WriteLine("Error Receiving Data from client" + e.ToString());
                throw new MessageProcessException("Server.ProcessCommunication Exception.", e);
            }
        }

        /// <summary>
        /// Sets the communication completed.
        /// </summary>
        protected override void SetCommunicationCompleted()
        {
            CommunicationCompleted = true;
            this.SocketListener.BeginAccept(new AsyncCallback(this.ProcessCommunication), this.SocketListener);
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
            try
            {
                XmlElement message = messageDoc.DocumentElement;
                XmlAttribute messageType;
                IPEndPoint remoteIPEndPoint = new IPEndPoint(remoteIPAddress, ClientListenerPortNumber);
                Guid gameGuid = this.GetGameGuid(remoteIPAddress);
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

                    base.TransportCommunication(messageGameList.MessageDocument);
                }
                else if (mt == Message.MessageType.RequestGame.ToString())
                {
                    MessageRequestGame messageRequestGame = new MessageRequestGame();

                    messageRequestGame.ProcessMessage(messageDoc);

                    if (messageRequestGame.GameType != null)
                    {
                        string gameType = messageRequestGame.GameType;
                        bool found = false;

                        Guid newGame = GameController.CreateGame(gameType);

                        foreach (ClientObject co in this.clientList)
                        {
                            if (co.ClientIPAddress.Equals(remoteIPAddress) && !found)
                            {
                                co.Game = newGame;
                                found = true;
                            }
                        }

                        this.TransportCommunication(GameController.GetGame(newGame));
                    }
                    else
                    {
                        Guid selectedGameGuid = messageRequestGame.GameGuid;
                        bool found = false;

                        foreach (ClientObject co in this.clientList)
                        {
                            if (co.ClientIPAddress.Equals(remoteIPAddress) && !found)
                            {
                                co.Game = selectedGameGuid;
                                found = true;
                            }
                        }

                        this.TransportCommunication(GameController.GetGame(selectedGameGuid));
                    }
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
                        try
                        {
                            GameController.GetGame(gameGuid).MoveAction(physicalObject, destinationPile);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                            throw new MessageProcessException("Error executing move action.", e);
                        }

                        this.TransportCommunication(GameController.GetGame(gameGuid));
                    }
                    else if (action[0] == MessageAction.ActionType.Custom.ToString())
                    {
                        string actionName = action[1];
                        string playerName = action[2];

                        // Executes Custum Action
                        try
                        {
                            GameController.GetGame(gameGuid).ExecuteAction(actionName, playerName);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                            throw new MessageProcessException("Error executing custom action.", e);
                        }

                        this.TransportCommunication(GameController.GetGame(gameGuid));
                    }
                }
                else if (mt == Message.MessageType.FlipCard.ToString())
                {
                    MessageFlipCard messageFlipCard = new MessageFlipCard();

                    messageFlipCard.ProcessMessage(messageDoc);

                    Guid cardGuid = messageFlipCard.CardGuid;

                    GameController.GetGame(gameGuid).FlipCard(cardGuid);
                }
                else if (mt == Message.MessageType.RequestExistingGames.ToString())
                {
                    MessageRequestExistingGames messageRequestExistingGames = new MessageRequestExistingGames();
                    Collection<Collection<string>> games = new Collection<Collection<string>>();
                    Collection<string> newGame = new Collection<string>();

                    messageRequestExistingGames.ProcessMessage(messageDoc);

                    newGame.Add(messageRequestExistingGames.SelectedGame);
                    newGame.Add("New Game");
                    newGame.Add(Guid.Empty.ToString());
                    newGame.Add("0");

                    games.Add(newGame);

                    foreach (Game game in GameController.Games)
                    {
                        if (game.Name == messageRequestExistingGames.SelectedGame)
                        {
                            Collection<string> gameObject = new Collection<string>();

                            gameObject.Add(game.Name);
                            gameObject.Add(game.Name);
                            gameObject.Add(game.Id.ToString());
                            gameObject.Add(game.NumberOfPlayers + "/" + game.Seats.Count);

                            games.Add(gameObject);
                        }
                    }

                    MessageExistingGames messageExistingGames = new MessageExistingGames();

                    messageExistingGames.BuildMessage(games);

                    base.TransportCommunication(messageExistingGames.MessageDocument);
                }
                else if (mt == Message.MessageType.RequestCurrentGameState.ToString())
                {
                    MessageRequestCurrentGameState mrcgs = new MessageRequestCurrentGameState();

                    mrcgs.ProcessMessage(messageDoc);

                    this.TransportCommunication(this.GameController.GetGame(mrcgs.GameGuid));
                }
                else if (mt == Message.MessageType.RequestSeatCodeChange.ToString())
                {
                    MessageRequestSeatCodeChange mrscc = new MessageRequestSeatCodeChange();

                    mrscc.ProcessMessage(messageDoc);

                    //// TODO: Code to update the seat code.

                    this.TransportCommunication(this.GameController.GetGame(gameGuid));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw new MessageProcessException("Server.ConvertXMLToMessage Exception.", e);
            }
        }

        /// <summary>
        /// Gets the game GUID.
        /// </summary>
        /// <param name="remoteIPAddress">The remote IP address.</param>
        /// <returns>the guid of the game.</returns>
        protected Guid GetGameGuid(IPAddress remoteIPAddress)
        {
            Guid gameGuid = Guid.Empty;
            bool found = false;

            foreach (ClientObject co in this.clientList)
            {
                if (co.ClientIPAddress.Equals(remoteIPAddress) && !found)
                {
                    gameGuid = co.Game;
                    found = true;
                }
            }

            return gameGuid;
        }
    }
}