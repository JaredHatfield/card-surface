// <copyright file="MessageGameState.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A message for an action that was performed on the table.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using CardGame;

    /// <summary>
    /// A message for a game state.
    /// </summary>
    public class MessageGameState : Message
    {
        /// <summary>
        /// The serialized game object.
        /// </summary>
        private string serializedGame;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageGameState"/> class.
        /// </summary>
        public MessageGameState()
        {
            MessageTypeName = MessageType.GameState.ToString();
        }

        /// <summary>
        /// Gets the serialized game.
        /// </summary>
        /// <value>The serialized game.</value>
        public string SerializedGame
        {
            get { return this.serializedGame; }
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

        /////// <summary>
        /////// Builds the message.
        /////// </summary>
        /////// <param name="serializedGame">The serialized game.</param>
        /////// <returns>whether the message was built.</returns>
        ////public bool BuildMessage(string serializedGame)
        ////{
        ////    bool success = true;

        ////    this.serializedGame = serializedGame;

        ////    success = base.BuildMessage();

        ////    return success;
        ////}

        /// <summary>
        /// Builds the body.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildBody(ref XmlElement message)
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
            XmlElement game = this.MessageDocument.CreateElement("GameState");
            string name = String.Empty;

            game.SetAttribute("gameObject", this.serializedGame);

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
                    case "gameState":
                        this.serializedGame = a.Value;
                        break;
                }
            }
        }
    }
}
