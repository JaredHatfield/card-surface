// <copyright file="MessageRequestGame.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A request to join or start a new game.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using CardGame;

    /// <summary>
    /// A reflection of the current game state.
    /// </summary>
    public class MessageRequestGame : Message
    {
        /// <summary>
        /// The string representation of the game type.
        /// </summary>
        private string gameType;

        /// <summary>
        /// The guid of the game to join.
        /// </summary>
        private Guid gameGuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestGame"/> class.
        /// </summary>
        public MessageRequestGame()
        {
            MessageTypeName = MessageType.RequestGame.ToString();
        }

        /// <summary>
        /// Gets the type of the game.
        /// </summary>
        /// <value>The type of the game.</value>
        public string GameType
        {
            get { return this.gameType; }
        }

        /// <summary>
        /// Gets the game GUID.
        /// </summary>
        /// <value>The game GUID.</value>
        public Guid GameGuid
        {
            get { return this.gameGuid; }
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
        /// Builds the message.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(string gameType)
        {
            bool success = true;

            if (gameType.Contains("-") && gameType.Length == Guid.Empty.ToString().Length)
            {
                this.gameGuid = new Guid(gameType);
            }
            else
            {
                this.gameType = gameType;
            }

            success = BuildM();

            return success;
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="gameGuid">The game GUID.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(Guid gameGuid)
        {
            bool success = true;

            this.gameGuid = gameGuid;

            success = BuildM();

            return success;
        }
        
        /// <summary>
        /// Builds the body.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void BuildBody(ref XmlElement message)
        {
            XmlElement body = this.MessageDocument.CreateElement("Body");

            this.BuildRequestGame(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="body">The body to be processed.</param>
        protected override void ProcessBody(XmlElement body)
        {
            XmlElement child = (XmlElement)body.FirstChild;

            this.ProcessRequestGame(child);
        }

        /// <summary>
        /// Builds the request game list.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildRequestGame(ref XmlElement message)
        {
            XmlElement game = this.MessageDocument.CreateElement("RequestGame");
            string name = String.Empty;

            if (this.gameType != null)
            {
                game.SetAttribute("GameType", this.gameType);
            }
            else
            {
                game.SetAttribute("GameGuid", this.gameGuid.ToString());
            }

            message.AppendChild(game);
        }

        /// <summary>
        /// Processes the request game.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void ProcessRequestGame(XmlElement message)
        {
            foreach (XmlNode node in message.Attributes)
            {
                XmlAttribute a = (XmlAttribute)node;

                switch (node.Name)
                {
                    case "GameType":
                        this.gameType = a.Value;
                        break;
                    case "GameGuid":
                        this.gameGuid = new Guid(a.Value);
                        break;
                }
            }
        }
    }
}
