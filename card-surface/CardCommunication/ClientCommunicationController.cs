﻿// <copyright file="ClientCommunicationController.cs" company="University of Louisville Speed School of Engineering">
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
    public class ClientCommunicationController : CommunicationController
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
        /// Initializes a new instance of the <see cref="ClientCommunicationController"/> class.
        /// </summary>
        public ClientCommunicationController()
            : base()
        {
            try
            {
                this.tcpClient = new TcpClient("localhost", CommunicationController.ServerListenerPortNumber);
            }
            catch (SocketException se)
            {
                Debug.WriteLine("TableCommunicationController: Could not connect to the specified server (" + se.Message + ")");
                throw new SocketBindingException("Could not connect to the specified server", se);
            }

            Debug.WriteLine("Client: connected to the server.");
            NetworkStream clientSockStream = this.tcpClient.GetStream();
            this.clientStreamReader = new StreamReader(clientSockStream);
            this.clientStreamWriter = new StreamWriter(clientSockStream);
            this.functionCallSemaphore = new object();

            // Set up the thread that listens to the server
            this.listenerThread = new Thread(this.ListenToServer);
            this.listenerThread.SetApartmentState(ApartmentState.STA);
            this.listenerThread.Name = "Client Listener";
            this.listenerThread.Start();

            // Set up the compoents that will be used by the thread
            this.messageSemaphore = new object();
            this.receivedMessage = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCommunicationController"/> class.
        /// </summary>
        /// <param name="address">The ip address.</param>
        public ClientCommunicationController(string address)
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
            this.listenerThread.SetApartmentState(ApartmentState.STA);
            this.listenerThread.Name = "Client Listener";
            this.listenerThread.Start();

            // Set up the compoents that will be used by the thread
            this.messageSemaphore = new object();
            this.receivedMessage = string.Empty;
        }

        /// <summary>
        /// Delegate for updating the game state.
        /// </summary>
        /// <param name="game">The game update.</param>
        public delegate void UpdateGameStateHandler(Game game);

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
                string received;

                try
                {
                    Debug.WriteLine("Client: waiting for message from server");
                    received = this.clientStreamReader.ReadLine();
                    Debug.WriteLine("Client: received a message from the server, processing...");
                }
                catch (IOException e)
                {
                    Debug.WriteLine("Server Closed. Close the thread." + e.ToString());
                    
                    // TODO: shutdown the client or reset to oringinal state.
                    break;
                }

                // Determine what to do with the data received
                if (received.StartsWith(HeaderMessage) || received.StartsWith(HeaderGame))
                {
                    // It was an XML message
                    Debug.WriteLine("Client received an XML message");
                    received = received.Substring(HeaderMessage.Length, received.Length - HeaderMessage.Length);
                    lock (this.messageSemaphore)
                    {
                        this.receivedMessage = received;
                    }
                }
                else if (received.StartsWith(HeaderPush))
                {
                    // It was an unexpected message
                    Debug.WriteLine("Client received an unexpected game state");
                    received = received.Substring(HeaderPush.Length, received.Length - HeaderPush.Length);
                    if (this.OnUpdateGameState != null)
                    {
                        GameMessage gameMessage = null;
                        try
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            MemoryStream memstream = new MemoryStream(Convert.FromBase64String(received));
                            gameMessage = (GameMessage)bf.Deserialize(memstream);
                        }
                        catch (SerializationException e)
                        {
                            Debug.WriteLine("Error deserializing game" + e.ToString());
                            throw new MessageDeserializationException("Error deserializing game", e);
                        }

                        // Now that we have the game, we can update it!
                        if (gameMessage != null)
                        {
                            Debug.WriteLine("Client: About to process a pushed game update.");
                            this.OnUpdateGameState(gameMessage);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Message ignored: " + received);
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
                Message messageRequestGameList = new Message();
                messageRequestGameList.BuildMessage(Message.MessageType.RequestGameList.ToString(), null);
                this.clientStreamWriter.WriteLine(messageRequestGameList.MessageDocument.InnerXml);
                this.clientStreamWriter.Flush();
                Debug.WriteLine("Client: SendRequestGameListMessage Message Sent waiting for response");

                // Get the response from the server
                string response = this.GetMessageSynchronously();
                
                Debug.WriteLine("Client: SendRequestGameListMessage response received");
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                MemoryStream ms = new MemoryStream(responseData);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);

                XmlElement messageElement = messageDoc.DocumentElement;
                XmlElement bodyElement = (XmlElement)messageElement.LastChild;
                string mt = bodyElement.Attributes[0].Value;

                if (mt == Message.MessageType.GameList.ToString())
                {
                    Message messageGameList = new Message();
                    Collection<ParameterStruct> parameters = new Collection<ParameterStruct>();
                    Collection<string> gameNameList = new Collection<string>();
                    messageGameList.ProcessMessage(messageDoc);
                    Debug.WriteLine("Client: End of SendRequestGameListMessage");
                    parameters = messageGameList.Parameters;

                    foreach (ParameterStruct p in parameters)
                    {
                        gameNameList.Add(p.Value);
                    }

                    return gameNameList;
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
                Message messageRequestExistingGames = new Message();
                Collection<ParameterStruct> parameters = new Collection<ParameterStruct>();
                ParameterStruct newPs = new ParameterStruct();
                newPs.Name = "selectedGame";
                newPs.Value = selectedGame;
                parameters.Add(newPs);

                messageRequestExistingGames.BuildMessage(Message.MessageType.RequestExistingGames.ToString(), parameters);
                this.clientStreamWriter.WriteLine(messageRequestExistingGames.MessageDocument.InnerXml);
                this.clientStreamWriter.Flush();
                Debug.WriteLine("Client: SendRequestExistingGames Message Sent waiting for response");

                // Get the response from the server
                string response = this.GetMessageSynchronously();
                Debug.WriteLine("Client: SendRequestExistingGames response received");
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                MemoryStream ms = new MemoryStream(responseData);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);

                XmlElement messageElement = messageDoc.DocumentElement;
                XmlElement bodyElement = (XmlElement)messageElement.LastChild;
                string mt = bodyElement.Attributes[0].Value;

                if (mt == Message.MessageType.ExistingGames.ToString())
                {
                    Message messageExistingGames = new Message();
                    Collection<ActiveGameStruct> activeGames = new Collection<ActiveGameStruct>();
                    messageExistingGames.ProcessMessage(messageDoc);

                    foreach (ParameterStruct ps in messageExistingGames.Parameters)
                    {
                        ActiveGameStruct ags = new ActiveGameStruct();
                        char[] separater = new char[1];
                        separater[0] = ';';
                        string[] segments = ps.Value.Split(separater);
                        ags.GameType = segments[0];
                        ags.DisplayString = segments[1];
                        ags.Id = new Guid(segments[2]);
                        ags.Players = segments[3];
                        activeGames.Add(ags);
                    }

                    Debug.WriteLine("Client: End of SendRequestExistingGames");
                    return activeGames;
                }
                else
                {
                    throw new Exception("Wrong response from server!");
                }
            }
        }

        /// <summary>
        /// Sends the request game message.
        /// </summary>
        /// <param name="game">The game to join.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendRequestGameMessage(ActiveGameStruct game)
        {
            GameMessage gameObject;

            lock (this.functionCallSemaphore)
            {
                Debug.WriteLine("Client: Start of SendRequestGameMessage");

                // Send the message to the server
                Message messageRequestGame = new Message();
                Collection<ParameterStruct> parameters = new Collection<ParameterStruct>();

                ParameterStruct newPs = new ParameterStruct();
                newPs.Name = "gameType";
                newPs.Value = game.GameType;
                parameters.Add(newPs);

                newPs = new ParameterStruct();
                newPs.Name = "gameGuid";
                newPs.Value = game.Id.ToString();
                parameters.Add(newPs);

                messageRequestGame.BuildMessage(Message.MessageType.RequestGame.ToString(), parameters);

                this.clientStreamWriter.WriteLine(messageRequestGame.MessageDocument.InnerXml);
                this.clientStreamWriter.Flush();
                Debug.WriteLine("Client: SendRequestGame Message Sent waiting for response");

                // Get the response from the server
                string response = this.GetMessageSynchronously();
                Debug.WriteLine("Client: SendRequestGame response received");
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                MemoryStream ms = new MemoryStream(responseData);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);

                XmlElement messageElement = messageDoc.DocumentElement;
                XmlElement bodyElement = (XmlElement)messageElement.LastChild;
                string mt = bodyElement.Attributes[0].Value;

                if (mt == Message.MessageType.GameState.ToString())
                {
                    Message messageGameState = new Message();
                    messageGameState.ProcessMessage(messageDoc);
                    Debug.WriteLine("Client: End of SendRequestGame Message");
                    string serializedGame = string.Empty;

                    foreach (ParameterStruct ps in messageGameState.Parameters)
                    {
                        switch (ps.Name)
                        {
                            case "gameState":
                                serializedGame = ps.Value;
                                break;
                        }
                    }

                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream memstream = new MemoryStream(Convert.FromBase64String(serializedGame));
                        gameObject = (GameMessage)bf.Deserialize(memstream);
                    }
                    catch (SerializationException e)
                    {
                        Debug.WriteLine("Error deserializing game" + e.ToString());
                        throw new MessageDeserializationException("Error deserializing game", e);
                    }
                }
                else
                {
                    throw new MessageProcessException("Wrong response from server!");
                }
            }

            return gameObject;
        }

        /// <summary>
        /// Sends the request seat code change message.
        /// </summary>
        /// <param name="seatGuid">The seat GUID.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendRequestSeatCodeChangeMessage(Guid seatGuid)
        {
            GameMessage gameObject;

            lock (this.functionCallSemaphore)
            {
                Debug.WriteLine("Client: Start of SendRequestSeatCodeChangeMessage");

                // Send the message to the server
                Message messageRequestSeatCodeChange = new Message();
                Collection<ParameterStruct> parameters = new Collection<ParameterStruct>();

                ParameterStruct newPs = new ParameterStruct();
                newPs.Name = "seatGuid";
                newPs.Value = seatGuid.ToString();

                messageRequestSeatCodeChange.BuildMessage(Message.MessageType.RequestSeatCodeChange.ToString(), parameters);

                this.clientStreamWriter.WriteLine(messageRequestSeatCodeChange.MessageDocument.InnerXml);
                this.clientStreamWriter.Flush();
                Debug.WriteLine("Client: SendRequestSeatCodeChange Message Sent waiting for response");

                // Get the response from the server
                string response = this.GetMessageSynchronously();
                Debug.WriteLine("Client: SendRequestSeatCodeChange response received");
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                MemoryStream ms = new MemoryStream(responseData);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);

                XmlElement messageElement = messageDoc.DocumentElement;
                XmlElement bodyElement = (XmlElement)messageElement.LastChild;
                string mt = bodyElement.Attributes[0].Value;

                if (mt == Message.MessageType.GameState.ToString())
                {
                    Message messageGameState = new Message();
                    messageGameState.ProcessMessage(messageDoc);
                    Debug.WriteLine("Client: End of SendRequestSeatCodeChange Message");
                    string serializedGame = string.Empty;

                    foreach (ParameterStruct ps in messageGameState.Parameters)
                    {
                        switch (ps.Name)
                        {
                            case "gameState":
                                serializedGame = ps.Value;
                                break;
                        }
                    }

                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream memstream = new MemoryStream(Convert.FromBase64String(serializedGame));
                        gameObject = (GameMessage)bf.Deserialize(memstream);
                    }
                    catch (SerializationException e)
                    {
                        Debug.WriteLine("Error deserializing game" + e.ToString());
                        throw new MessageDeserializationException("Error deserializing game", e);
                    }
                }
                else
                {
                    throw new MessageProcessException("Wrong response from server!");
                }
            }

            return gameObject;
        }

        /// <summary>
        /// Sends the move action message.
        /// </summary>
        /// <param name="guidObject">The object's Guid.</param>
        /// <param name="pile">The destination pile's Guid.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendMoveActionMessage(string guidObject, string pile)
        {
            GameMessage gameObject;
            lock (this.functionCallSemaphore)
            {
                Debug.WriteLine("Client: Start of SendMoveActionMessage");

                // Send the message to the server
                Message messageAction = new Message();
                string action = guidObject + ";" + pile;

                Collection<ParameterStruct> parameters = new Collection<ParameterStruct>();
                ParameterStruct parameter = new ParameterStruct();
                parameter.Name = "actionType";
                parameter.Value = "Move";
                parameters.Add(parameter);

                parameter = new ParameterStruct();
                parameter.Name = "param1";
                parameter.Value = guidObject;
                parameters.Add(parameter);

                parameter = new ParameterStruct();
                parameter.Name = "param2";
                parameter.Value = pile;
                parameters.Add(parameter);

                messageAction.BuildMessage(Message.MessageType.Action.ToString(), parameters);
                this.clientStreamWriter.WriteLine(messageAction.MessageDocument.InnerXml);
                this.clientStreamWriter.Flush();
                Debug.WriteLine("Client: SendMoveActionMessage Message Sent waiting for response");

                // Get the response from the server
                string response = this.GetMessageSynchronously();

                Debug.WriteLine("Client: SendMoveActionMessage response received");
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                MemoryStream ms = new MemoryStream(responseData);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);

                XmlElement messageElement = messageDoc.DocumentElement;
                XmlElement bodyElement = (XmlElement)messageElement.LastChild;
                string mt = bodyElement.Attributes[0].Value;

                if (mt == Message.MessageType.GameState.ToString())
                {
                    MessageGameState messageGameState = new MessageGameState();
                    messageGameState.ProcessMessage(messageDoc);
                    Debug.WriteLine("Client: End of SendMoveActionMessage");

                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream memstream = new MemoryStream(Convert.FromBase64String(messageGameState.SerializedGame));
                        gameObject = (GameMessage)bf.Deserialize(memstream);
                    }
                    catch (SerializationException e)
                    {
                        Debug.WriteLine("Error deserializing game" + e.ToString());
                        throw new MessageDeserializationException("Error deserializing game", e);
                    }

                    //// TODO: Update the game; however that is done.
                }
                else
                {
                    throw new MessageProcessException("Wrong response from server!");
                }
            }

            return gameObject;
        }

        /// <summary>
        /// Sends the custom action message.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="player">The player.</param>
        /// <returns>The new game state.</returns>
        public GameMessage SendCustomActionMessage(string action, string player)
        {
            GameMessage gameObject;
            lock (this.functionCallSemaphore)
            {
                Debug.WriteLine("Client: Start of SendCustomActionMessage");

                // Send the message to the server
                Message messageAction = new Message();
                Collection<ParameterStruct> parameters = new Collection<ParameterStruct>();
                ParameterStruct parameter = new ParameterStruct();
                parameter.Name = "actionType";
                parameter.Value = "Custom";
                parameters.Add(parameter);

                parameter = new ParameterStruct();
                parameter.Name = "param1";
                parameter.Value = action;
                parameters.Add(parameter);

                parameter = new ParameterStruct();
                parameter.Name = "param2";
                parameter.Value = player;
                parameters.Add(parameter);

                messageAction.BuildMessage(Message.MessageType.Action.ToString(), parameters);

                this.clientStreamWriter.WriteLine(messageAction.MessageDocument.InnerXml);
                this.clientStreamWriter.Flush();
                Debug.WriteLine("Client: SendCustomActionMessage Message Sent waiting for response");

                // Get the response from the server
                string response = this.GetMessageSynchronously();

                Debug.WriteLine("Client: SendMoveCustomMessage response received");
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                MemoryStream ms = new MemoryStream(responseData);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);

                XmlElement messageElement = messageDoc.DocumentElement;
                XmlElement bodyElement = (XmlElement)messageElement.LastChild;
                string mt = bodyElement.Attributes[0].Value;

                if (mt == Message.MessageType.GameState.ToString())
                {
                    MessageGameState messageGameState = new MessageGameState();
                    messageGameState.ProcessMessage(messageDoc);
                    Debug.WriteLine("Client: End of SendCustomActionMessage");

                    try
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream memstream = new MemoryStream(Convert.FromBase64String(messageGameState.SerializedGame));
                        gameObject = (GameMessage)bf.Deserialize(memstream);
                    }
                    catch (SerializationException e)
                    {
                        Debug.WriteLine("Error deserializing game" + e.ToString());
                        throw new MessageDeserializationException("Error deserializing game", e);
                    }

                    //// TODO: Update the game; however that is done.
                }
                else
                {
                    throw new MessageProcessException("Wrong response from server!");
                }
            }

            return gameObject;
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
