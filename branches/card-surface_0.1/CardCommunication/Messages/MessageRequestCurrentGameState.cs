// <copyright file="MessageRequestCurrentGameState.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A message for a request for the current game state.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using CardGame;

    /// <summary>
    /// A message for an action that was performed on the table.
    /// </summary>
    public class MessageRequestCurrentGameState : Message
    {
        /// <summary>
        /// Guid of the seat to get a new password.
        /// </summary>
        private Guid gameGuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestCurrentGameState"/> class.
        /// </summary>
        public MessageRequestCurrentGameState()
        {
            MessageTypeName = MessageType.RequestCurrentGameState.ToString();
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
        /// Builds the message.
        /// </summary>
        /// <param name="gameGuid">The game GUID.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(Guid gameGuid)
        {
            bool success = true;

            this.gameGuid = gameGuid;
            success = this.BuildM();

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

            this.BuildRequestCurrentGameState(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="element">The element to be processed.</param>
        protected override void ProcessBody(XmlElement element)
        {
            XmlElement child = (XmlElement)element.FirstChild;

            this.ProcessRequestCurrentGameState(child);
        }

        /// <summary>
        /// Builds the action.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildRequestCurrentGameState(ref XmlElement message)
        {
            XmlElement state = this.MessageDocument.CreateElement("RequestCurrentGameState");

            state.SetAttribute("gameGuid", this.gameGuid.ToString());

            message.AppendChild(state);
        }

        /// <summary>
        /// Processes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void ProcessRequestCurrentGameState(XmlElement action)
        {
            string gameGuid = String.Empty;
            foreach (XmlAttribute a in action.Attributes)
            {
                if (a.Name == "gameGuid")
                {
                    gameGuid = a.Value;
                }
            }

            this.gameGuid = new Guid(gameGuid);
        }
    }
}

