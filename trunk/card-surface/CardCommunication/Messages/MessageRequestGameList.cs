// <copyright file="MessageRequestGameList.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An message that is sent to the server communication controller
// to retreive a list of playable games.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using CardGame;
    ////using GameObject;

    /// <summary>
    /// A message to send the list of games.
    /// </summary>
    public class MessageRequestGameList : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestGameList"/> class.
        /// </summary>
        public MessageRequestGameList()
        {
            MessageTypeName = MessageType.RequestGameList.ToString();
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
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage()
        {
            bool success = true;

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

            this.BuildRequestGameList(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="body">The body to be processed.</param>
        protected override void ProcessBody(XmlElement body)
        {
            foreach (XmlNode node in body.Attributes)
            {
                XmlAttribute a = (XmlAttribute)node;

                switch (node.Name)
                {
                    case "MessageType":
                        break;
                }
            }
        }

        /// <summary>
        /// Builds the request game list.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildRequestGameList(ref XmlElement message)
        {
            XmlElement gameList = this.MessageDocument.CreateElement("RequestGameList");
            string name = String.Empty;

            gameList.SetAttribute("MessageType", MessageTypeName);

            message.AppendChild(gameList);
        }
    }
}
