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
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Formatters.Soap;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using CardGame;
    using CommunicationException;
    using GameObject;
    using Messages;

    /// <summary>
    /// The controller that the table uses for communication with the server.
    /// </summary>
    public class TableCommunicationController : CommunicationController
    {
        /// <summary>
        /// The stream that reads data from the server.
        /// </summary>
        private StreamReader clientStreamReader;

        /// <summary>
        /// The stream that writes data to the server.
        /// </summary>
        private StreamWriter clientStreamWriter;

        /// <summary>
        /// The tcp client that is connected to the server.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCommunicationController"/> class.
        /// </summary>
        public TableCommunicationController()
            : base()
        {
            this.tcpClient = new TcpClient("localhost", CommunicationController.ServerListenerPortNumber);
            Debug.WriteLine("Client: connected to the server.");
            NetworkStream clientSockStream = this.tcpClient.GetStream();
            this.clientStreamReader = new StreamReader(clientSockStream);
            this.clientStreamWriter = new StreamWriter(clientSockStream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCommunicationController"/> class.
        /// </summary>
        /// <param name="address">The ip address.</param>
        public TableCommunicationController(string address)
            : base()
        {
            this.tcpClient = new TcpClient(address, CommunicationController.ServerListenerPortNumber);
            Debug.WriteLine("Client: connected to the server.");
            NetworkStream clientSockStream = this.tcpClient.GetStream();
            this.clientStreamReader = new StreamReader(clientSockStream);
            this.clientStreamWriter = new StreamWriter(clientSockStream);
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
        /// Sends the request game message.
        /// </summary>
        /// <param name="game">The game to join.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendRequestGameMessage(ActiveGameStruct game)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the move action message.
        /// </summary>
        /// <param name="guidObject">The object's Guid.</param>
        /// <param name="pile">The destination pile's Guid.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendMoveActionMessage(string guidObject, string pile)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the custom action message.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="player">The player.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendCustomActionMessage(string action, string player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void Close(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the request game list message.
        /// </summary>
        /// <returns>the gameList</returns>
        public Collection<string> SendRequestGameListMessage()
        {
            Debug.WriteLine("Client: Start of SendRequestGameListMessage");

            // TODO: We should really put a lock around this

            // Send the message to the server
            MessageRequestGameList message = new MessageRequestGameList();
            message.BuildMessage();
            this.clientStreamWriter.WriteLine(message.MessageDocument.InnerXml);
            this.clientStreamWriter.Flush();
            Debug.WriteLine("Client: SendRequestGameListMessage Message Sent waiting for response");

            // Get the response from the server
            string response = this.clientStreamReader.ReadLine();
            Debug.WriteLine("Client: SendRequestGameListMessage response received");
            byte[] responseData = Encoding.ASCII.GetBytes(response);
            MemoryStream ms = new MemoryStream(responseData);
            XmlDocument messageDoc = new XmlDocument();
            messageDoc.Load(ms);

            XmlElement messageResponse = messageDoc.DocumentElement;
            string mt = messageResponse.Attributes[0].Value;

            if (mt == Message.MessageType.GameList.ToString())
            {
                MessageGameList messageGameList = new MessageGameList();
                messageGameList.ProcessMessage(messageDoc);
                Debug.WriteLine("Client: End of SendRequestGameListMessage");
                return messageGameList.GameNameList;
            }
            else
            {
                throw new Exception("Wrong response from server!");
            }
        }

        /// <summary>
        /// Sends the request existing games.
        /// </summary>
        /// <param name="selectedGame">The selected game.</param>
        /// <returns>the collection of existing games.</returns>
        public Collection<ActiveGameStruct> SendRequestExistingGames(string selectedGame)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends the state of the request current game.
        /// </summary>
        /// <param name="gameGuid">The game GUID.</param>
        public void SendRequestCurrentGameState(Guid gameGuid)
        {
        }
    }
}
