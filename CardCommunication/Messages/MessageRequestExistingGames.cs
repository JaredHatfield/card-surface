// <copyright file="MessageRequestExistingGames.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A request for a list of existing games.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using CommunicationException;

    /// <summary>
    /// A Message request for a list of existing games.
    /// </summary>
    public class MessageRequestExistingGames : Message
    {
        /// <summary>
        /// String representation of the selected Game.
        /// </summary>
        private string selectedGame = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestExistingGames"/> class.
        /// </summary>
        public MessageRequestExistingGames()
        {
            MessageTypeName = MessageType.RequestExistingGames.ToString();
        }

        /// <summary>
        /// Gets the selected game.
        /// </summary>
        /// <value>The selected game.</value>
        public string SelectedGame
        {
            get { return this.selectedGame; }
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="selectedGame">The selected game.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(string selectedGame)
        {
            XmlElement message = this.MessageDocument.CreateElement("Message");
            bool success = true;

            this.selectedGame = selectedGame;

            try
            {
                message.SetAttribute("MessageType", this.MessageTypeName);
                this.BuildHeader(ref message);
                this.BuildBody(ref message);

                this.MessageDocument.InnerXml = message.OuterXml;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Building Message" + e.ToString());
                success = false;
                throw new MessageTransportException("Error Building Request Existing Games Message.", e);
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
        /// Processes the body.
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        protected override void BuildBody(ref XmlElement message)
        {
            XmlElement body = MessageDocument.CreateElement("Body");

            this.BuildGameType(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        protected override void ProcessBody(XmlElement message)
        {
            foreach (XmlNode n in message.ChildNodes)
            {
                XmlElement existingGamesElement = (XmlElement)n;
                existingGamesElement.InnerXml = n.InnerXml;

                switch (existingGamesElement.Name)
                {
                    case "GameType":
                        this.ProcessGameType(existingGamesElement);
                        break;
                }
            }
        }

        /// <summary>
        /// Builds the existing games.
        /// </summary>
        /// <param name="message">The existing games.</param>
        protected void BuildGameType(ref XmlElement message)
        {
            XmlElement existingGames = this.MessageDocument.CreateElement("GameType");

            existingGames.SetAttribute("SelectedType", this.selectedGame);

            message.AppendChild(existingGames);
        }

        /// <summary>
        /// Processes the existing games.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void ProcessGameType(XmlElement message)
        {
            foreach (XmlAttribute a in message.Attributes)
            {
                switch (a.Name)
                {
                    case "SelectedType":
                        this.selectedGame = a.Value;
                        break;
                }
            }
        }
    }
}
