// <copyright file="Message.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract message that is sent between the server and table communication controllers.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using CardGame;
    using CommunicationException;

    /// <summary>
    /// An abstract message that is sent between the server and table communication controllers.
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Document containing xml message.
        /// </summary>
        private XmlDocument messageDoc;

        /// <summary>
        /// game state.
        /// </summary>
        private Game game;

        /// <summary>
        /// The String representation of the Message Type.
        /// </summary>
        private string messageTypeName;

        /// <summary>
        /// Whether the message was validated successfully.
        /// </summary>
        private bool validated = true;

        /////// <summary>
        /////// Whether the xmlDocumnet was validated against the schema.
        /////// </summary>
        ////private bool schemaValidated = true;

        /////// <summary>
        /////// game object
        /////// </summary>
        ////private GameMessage gameObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        internal Message()
        {
            string filePath = Directory.GetCurrentDirectory();
            string parent = "card-surface\\card-surface\\";
            int index = 0;

            index = filePath.IndexOf(parent);
            if (index != 0)
            {
                filePath = filePath.Remove(index + parent.Length);
                filePath = filePath + "CardCommunication\\Messages\\MessageSchema.xsd";
            }

            this.messageDoc = new XmlDocument();
            this.messageDoc.Schemas.Add(null, filePath);
        }

        /// <summary>
        /// The types of Messages.
        /// </summary>
        public enum MessageType
        {
            /// <summary>
            /// The Action Message.
            /// </summary>
            Action,

            /// <summary>
            /// The Existing Games Message
            /// </summary>
            ExistingGames,

            /// <summary>
            /// The Flip Card Message.
            /// </summary>
            FlipCard,

            /// <summary>
            /// The Game List Message.
            /// </summary>
            GameList,

            /// <summary>
            /// The Request Existing Games Message.
            /// </summary>
            RequestExistingGames,

            /// <summary>
            /// The Request Game Message.
            /// Requests to join or start a new game.
            /// </summary>
            RequestGame,

            /// <summary>
            /// The Request Game List Message.
            /// </summary>
            RequestGameList
        }

        /// <summary>
        /// Gets the message document.
        /// </summary>
        /// <value>The message document.</value>
        public XmlDocument MessageDocument
        {
            get { return this.messageDoc; }
        }

        /// <summary>
        /// Gets or sets the string representation of the message type.
        /// </summary>
        /// <value>The string representation of the message type.</value>
        public string MessageTypeName
        {
            get { return this.messageTypeName; }
            set { this.messageTypeName = value; }
        }

        /// <summary>
        /// Gets or sets the game.
        /// </summary>
        /// <value>The game object.</value>
        public Game Game
        {
            get { return this.game; }
            set { this.game = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Message"/> is validated.
        /// </summary>
        /// <value><c>true</c> if validated; otherwise, <c>false</c>.</value>
        public bool Validated
        {
            get { return this.validated; }
        }

        /// <summary>
        /// Converts to game.
        /// </summary>
        /// <param name="message">The message to be converted to a game.</param>
        /// <returns>the game formed from the message.</returns>
        public Game ConvertToGame(XmlDocument message)
        {
            XmlTextReader tx = new XmlTextReader(this.messageDoc.InnerText);

            while (tx.Read())
            {
                XmlNodeList nodeList = null;
                XmlNode node = null;

                //// Look at the first element of Body to find the type of Message
                nodeList = this.messageDoc.GetElementsByTagName("Body");

                node = nodeList.Item(0);
                XmlElement element = this.messageDoc.CreateElement(node.Name);
                element.InnerXml = node.InnerXml;

                try
                {
                    bool found = false;

                    foreach (XmlNode n in element.ChildNodes)
                    {
                        //// Don't seek if type has already been found.
                        if (n.NodeType == XmlNodeType.Element && !found)
                        {
                            switch (n.Name)
                            {
                                case "Action":
                                    found = true;
                                    MessageAction messageAction = new MessageAction();
                                    ////this.gameObject = messageAction.ProcessMessage(message);
                                    break;
                                case "GameState":
                                    found = true;
                                    MessageGameState messageGameState = new MessageGameState();
                                    ////this.gameObject = messageGameState.ProcessMessage(message);
                                    break;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw new MessageTransportException("Message.ConvertToGame");
                }
            }

            return this.game;
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageDoc">The message document.</param>
        public abstract void ProcessMessage(XmlDocument messageDoc);

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <returns>whether the Message was built.</returns>
        protected bool BuildM()
        {
            XmlElement message = this.MessageDocument.CreateElement("Message");
            bool success = true;

            try
            {
                ValidationEventHandler veh = new ValidationEventHandler(this.Error);

                message.SetAttribute("MessageType", this.messageTypeName);
                this.BuildHeader(ref message);
                this.BuildBody(ref message);

                this.messageDoc.InnerXml = message.OuterXml;
                this.messageDoc.Validate(veh);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Building Message", e);
                success = false;
                throw new MessageTransportException("BuildM");
            }

            return success;
        }

        /// <summary>
        /// Errors the specified sender.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The <see cref="System.Xml.Schema.ValidationEventArgs"/> instance containing the event data.</param>
        protected void Error(object sender, ValidationEventArgs e)
        {
            Console.WriteLine(e.Severity + ".  " + e.Message + "  " + e.Exception);
            this.validated = false;
            throw new MessageTransportException("Message Validation Error");
        }

        /// <summary>
        /// Builds the header.
        /// </summary>
        /// <param name="message">The message to be built.</param>
        protected void BuildHeader(ref XmlElement message)
        {
            XmlElement header = this.messageDoc.CreateElement("Header");
            DateTime time = DateTime.UtcNow;

            header.SetAttribute("TimeStamp", time.ToString());
            message.AppendChild(header);
        }

        /// <summary>
        /// Processes the header.
        /// </summary>
        /// <param name="element">The element to be processed.</param>
        protected void ProcessHeader(XmlElement element)
        {
        }

        /// <summary>
        /// Builds the body.
        /// </summary>
        /// <param name="message">The message to be built.</param>
        protected abstract void BuildBody(ref XmlElement message);

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="element">The element to be processed.</param>
        protected abstract void ProcessBody(XmlElement element);
        
        /////// <summary>
        /////// Validates the schema.
        /////// </summary>
        /////// <param name="sender">The sender.</param>
        /////// <param name="e">The <see cref="System.Xml.Schema.ValidationEventArgs"/> instance containing the event data.</param>
        ////private void ValidateSchema(object sender, ValidationEventArgs e)
        ////{
        ////    Console.WriteLine("Schema Validation.", e);
        ////    ////this.schemaValidated = false;
        ////}
    }
}
