// <copyright file="TableCommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the table uses for communication with the server.</summary>
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
    using System.Text;
    using System.Threading;
    using System.Xml;
    using CardGame;
    using CommunicationException;
    using Messages;

    /// <summary>
    /// The controller that the table uses for communication with the server.
    /// </summary>
    public class TableCommunicationController : CommunicationController
    {
        /// <summary>
        /// Holds the type Collection of strings to return
        /// </summary>
        private Collection<string> stringCollection;

        /// <summary>
        /// Holds the Collection of Active Games to return
        /// </summary>
        private Collection<ActiveGameStruct> activeGameStruct;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCommunicationController"/> class.
        /// When a string representation of an IPAddress is not passed, the server is defaulted to
        /// the system's IPAddress.
        /// </summary>
        public TableCommunicationController()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCommunicationController"/> class.
        /// </summary>
        /// <param name="ipaddress">The ip address of the server.</param>
        public TableCommunicationController(string ipaddress)
        {
            MemoryStream ms = new MemoryStream();
            char[] splitChar = { '.' };
            string[] iparray = ipaddress.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            byte[] ip = { };
            try
            {
                foreach (string s in iparray)
                {
                    ms.WriteByte(Byte.Parse(s));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error converting IPAddress." + e.ToString());
                throw new CardCommunicationException("Error converting IPAddress", e);
            }

            ip = ms.ToArray();
            IPAddress serverIPAddress = new IPAddress(ip);
            RemoteEndPoint = new IPEndPoint(serverIPAddress, ServerListenerPortNumber);
        }

        /// <summary>
        /// Delegate for Updating the list of availiable games.
        /// </summary>
        /// <param name="gameList">The game list.</param>
        public delegate void UpdateGameListHandler(Collection<string> gameList);

        /// <summary>
        /// Delegate for Updating the list of existing games.
        /// </summary>
        /// <param name="existingGames">The list of existing games.</param>
        public delegate void UpdateExistingGamesHandler(Collection<ActiveGameStruct> existingGames);

        /// <summary>
        /// Delegate for updating the game state.
        /// </summary>
        /// <param name="game">The game update.</param>
        public delegate void UpdateGameStateHandler(Game game);

        /// <summary>
        /// Event occurs when game list is updated.
        /// </summary>
        public event UpdateGameListHandler OnUpdateGameList;

        /// <summary>
        /// Event occurs when existing games is updated.
        /// </summary>
        public event UpdateExistingGamesHandler OnUpdateExistingGames;

        /// <summary>
        /// Event occurs when Game State is updated.
        /// </summary>
        public event UpdateGameStateHandler OnUpdateGameState;

        /// <summary>
        /// Sends the request game list message.
        /// </summary>
        /// <returns>the gameList</returns>
        public Collection<string> SendRequestGameListMessage()
        {
            try
            {
                MessageRequestGameList message = new MessageRequestGameList();

                this.CommunicationCompleted = false;

                message.BuildMessage();

                base.TransportCommunication(message.MessageDocument);

                while (!CommunicationCompleted)
                {
                }

                return this.stringCollection;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Requesting game list. " + e.ToString());
                throw new MessageTransportException("Error Requesting game list. ", e);
            }
        }

        /// <summary>
        /// Sends the request seat code change message.
        /// </summary>
        /// <param name="seatGuid">The seat GUID.</param>
        public void SendRequestSeatCodeChange(Guid seatGuid)
        {
            try
            {
                MessageRequestSeatCodeChange message = new MessageRequestSeatCodeChange();

                message.BuildMessage(seatGuid);

                base.TransportCommunication(message.MessageDocument);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Requesting seat code change. " + e.ToString());
                throw new MessageTransportException("Error Requesting seat code change. ", e);
            }
        }

        /// <summary>
        /// Sends the request existing games.
        /// </summary>
        /// <param name="selectedGame">The selected game.</param>
        /// <returns>the collection of existing games.</returns>
        public Collection<ActiveGameStruct> SendRequestExistingGames(string selectedGame)
        {
            try
            {
                MessageRequestExistingGames message = new MessageRequestExistingGames();

                this.CommunicationCompleted = false;

                message.BuildMessage(selectedGame);

                base.TransportCommunication(message.MessageDocument);

                while (!CommunicationCompleted)
                {
                }

                return this.activeGameStruct;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Requesting list of active games. " + e.ToString());
                throw new MessageTransportException("Error Requesting list of active games. ", e);
            }
        }

        /// <summary>
        /// Sends the state of the request current game.
        /// </summary>
        /// <param name="gameGuid">The game GUID.</param>
        public void SendRequestCurrentGameState(Guid gameGuid)
        {
            try
            {
                MessageRequestCurrentGameState message = new MessageRequestCurrentGameState();

                message.BuildMessage(gameGuid);

                base.TransportCommunication(message.MessageDocument);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Requesting current game state. " + e.ToString());
                throw new MessageTransportException("Error Requesting current game state. ", e);
            }
        }

        /// <summary>
        /// Sends the action message.
        /// </summary>
        /// <param name="action">The action.</param>
        public void SendActionMessage(Collection<string> action)
        {
            try
            {
                MessageAction message = new MessageAction();

                message.BuildMessage(action);

                base.TransportCommunication(message.MessageDocument);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending action message. " + e.ToString());
                throw new MessageTransportException("Error sending action message. ", e);
            }
        }

        /// <summary>
        /// Sends the move action message.
        /// </summary>
        /// <param name="objectGuid">The object GUID.</param>
        /// <param name="pileGuid">The pile GUID.</param>
        public void SendMoveActionMessage(string objectGuid, string pileGuid)
        {
            try
            {
                Collection<string> action = new Collection<string>();

                action.Add(MessageAction.ActionType.Move.ToString());
                action.Add(objectGuid);
                action.Add(pileGuid);

                this.SendActionMessage(action);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending move action message. " + e.ToString());
                throw new MessageTransportException("Error sending move action message. ", e);
            }
        }

        /// <summary>
        /// Sends the custom action message.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="playerName">Name of the player.</param>
        public void SendCustomActionMessage(string actionName, string playerName)
        {
            try
            {
                Collection<string> action = new Collection<string>();

                action.Add(MessageAction.ActionType.Custom.ToString());
                action.Add(actionName);
                action.Add(playerName);

                this.SendActionMessage(action);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending custom action message. " + e.ToString());
                throw new MessageTransportException("Error sending custom action message. ", e);
            }
        }

        /// <summary>
        /// Sends the flip card message.
        /// </summary>
        /// <param name="cardGuid">The card GUID.</param>
        public void SendFlipCardMessage(Guid cardGuid)
        {
            try
            {
                MessageFlipCard message = new MessageFlipCard();

                message.BuildMessage(cardGuid);

                base.TransportCommunication(message.MessageDocument);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error sending flip card message. " + e.ToString());
                throw new MessageTransportException("Error sending flip card message. ", e);
            }
        }

        /// <summary>
        /// Sends the request game message.
        /// </summary>
        /// <param name="game">The game to join or create.</param>
        internal void SendRequestGameMessage(ActiveGameStruct game)
        {
            try
            {
                MessageRequestGame message = new MessageRequestGame();

                this.CommunicationCompleted = false;

                if (game.Id != Guid.Empty)
                {
                    message.BuildMessage(game.Id);
                }
                else
                {
                    message.BuildMessage(game.GameType);
                }

                base.TransportCommunication(message.MessageDocument);

                while (!this.CommunicationCompleted)
                {
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Requesting game list. " + e.ToString());
                throw new MessageTransportException("Error Requesting game list. ", e);
            }
        }

        /// <summary>
        /// Called when game list is updated.
        /// </summary>
        /// <param name="gameList">The game list.</param>
        protected void OnUpdatedGameList(object gameList)
        {
            if (this.OnUpdateGameList != null)
            {
                /* The gameList object must be casted as a collection because the method signature
                 * requires an object parameter to function as a ParameterizedThreadStart. */
                this.OnUpdateGameList((Collection<string>) gameList);
            }
        }

        /// <summary>
        /// Called when [updated existing games].
        /// </summary>
        /// <param name="existingGames">The existing games.</param>
        protected void OnUpdatedExistingGames(object existingGames)
        {
            if (this.OnUpdateExistingGames != null)
            {
                /* The existingGames object must be casted as a collection because the method signature
                 * requires an object parameter to function as a ParameterizedThreadStart. */
                this.OnUpdateExistingGames((Collection<ActiveGameStruct>)existingGames);
            }
        }

        /// <summary>
        /// Called when game state is updated.
        /// </summary>
        /// <param name="game">The game update.</param>
        protected void OnUpdatedGameState(object game)
        {
            if (this.OnUpdateGameState != null)
            {
                /* The game object must be casted as a collection because the method signature
                 * requires an object parameter to function as a ParameterizedThreadStart. */
                this.OnUpdateGameState((Game)game);
            }
        }

        /// <summary>
        /// Initializes the IP address ports.
        /// </summary>
        protected override void InitializeIPAddressPorts()
        {
            HostReceiveEndPoint = new IPEndPoint(BaseIPAddress, ClientListenerPortNumber);
            RemoteEndPoint = new IPEndPoint(BaseIPAddress, ServerListenerPortNumber);
        }

        /// <summary>
        /// Transports the communication.
        /// </summary>
        /// <param name="game">The game to be sent.</param>
        protected override void TransportCommunication(Game game)
        {
            //// There is no reason this function should be implemented.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes the client communication.
        /// </summary>
        /// <param name="asyncResult">The async result.</param>
        protected override void ProcessCommunication(IAsyncResult asyncResult)
        {
            try
            {
                Monitor.Enter(this.ProcessCommSephamore);

                Socket socketProcessor = this.SocketListener.EndAccept(asyncResult);
                CommunicationObject commObject = new CommunicationObject();
                byte[] data = { };

                commObject.WorkSocket = socketProcessor;
                commObject.Data = data;
                commObject.RemoteIPAddress = GetIPAddress((IPEndPoint)socketProcessor.RemoteEndPoint);
                              
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
                throw new MessageProcessException("Table.ProcessCommunication Exception.", e);
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
            /* Because OnUpdatedGameList() triggers an event in the table components,
             * we need to call this from an STA thread to enable UI updates. */
            Thread onUpdateGameListThread = new Thread(new ParameterizedThreadStart(this.OnUpdatedGameState));
            onUpdateGameListThread.Name = "UpdateGameStateThread";
            onUpdateGameListThread.SetApartmentState(ApartmentState.STA);
            onUpdateGameListThread.Start(game);
            onUpdateGameListThread.Join();      // Wait for this event call to finish        
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
                ////IPEndPoint remoteIPEndPoint = new IPEndPoint(remoteIPAddress, ClientListenerPortNumber);
                string mt;

                ////this.RemoteEndPoint = remoteIPEndPoint;
                messageType = message.Attributes[0];

                mt = messageType.Value;

                if (mt == Message.MessageType.GameList.ToString())
                {
                    MessageGameList messageGameList = new MessageGameList();

                    messageGameList.ProcessMessage(messageDoc);

                    this.stringCollection = messageGameList.GameNameList;

                    /* Because OnUpdatedGameList() triggers an event in the table components,
                     * we need to call this from an STA thread to enable UI updates. */
                    Thread onUpdateGameListThread = new Thread(new ParameterizedThreadStart(this.OnUpdatedGameList));
                    onUpdateGameListThread.Name = "UpdateGameListThread";
                    onUpdateGameListThread.SetApartmentState(ApartmentState.STA);
                    onUpdateGameListThread.Start(this.stringCollection);
                    onUpdateGameListThread.Join();      // Wait for this event call to finish
                }
                else if (mt == Message.MessageType.ExistingGames.ToString())
                {
                    MessageExistingGames messageExistingGames = new MessageExistingGames();

                    messageExistingGames.ProcessMessage(messageDoc);
                    
                    this.activeGameStruct = messageExistingGames.ActiveGames;

                    if (this.OnUpdateExistingGames != null)
                    {
                        this.OnUpdateExistingGames(messageExistingGames.ActiveGames);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw new MessageProcessException("Table.ConvertFromXMLToMessage Exception.", e);
            }
        }
    }
}
