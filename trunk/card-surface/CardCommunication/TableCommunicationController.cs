// <copyright file="TableCommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the table uses for communication with the server.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
                Console.WriteLine("Error converting IPAddress.", e);
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
        public delegate void UpdateExistingGamesHandler(Collection<string> existingGames); //// don't know contents yet.

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
            MessageRequestGameList message = new MessageRequestGameList();

            this.CommunicationCompleted = false;

            message.BuildMessage();

            this.TransportCommunication(message.MessageDocument);

            while (!CommunicationCompleted)
            {
            }

            return this.stringCollection;
        }

        /// <summary>
        /// Sends the request join or create a game.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        public void SendRequestGameMessage(string gameType)
        {
            MessageRequestGame message = new MessageRequestGame();

            this.CommunicationCompleted = false;

            message.BuildMessage(gameType);

            /* TODO: Why are you using this instead of base? */
            this.TransportCommunication(message.MessageDocument);
        }

        /// <summary>
        /// Sends the request existing games.
        /// </summary>
        /// <param name="selectedGame">The selected game.</param>
        public void SendRequestExistingGames(string selectedGame)
        {
            MessageRequestExistingGames message = new MessageRequestExistingGames();

            message.BuildMessage(selectedGame);

            this.TransportCommunication(message.MessageDocument);

            while (!CommunicationCompleted)
            {
            }
        }

        /// <summary>
        /// Sends the action message.
        /// </summary>
        /// <param name="action">The action.</param>
        public void SendActionMessage(Collection<string> action)
        {
            MessageAction message = new MessageAction();

            message.BuildMessage(action);

            this.TransportCommunication(message.MessageDocument);
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
                this.OnUpdateExistingGames((Collection<string>)existingGames);
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
            catch (Exception)
            {
                throw new MessageProcessException("Table.ProcessCommunication.");
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

                ////else if (mt == Message.MessageType.RequestExistingGames.ToString())
                ////{
                ////    MessageRequestExistingGames messageRequestExistingGames = new MessageRequestExistingGames();

                ////    messageRequestExistingGames.ProcessMessage(messageDoc);

                ////    this.OnUpdateExistingGames(messageRequestExistingGames.
                ////}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new MessageProcessException("Table.ConvertFromXMLToMessage");
            }
        }
    }
}
