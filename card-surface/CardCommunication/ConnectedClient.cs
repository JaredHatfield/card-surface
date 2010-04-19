// <copyright file="ConnectedClient.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The representation of a connected client.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using CardCommunication.GameObject;
    using CardGame;
    
    /// <summary>
    /// The representation of a connected client.
    /// </summary>
    internal class ConnectedClient
    {
        /// <summary>
        /// The connection that this client will use.
        /// </summary>
        private TcpClient tcpClient;

        /// <summary>
        /// The identifier for this client;
        /// </summary>
        private int id;

        /// <summary>
        /// The guid of the game associated with the client.
        /// </summary>
        private Guid gameGuid;

        /// <summary>
        /// The instance of the game that this table has joined;
        /// </summary>
        private Game game;

        /// <summary>
        /// The stream of data that is read from the server.
        /// </summary>
        private StreamReader serverStreamReader;

        /// <summary>
        /// The stream of data that is written to the server.
        /// </summary>
        private StreamWriter serverStreamWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectedClient"/> class.
        /// </summary>
        /// <param name="tcpClient">The TCP client.</param>
        /// <param name="id">The id of the client.</param>
        internal ConnectedClient(TcpClient tcpClient, int id)
        {
            this.tcpClient = tcpClient;
            this.id = id;
            this.serverStreamWriter = new StreamWriter(tcpClient.GetStream());
            this.serverStreamReader = new StreamReader(tcpClient.GetStream());
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        internal int Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the game's Guid.
        /// </summary>
        /// <value>The game's Guid.</value>
        internal Guid GameGuid
        {
            get { return this.gameGuid; }
        }

        /// <summary>
        /// Gets the next message.
        /// This function will not return until the client sends some data!
        /// </summary>
        /// <returns>A string representation of the data sent from the client.</returns>
        internal string GetNextMessage()
        {
            return this.serverStreamReader.ReadLine();
        }

        /// <summary>
        /// Sends the message to the client.
        /// This automatically flushes the buffer so the data is sent immediately.
        /// </summary>
        /// <param name="message">The message to send..</param>
        internal void SendMessage(string message)
        {
            this.serverStreamWriter.WriteLine(message);
            this.serverStreamWriter.Flush();
        }

        /// <summary>
        /// Joins the game.
        /// </summary>
        /// <param name="game">The game to join.</param>
        internal void JoinGame(Game game)
        {
            this.game = game;
            this.gameGuid = game.Id;
            this.game.GameStateUpdated += new Game.GameStateUpdatedHandler(this.GameStateDidUpdate);
        }

        /// <summary>
        /// The game state updated and we should push a new game to the server.
        /// </summary>
        private void GameStateDidUpdate()
        {
            Debug.WriteLine("Server: Client " + this.id + " pushing updated game state.");
            MemoryStream gameStream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            Game gameObject = new GameMessage(this.game);
            bf.Serialize(gameStream, gameObject);
            this.serverStreamWriter.WriteLine("PUSH" + Convert.ToBase64String(gameStream.ToArray()));
            this.serverStreamWriter.Flush();
        }
    }
}
