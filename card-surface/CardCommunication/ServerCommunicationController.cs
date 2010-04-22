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
            this.serverListenerLoopThread.Abort();
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

                XmlElement message = messageDoc.DocumentElement;
                string mt = message.Attributes[0].Value;

                if (mt == Message.MessageType.RequestGameList.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestGameListMessage");
                    MessageRequestGameList messageRequestGameList = new MessageRequestGameList();
                    messageRequestGameList.ProcessMessage(messageDoc);
                    MessageGameList messageGameList = new MessageGameList();
                    messageGameList.BuildMessage(this.gameController.GameTypes);
                    Debug.WriteLine("Server: Client " + cc.Id + " returned the list of game types");
                    cc.SendMessage(HeaderMessage + messageGameList.MessageDocument.InnerXml);
                }
                else if (mt == Message.MessageType.RequestGame.ToString())
                {
                    // This actually indicates that the table joined the game!
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestGameListMessage");
                    MessageRequestGame messageRequestGame = new MessageRequestGame();
                    messageRequestGame.ProcessMessage(messageDoc);

                    if (messageRequestGame.GameType != null)
                    {
                        string gameType = messageRequestGame.GameType;
                        Guid newGame = this.gameController.CreateGame(gameType);
                        cc.JoinGame(this.gameController.GetGame(newGame));
                        Debug.WriteLine("Server: Client " + cc.Id + " returned the game state.");

                        // This needs to send the game
                        cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
                    }
                    else
                    {
                        Guid selectedGameGuid = messageRequestGame.GameGuid;
                        cc.JoinGame(this.gameController.GetGame(selectedGameGuid));
                        Debug.WriteLine("Server: Client " + cc.Id + " returned the game state.");

                        // This needs to send the game
                        cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
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
                            this.gameController.GetGame(cc.GameGuid).MoveAction(physicalObject, destinationPile);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e);
                            throw new MessageProcessException("Error executing move action.", e);
                        }

                        // This needs to send the game
                        cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));

                        // Push the updates to all of the games
                        this.GameStateNeedsPushed(cc.GameGuid);
                    }
                    else if (action[0] == MessageAction.ActionType.Custom.ToString())
                    {
                        string actionName = action[1];
                        string playerName = action[2];

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
                        
                        // This needs to send the game
                        cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));

                        // Push the updates to all of the games
                        this.GameStateNeedsPushed(cc.GameGuid);
                    }
                }
                else if (mt == Message.MessageType.FlipCard.ToString())
                {
                    MessageFlipCard messageFlipCard = new MessageFlipCard();
                    messageFlipCard.ProcessMessage(messageDoc);
                    Guid cardGuid = messageFlipCard.CardGuid;
                    this.gameController.GetGame(cc.GameGuid).FlipCard(cardGuid);
                                       
                    // This needs to send the game
                    cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
                }
                else if (mt == Message.MessageType.RequestExistingGames.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a ExistingGames");
                    MessageRequestExistingGames messageRequestExistingGames = new MessageRequestExistingGames();
                    messageRequestExistingGames.ProcessMessage(messageDoc);
                    MessageExistingGames messageExistingGames = new MessageExistingGames();
                    Collection<Collection<string>> existingGames = new Collection<Collection<string>>();
                    Collection<string> newGame = new Collection<string>();

                    newGame.Add(messageRequestExistingGames.SelectedGame);
                    newGame.Add("New Game");
                    newGame.Add(Guid.Empty.ToString());
                    newGame.Add(String.Empty);

                    existingGames.Add(newGame);
                    
                    foreach (Game game in this.gameController.Games)
                    {
                        Collection<string> gameObject = new Collection<string>();

                        if (game.Name == messageRequestExistingGames.SelectedGame)
                        {
                            // Element 1 - Type, 2 - Display, 3 - ID, 4 - #Players
                            gameObject.Add(game.Name);
                            gameObject.Add(game.Name);
                            gameObject.Add(game.Id.ToString());
                            gameObject.Add(game.NumberOfPlayers + "/" + game.NumberOfSeats);
                            //// gameObject.Add(game.location);
                        }

                        existingGames.Add(gameObject);
                    }

                    messageExistingGames.BuildMessage(existingGames);
                    
                    Debug.WriteLine("Server: Client " + cc.Id + " returned the list of exiting games");
                    cc.SendMessage(HeaderMessage + messageExistingGames.MessageDocument.InnerXml);
                }                
                else if (mt == Message.MessageType.RequestCurrentGameState.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestCurrentGameState");
                    MessageRequestCurrentGameState messageRequestCurrentGameState = new MessageRequestCurrentGameState();
                    messageRequestCurrentGameState.ProcessMessage(messageDoc);

                    Debug.WriteLine("Server: Client " + cc.Id + " returned the game state");

                    // This needs to send the game
                    cc.SendMessage(HeaderGame + this.SerializeGameToMessage(this.gameController.GetGame(cc.GameGuid)));
                }
                else if (mt == Message.MessageType.RequestSeatCodeChange.ToString())
                {
                    Debug.WriteLine("Server: Client " + cc.Id + " has a RequestSeatCodeChange");
                    MessageRequestSeatCodeChange mrscc = new MessageRequestSeatCodeChange();
                    mrscc.ProcessMessage(messageDoc);

                    bool success = this.gameController.GetGame(cc.GameGuid).RegenerateSeatCode(mrscc.SeatGuid);

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
            MessageGameState messageGameState = new MessageGameState();
            MemoryStream gameStream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            Game gameObject = new GameMessage(game);
                        
            bf.Serialize(gameStream, gameObject);
            string str = Convert.ToBase64String(gameStream.ToArray());

            messageGameState.BuildMessage(str);

            return messageGameState.MessageDocument.InnerXml;
        }
    }
}