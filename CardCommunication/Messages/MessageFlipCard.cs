// <copyright file="MessageFlipCard.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A message for an action that was performed on the table.</summary>
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
    public class MessageFlipCard : Message
    {
        /// <summary>
        /// Guid of the card to be flipped.
        /// </summary>
        private Guid cardGuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageFlipCard"/> class.
        /// </summary>
        public MessageFlipCard()
        {
            MessageTypeName = MessageType.Action.ToString();
        }

        /// <summary>
        /// Gets the card GUID.
        /// </summary>
        /// <value>The card GUID.</value>
        public Guid CardGuid
        {
            get { return this.cardGuid; }
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="cardGuid">The card GUID.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(Guid cardGuid)
        {
            bool success = true;

            this.cardGuid = cardGuid;
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

            this.BuildFlipCard(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="element">The element to be processed.</param>
        protected override void ProcessBody(XmlElement element)
        {
            XmlElement action = this.MessageDocument.CreateElement("FlipCard");

            action.InnerXml = element.InnerXml;
            this.ProcessAction(action);
        }

        /// <summary>
        /// Builds the action.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildFlipCard(ref XmlElement message)
        {
            XmlElement action = this.MessageDocument.CreateElement("FlipCard");

            action.SetAttribute("cardGuid", this.cardGuid.ToString());

            message.AppendChild(action);
        }

        /// <summary>
        /// Processes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void ProcessAction(XmlElement action)
        {
            string card = String.Empty;
            foreach (XmlAttribute a in action.Attributes)
            {
                if (a.Name == "CardGuid")
                {
                    card = a.Value;
                }
            }

            this.cardGuid = new Guid(card);
        }
    }
}
