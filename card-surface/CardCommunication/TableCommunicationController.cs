﻿// <copyright file="TableCommunicationController.cs" company="University of Louisville Speed School of Engineering">
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
        /// The thread that is responsible for constantly listening to the server
        /// </summary>
        private Thread listenerThread;

        /// <summary>
        /// The semaphore that insures that all function calls are executed sequentically.
        /// </summary>
        private object functionCallSemaphore;

        /// <summary>
        /// The semaphore that synchronizes access to the shared buffers.
        /// </summary>
        private object messageSemaphore;

        /// <summary>
        /// The received message that was sent from the server.
        /// Access to this attributes requires the use of the messageSemaphore.
        /// </summary>
        private string receivedMessage;

        /// <summary>
        /// The received game state that was sent from the server.
        /// Access to this attributes requires the use of the messageSemaphore.
        /// </summary>
        private string receivedGame;

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
            this.functionCallSemaphore = new object();

            // Set up the thread that listens to the server
            this.listenerThread = new Thread(this.ListenToServer);
            this.listenerThread.Name = "Client Listener";
            this.listenerThread.Start();

            // Set up the compoents that will be used by the thread
            this.messageSemaphore = new object();
            this.receivedMessage = string.Empty;
            this.receivedGame = string.Empty;
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
            this.functionCallSemaphore = new object();

            // Set up the thread that listens to the server
            this.listenerThread = new Thread(this.ListenToServer);
            this.listenerThread.Name = "Client Listener";
            this.listenerThread.Start();

            // Set up the compoents that will be used by the thread
            this.messageSemaphore = new object();
            this.receivedMessage = string.Empty;
            this.receivedGame = string.Empty;
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
        /// TODO: REMOVE THIS?
        /// </summary>
        public event UpdateGameListHandler OnUpdateGameList;

        /// <summary>
        /// Event occurs when existing games is updated.
        /// TODO: REMOVE THIS?
        /// </summary>
        public event UpdateExistingGamesHandler OnUpdateExistingGames;

        /// <summary>
        /// Event occurs when Game State is updated.
        /// </summary>
        public event UpdateGameStateHandler OnUpdateGameState;

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
        /// Listens to server.
        /// </summary>
        public void ListenToServer()
        {
            while (true)
            {
                string received = this.clientStreamReader.ReadLine();
                
                // TODO: We should read the message header here and respond accordingly.

                // We should read the header GAME, PUSH, or MESS
                // For now we assume it is a MESS
                lock (this.messageSemaphore)
                {
                    this.receivedMessage = received;
                }
            }
        }

        /// <summary>
        /// Sends the request game list message.
        /// </summary>
        /// <returns>the gameList</returns>
        public Collection<string> SendRequestGameListMessage()
        {
            lock (this.functionCallSemaphore)
            {
                Debug.WriteLine("Client: Start of SendRequestGameListMessage");

                // Send the message to the server
                MessageRequestGameList message = new MessageRequestGameList();
                message.BuildMessage();
                this.clientStreamWriter.WriteLine(message.MessageDocument.InnerXml);
                this.clientStreamWriter.Flush();
                Debug.WriteLine("Client: SendRequestGameListMessage Message Sent waiting for response");

                // Get the response from the server
                string response = this.GetMessageSynchronously();
                
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
        }

        /// <summary>
        /// Sends the request existing games.
        /// </summary>
        /// <param name="selectedGame">The selected game.</param>
        /// <returns>the collection of existing games.</returns>
        public Collection<ActiveGameStruct> SendRequestExistingGames(string selectedGame)
        {
            lock (this.functionCallSemaphore)
            {
                Debug.WriteLine("Client: Start of SendRequestExistingGames");

                // Send the message to the server
                MessageRequestExistingGames message = new MessageRequestExistingGames();
                message.BuildMessage(selectedGame);
                this.clientStreamWriter.WriteLine(message.MessageDocument.InnerXml);
                this.clientStreamWriter.Flush();
                Debug.WriteLine("Client: SendRequestExistingGames Message Sent waiting for response");

                // Get the response from the server
                string response = this.GetMessageSynchronously();
                Debug.WriteLine("Client: SendRequestExistingGames response received");
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                MemoryStream ms = new MemoryStream(responseData);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);

                XmlElement messageResponse = messageDoc.DocumentElement;
                string mt = messageResponse.Attributes[0].Value;

                if (mt == Message.MessageType.ExistingGames.ToString())
                {
                    MessageExistingGames messageExistingGames = new MessageExistingGames();
                    messageExistingGames.ProcessMessage(messageDoc);
                    Debug.WriteLine("Client: End of SendRequestExistingGames");
                    return messageExistingGames.ActiveGames;
                }
                else
                {
                    throw new Exception("Wrong response from server!");
                }
            }
        }

        /// <summary>
        /// Sends the state of the request current game.
        /// This is used to "refresh" the game state and should be removed.
        /// </summary>
        /// <param name="gameGuid">The game GUID.</param>
        public void SendRequestCurrentGameState(Guid gameGuid)
        {
            lock (this.functionCallSemaphore)
            {
                // TODO: REMOVE THIS FUNCTION?
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sends the request game message.
        /// </summary>
        /// <param name="game">The game to join.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendRequestGameMessage(ActiveGameStruct game)
        {
            lock (this.functionCallSemaphore)
            {
                Debug.WriteLine("Client: Start of SendRequestGameMessage");

                // Send the message to the server
                MessageRequestGame message = new MessageRequestGame();
                if (game.Id != Guid.Empty)
                {
                    // The table is requesting an existing game
                    message.BuildMessage(game.Id);
                }
                else
                {
                    // The table is requesting a new game
                    message.BuildMessage(game.GameType);
                }

                // Get the response from the server
                string response = this.clientStreamReader.ReadLine();
                Debug.WriteLine("Client: SendRequestGameMessage response received");

                // TODO: Convert the game here!
                // DO THIS, IT IS VERY IMPORTANT!!!
                // USE THE GetMessageGameSynchronously FUNCTION
                Debug.WriteLine("Client: End of SendRequestGameMessage");
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sends the move action message.
        /// </summary>
        /// <param name="guidObject">The object's Guid.</param>
        /// <param name="pile">The destination pile's Guid.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendMoveActionMessage(string guidObject, string pile)
        {
            lock (this.functionCallSemaphore)
            {
                // TODO: Add the move action, it should return a new game state.
                // USE THE GetMessageGameSynchronously FUNCTION
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Sends the custom action message.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="player">The player.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendCustomActionMessage(string action, string player)
        {
            lock (this.functionCallSemaphore)
            {
                // TODO: Add the custom action, it should return a new game state.
                // USE THE GetMessageGameSynchronously FUNCTION
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the game message synchronously.
        /// This function waits for the listenerThread to receive the response from the server.
        /// </summary>
        /// <returns>The game message from the server.</returns>
        private string GetMessageGameSynchronously()
        {
            // Lets set things up
            string response = string.Empty;
            bool wait = true;

            // We wait until we get the response from the server
            while (wait)
            {
                // Since multiple threads have access to the buffers use protection
                lock (this.messageSemaphore)
                {
                    // Check to see if we have a response
                    if (!this.receivedGame.Equals(string.Empty))
                    {
                        response = this.receivedGame;
                        this.receivedGame = string.Empty;
                        wait = false;
                    }
                }
            }

            // Provide the response
            return response;
        }

        /// <summary>
        /// Gets the message synchronously.
        /// This function waits for the listenerThread to receive the response from the server.
        /// </summary>
        /// <returns>The message from the server.</returns>
        private string GetMessageSynchronously()
        {
            // Lets set things up
            string response = string.Empty;
            bool wait = true;

            // We wait until we get the response from the server
            while (wait)
            {
                // Since multiple threads have access to the buffers use protection
                lock (this.messageSemaphore)
                {
                    // Check to see if we have a response
                    if (!this.receivedMessage.Equals(string.Empty))
                    {
                        response = this.receivedMessage;
                        this.receivedMessage = string.Empty;
                        wait = false;
                    }
                }
            }

            // Provide the response
            return response;
        }
    }
}
