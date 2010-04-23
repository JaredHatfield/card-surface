// <copyright file="MessageExistingGames.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An message that is sent to the table communication controller
// to send a list of playable games.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using CardGame;
    using CommunicationException;

    /// <summary>
    /// A message to send the list of existing/active games.
    /// </summary>
    public class MessageExistingGames : Message
    {
        /// <summary>
        /// Collection of Active Game Structures for use by the command line.
        /// </summary>
        private Collection<ActiveGameStruct> activeGames = new Collection<ActiveGameStruct>();

        /// <summary>
        /// Collection of string composites of games for use by the table.
        /// </summary>
        private Collection<string> gameNames = new Collection<string>();

        /// <summary>
        /// Collection of existing games that can be played.
        /// </summary>
        private Collection<Collection<string>> existingGameList;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageExistingGames"/> class.
        /// </summary>
        public MessageExistingGames()
        {
            MessageTypeName = MessageType.ExistingGames.ToString();
        }

        /// <summary>
        /// Gets the game names for use by the table.
        /// </summary>
        /// <value>The game names.</value>
        public Collection<string> GameNames
        {
            get { return this.gameNames; }
        }

        /// <summary>
        /// Gets the active games.
        /// </summary>
        /// <value>The active games.</value>
        public Collection<ActiveGameStruct> ActiveGames
        {
            get { return this.activeGames; }
        }

        /// <summary>
        /// Gets the game name list.
        /// </summary>
        /// <value>The game name list.</value>
        public Collection<Collection<string>> ExistingGameList
        {
            get { return this.existingGameList; }
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="existingGameList">The game name list.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(Collection<Collection<string>> existingGameList)
        {
            XmlElement message = this.MessageDocument.CreateElement("Message");
            bool success = true;

            try
            {
                this.existingGameList = existingGameList;

                message.SetAttribute("MessageType", this.MessageTypeName);
                this.BuildHeader(ref message);
                this.BuildBody(ref message);

                this.MessageDocument.InnerXml = message.OuterXml;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Building Message: " + e.InnerException);
                success = false;
                throw new MessageTransportException("Error Building Existing Games Message.", e);
            }

            return success;
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageDoc">The message document.</param>
        public override void ProcessMessage(XmlDocument messageDoc)
        {
            XmlElement message = messageDoc.DocumentElement;

            foreach (XmlNode node in message.ChildNodes)
            {
                XmlElement element = (XmlElement)node;

                switch (node.Name)
                {
                    case "Header":
                        this.ProcessHeader(element);
                        break;
                    case "Body":
                        this.ProcessBody(element);
                        break;
                }
            }
        }

        /// <summary>
        /// Builds the body.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void BuildBody(ref XmlElement message)
        {
            XmlElement body = this.MessageDocument.CreateElement("Body");

            this.BuildGameList(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="body">The body to be processed.</param>
        protected override void ProcessBody(XmlElement body)
        {
            foreach (XmlNode node in body.ChildNodes)
            {
                XmlElement childElement = (XmlElement)node;
                childElement.InnerXml = node.InnerXml;

                switch (childElement.Name)
                {
                    case "ExistingGames":
                        this.ProcessGameList(childElement);
                        break;
                }
            }
        }

        /// <summary>
        /// Builds the game list.
        /// </summary>
        /// <param name="message">The message to be built.</param>
        protected void BuildGameList(ref XmlElement message)
        {
            XmlElement gameList = this.MessageDocument.CreateElement("ExistingGames");
            string name = String.Empty;

            foreach (Collection<string> game in this.existingGameList)
            {
                this.BuildGame(ref gameList, game);
            }

            message.AppendChild(gameList);
        }

        /// <summary>
        /// Processes the game list.
        /// </summary>
        /// <param name="gameList">The game list.</param>
        protected void ProcessGameList(XmlElement gameList)
        {
            foreach (XmlNode node in gameList.ChildNodes)
            {
                ActiveGameStruct game = new ActiveGameStruct();
                XmlElement gameListElement = (XmlElement)node;
                string gameString = String.Empty;

                gameListElement.InnerXml = node.InnerXml;

                switch (gameListElement.Name)
                {
                    case "Game":
                        this.ProcessGame(gameListElement, ref game, ref gameString);
                        break;
                }

                this.gameNames.Add(gameString);
                this.activeGames.Add(game);
            }
        }

        /// <summary>
        /// Builds the game.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="game">The game to be built.</param>
        protected void BuildGame(ref XmlElement message, Collection<string> game)
        {
            XmlElement gameElement = this.MessageDocument.CreateElement("Game");

            gameElement.SetAttribute("type", game[0]);
            gameElement.SetAttribute("display", game[1]);
            gameElement.SetAttribute("id", game[2]);
            gameElement.SetAttribute("players", game[3]);

            message.AppendChild(gameElement);
        }

        /// <summary>
        /// Processes the game.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="game">The game for the command line.</param>
        /// <param name="gameString">The game string for the table.</param>
        protected void ProcessGame(XmlElement message,  ref ActiveGameStruct game, ref string gameString)
        {
            string type = String.Empty;
            string display = String.Empty;
            string id = String.Empty;
            string players = String.Empty;

            foreach (XmlNode n in message.Attributes)
            {
                XmlAttribute a = (XmlAttribute)n;
                
                switch (a.Name)
                {
                    case "type":
                        type = a.Value;
                        break;
                    case "display":
                        display = a.Value;
                        break;
                    case "id":
                        id = a.Value;
                        break;
                    case "players":
                        players = a.Value;
                        break;
                }
            }

            game.GameType = type;
            game.DisplayString = display;
            game.Id = new Guid(id);
            if (players != String.Empty)
            {
                gameString = type + "%" + id + "%" + players;
                game.Players = "(Players: " + players + ")";
            }
            else
            {
                gameString = type + "%" + id + "%";
                game.Players = String.Empty;
            }
        }
    }
}