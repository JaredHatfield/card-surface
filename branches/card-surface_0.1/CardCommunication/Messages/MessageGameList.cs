// <copyright file="MessageGameList.cs" company="University of Louisville Speed School of Engineering">
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
    using CardCommunication.CommunicationException;
    using CardGame;

    /// <summary>
    /// A message to send the list of games.
    /// </summary>
    public class MessageGameList : Message
    {
        /// <summary>
        /// ReadOnlyCollection of games that can be played.
        /// </summary>
        private ReadOnlyCollection<string> gameNames;

        /// <summary>
        /// Collection of games that can be played.
        /// </summary>
        private Collection<string> gameNameList;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageGameList"/> class.
        /// </summary>
        public MessageGameList()
        {
            MessageTypeName = MessageType.GameList.ToString();
        }

        /// <summary>
        /// Gets the game name list.
        /// </summary>
        /// <value>The game name list.</value>
        public Collection<string> GameNameList
        {
            get { return this.gameNameList; }
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="gameNameList">The game name list.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(ReadOnlyCollection<string> gameNameList)
        {
            XmlElement message = this.MessageDocument.CreateElement("Message");
            bool success = true;

            try
            {
                this.gameNames = gameNameList;
                
                message.SetAttribute("MessageType", this.MessageTypeName);
                this.BuildHeader(ref message);
                this.BuildBody(ref message);

                this.MessageDocument.InnerXml = message.OuterXml;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error Building Message" + e.ToString());
                success = false;
                throw new MessageTransportException("MessageGameList exception.", e);
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
                    case "GameList":
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
            XmlElement gameList = this.MessageDocument.CreateElement("GameList");
            string name = String.Empty;

            foreach (string n in this.gameNames)
            {
                name = name.Insert(0, n + ".");
            }

            gameList.SetAttribute("NameList", name);

            message.AppendChild(gameList);
        }

        /// <summary>
        /// Processes the game list.
        /// </summary>
        /// <param name="gameList">The game list.</param>
        protected void ProcessGameList(XmlElement gameList)
        {
            foreach (XmlNode node in gameList.Attributes)
            {
                XmlAttribute childAttribute = (XmlAttribute)node;
                childAttribute.InnerXml = node.InnerXml;

                switch (childAttribute.Name)
                {
                    case "NameList":
                        this.ParseGameList(childAttribute.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Parses the game list.
        /// </summary>
        /// <param name="gameList">The game list.</param>
        protected void ParseGameList(string gameList)
        {
            ////DO SOMETHING to update the game state.
            Collection<string> gameListNames = new Collection<string>();
            char[] splitChar = { '.' };
            string[] name = gameList.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < name.Length; i++)
            {
                gameListNames.Add(name[i]);
            }

            this.gameNameList = gameListNames;
        }
    }
}
