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
        /// The server's IP address.
        /// </summary>
        private string ip;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCommunicationController"/> class.
        /// </summary>
        /// <param name="gameController">The game controller.</param>
        public ServerCommunicationController(IGameController gameController)
            : base()
        {
            // Set some things up
            this.gameController = gameController;
            this.connectedClients = new Collection<Thread>();

            // Get the server's ip address and save it
            this.ip = string.Empty;
            IPAddress[] address = Dns.GetHostAddresses(Dns.GetHostName());
            for (int i = 0; i < address.Length; i++)
            {
                if (address[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    this.ip += address[i] + " ";
                }
            }

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
        /// Delegate for a method used to push game states to the client.
        /// </summary>
        /// <param name="game">The game that needs to be pushed.</param>
        internal delegate void GameStateNeedsPushedEventHandler(Guid game);

        /// <summary>
        /// Occurs when [game state needs pushed].
        /// </summary>
        internal event GameStateNeedsPushedEventHandler GameStateNeedsPushed;

        /// <summary>
        /// Gets the IP address.
        /// </summary>
        /// <value>The IP address.</value>
        public string IP
        {
            get { return this.ip; }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void Close(object sender, EventArgs args)
        {
            // Close down the server listener thread
            this.serverListenerLoopThread.Abort();

            // Close down all of the client threads
            for (int i = 0; i < this.connectedClients.Count; i++)
            {
                this.connectedClients[i].Abort();
            }
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
            this.GameStateNeedsPushed += new GameStateNeedsPushedEventHandler(cc.GameStateDidUpdate);

            // TODO: this is where all of the magic happens.
            while (true)
            {
                string input = String.Empty;
                Debug.WriteLine("Server: Client " + cc.Id + " is waiting for a message");

                try
                {
                    input = cc.GetNextMessage();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Communication Link Disconnected. Close the thread." + e.ToString());

                    // TODO: properly shutdown the game if no other clients are connected.
                    break;
                }

                Debug.WriteLine("Server: Client " + cc.Id + " has a message to process");
                byte[] byteArray = Encoding.ASCII.GetBytes(input);
                MemoryStream ms = new MemoryStream(byteArray);
                XmlDocument messageDoc = new XmlDocument();
                messageDoc.Load(ms);
                
                XmlElement messageElement = messageDoc.DocumentElement;
                XmlElement bodyElement = (XmlElement)messageElement.LastChild;
                string mt = bodyElement.Attributes[0].Value;

                Collection<ParameterStruct> parameters = new Collection<ParameterStruct>();

                if (mt == Message.MessageType.RequestGameList.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestGameList Message");
                    Message messageRequestGameList = new Message();
                    messageRequestGameList.ProcessMessage(messageDoc);
                    Message messageGameList = new Message();
                    
                    int index = 0;
                    //// this may need a counter to pass as attributes.
                    foreach (string s in this.gameController.GameTypes)
                    {
                        AddParameter(parameters, "GameType" + index, s);
                        index++;
                    }

                    messageGameList.BuildMessage(Message.MessageType.GameList.ToString(), parameters);
                    Debug.WriteLine("Server: Client " + cc.Id + " returned the list of game types");
                    cc.SendMessage(HeaderMessage + messageGameList.MessageDocument.InnerXml);
                }
                else if (mt == Message.MessageType.RequestGame.ToString())
                {
                    string gameType = string.Empty;
                    Guid gameGuid = Guid.Empty;

                    // This actually indicates that the table joined the game!
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestGameListMessage");
                    Message messageRequestGame = new Message();
                    messageRequestGame.ProcessMessage(messageDoc);

                    foreach (ParameterStruct ps in messageRequestGame.Parameters)
                    {
                        switch (ps.Name)
                        {
                            case "gameType":
                                gameType = ps.Value;
                                break;
                            case "gameGuid":
                                gameGuid = new Guid(ps.Value);
                                break;
                        }
                    }

                    if (gameType != null)
                    {
                        Guid newGame = this.gameController.CreateGame(gameType);
                        cc.JoinGame(this.gameController.GetGame(newGame));
                        Debug.WriteLine("Server: Client " + cc.Id + " returned the game state.");

                        // This needs to send the game
                        cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
                    }
                    else
                    {
                        cc.JoinGame(this.gameController.GetGame(gameGuid));
                        Debug.WriteLine("Server: Client " + cc.Id + " returned the game state.");

                        // This needs to send the game
                        cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
                    }
                }
                else if (mt == Message.MessageType.Action.ToString())
                {
                    Message messageAction = new Message();
                    bool success = false;
                    string actionType = string.Empty;

                    messageAction.ProcessMessage(messageDoc);

                    foreach (ParameterStruct ps in messageAction.Parameters)
                    {
                        switch (ps.Name)
                        {
                            case "actionType":
                                actionType = ps.Value;
                                break;
                        }
                    }

                    if (actionType == "Move")
                    {
                        Guid physicalObject = Guid.Empty;
                        Guid destinationPile = Guid.Empty;

                        foreach (ParameterStruct ps in messageAction.Parameters)
                        {
                            switch (ps.Name)
                            {
                                case "param1":
                                    physicalObject = new Guid(ps.Value);
                                    break;
                                case "param2":
                                    destinationPile = new Guid(ps.Value);
                                    break;
                            }
                        }

                        // Executes Move
                        try
                        {
                            success = this.gameController.GetGame(cc.GameGuid).MoveAction(physicalObject, destinationPile);
                        }
                        catch (CardGameMoveToNonOpenPileException)
                        {
                            // We don't want to crash if the destination pile is not open
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                            throw new MessageProcessException("Error executing move action.", e);
                        }
                        finally
                        {
                            this.gameController.SetActionStatus(cc.GameGuid, success);
                        }

                        // This needs to send the game
                        cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));

                        // Push the updates to all of the games
                        this.GameStateNeedsPushed(cc.GameGuid);
                    }
                    else if (actionType == "Custom")
                    {
                        string actionName = string.Empty;
                        string playerName = string.Empty;

                        foreach (ParameterStruct ps in messageAction.Parameters)
                        {
                            switch (ps.Name)
                            {
                                case "param1":
                                    actionName = ps.Value;
                                    break;
                                case "param2":
                                    playerName = ps.Value;
                                    break;
                            }
                        }

                        // Executes Custum Action
                        try
                        {
                            this.gameController.GetGame(cc.GameGuid).ExecuteAction(actionName, playerName);
                        }
                        catch (CardGameActionNotFoundException e)
                        {
                            Debug.WriteLine("The action the user requested could not be found: " + e.ToString());
                        }
                        catch (CardGameActionAccessDeniedException e)
                        {
                            Debug.WriteLine("The the user was not allowed to use the requested exception: " + e.ToString());
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("We tried to execute an action... " + e.ToString());

                            // Something else very bad happened so we are going to throw another exception.
                            throw new MessageProcessException("Something else went wrong with the game.", e);
                        }
                        finally
                        {
                            this.gameController.SetActionStatus(cc.GameGuid, success);
                        }
                        
                        // This needs to send the game
                        cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));

                        // Push the updates to all of the games
                        this.GameStateNeedsPushed(cc.GameGuid);
                    }
                }
                else if (mt == Message.MessageType.FlipCard.ToString())
                {
                    Message messageFlipCard = new Message();
                    messageFlipCard.ProcessMessage(messageDoc);
                    Guid cardGuid = Guid.Empty;

                    foreach (ParameterStruct ps in messageFlipCard.Parameters)
                    {
                        switch (ps.Name)
                        {
                            case "cardGuid":
                                cardGuid = new Guid(ps.Value);
                                break;
                        }
                    }

                    this.gameController.GetGame(cc.GameGuid).FlipCard(cardGuid);
                    
                    // This needs to send the game
                    cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
                }
                else if (mt == Message.MessageType.RequestExistingGames.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a ExistingGames");
                    Message messageRequestExistingGames = new Message();
                    messageRequestExistingGames.ProcessMessage(messageDoc);
                    Message messageExistingGames = new Message();
                    Collection<string> existingGames = new Collection<string>();
                    string newGame = string.Empty;
                    string selectedGame = string.Empty;

                    foreach (ParameterStruct ps in messageRequestExistingGames.Parameters)
                    {
                        switch (ps.Name)
                        {
                            case "selectedGame":
                                selectedGame = ps.Value;
                                break;
                        }
                    }

                    newGame = selectedGame + ";New Game;" + Guid.Empty.ToString() + ";";

                    existingGames.Add(newGame);
                    
                    foreach (Game game in this.gameController.Games)
                    {
                        Collection<string> gameObject = new Collection<string>();
                        string gameObject1 = string.Empty;
                        if (game.Name == selectedGame)
                        {
                            // Element 1 - Type, 2 - Display, 3 - ID, 4 - #Players
                            gameObject1 = game.Name + ";" + game.Name + ";" + game.Id.ToString() + ";" + game.NumberOfPlayers + "/" + game.NumberOfSeats;
                        }

                        existingGames.Add(gameObject1);
                    }

                    int index = 0;
                    foreach (string s in existingGames)
                    {
                        AddParameter(parameters, "Game" + index, existingGames[index]);
                        index++;
                    }

                    messageExistingGames.BuildMessage(Message.MessageType.ExistingGames.ToString(), parameters);                   
                    Debug.WriteLine("Server: Client " + cc.Id + " returned the list of exiting games");
                    cc.SendMessage(HeaderMessage + messageExistingGames.MessageDocument.InnerXml);
                }                
                else if (mt == Message.MessageType.RequestCurrentGameState.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestCurrentGameState");
                    Message messageRequestCurrentGameState = new Message();
                    messageRequestCurrentGameState.ProcessMessage(messageDoc);

                    Debug.WriteLine("Server: Client " + cc.Id + " returned the game state");

                    // This needs to send the game
                    cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
                }
                else if (mt == Message.MessageType.RequestSeatCodeChange.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestSeatCodeChange");
                    Message mrscc = new Message();
                    mrscc.ProcessMessage(messageDoc);
                    Guid seatGuid = Guid.Empty;

                    foreach (ParameterStruct ps in mrscc.Parameters)
                    {
                        switch (ps.Name)
                        {
                            case "seatGuid":
                                seatGuid = new Guid(ps.Value);
                                break;
                        }
                    }

                    bool success = this.gameController.GetGame(cc.GameGuid).RegenerateSeatCode(seatGuid);

                    if (!success)
                    {
                        Debug.WriteLine("SeatCode failed to change.");
                    }

                    Debug.WriteLine("Server: Client " + cc.Id + " returned the game state");

                    // This needs to send the game
                    cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
                }
            }

            this.GameStateNeedsPushed -= cc.GameStateDidUpdate;
        }

        /// <summary>
        /// Serializes the game to message.
        /// </summary>
        /// <param name="game">The game to serialize.</param>
        /// <returns>the string representation of the serialized game.</returns>
        private string SerializeGameToMessage(Game game)
        {
            Message messageGameState = new Message();
            MemoryStream gameStream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            Game gameObject = new GameMessage(game);
            Collection<ParameterStruct> parameters = new Collection<ParameterStruct>();
                        
            bf.Serialize(gameStream, gameObject);
            string str = Convert.ToBase64String(gameStream.ToArray());

            ParameterStruct ps = new ParameterStruct();
            ps.Name = "gameState";
            ps.Value = str;

            parameters.Add(ps);

            messageGameState.BuildMessage(Message.MessageType.GameState.ToString(), parameters);

            return messageGameState.MessageDocument.InnerXml;
        }
    }
}