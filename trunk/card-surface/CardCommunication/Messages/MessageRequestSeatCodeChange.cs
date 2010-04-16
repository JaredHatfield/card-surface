// <copyright file="MessageRequestSeatCodeChange.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A message for a request to change a seat password.</summary>
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
    ////using GameObject;

    /// <summary>
    /// A message for a request to change a seat password.
    /// </summary>
    public class MessageRequestSeatCodeChange : Message
    {
        /// <summary>
        /// Guid of the seat to get a new password.
        /// </summary>
        private Guid seatGuid;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestSeatCodeChange"/> class.
        /// </summary>
        public MessageRequestSeatCodeChange()
        {
            MessageTypeName = MessageType.RequestSeatCodeChange.ToString();
        }

        /// <summary>
        /// Gets the card GUID.
        /// </summary>
        /// <value>The card GUID.</value>
        public Guid CardGuid
        {
            get { return this.seatGuid; }
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="seatGuid">The seat GUID.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(Guid seatGuid)
        {
            bool success = true;

            this.seatGuid = seatGuid;
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

            this.BuildRequestSeatCodeChange(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="element">The element to be processed.</param>
        protected override void ProcessBody(XmlElement element)
        {
            XmlElement child = (XmlElement)element.FirstChild;

            child.InnerXml = element.InnerXml;
            this.ProcessRequestSeatCodeChange(child);
        }

        /// <summary>
        /// Builds the action.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildRequestSeatCodeChange(ref XmlElement message)
        {
            XmlElement action = this.MessageDocument.CreateElement("Seat");

            action.SetAttribute("seatGuid", this.seatGuid.ToString());

            message.AppendChild(action);
        }

        /// <summary>
        /// Processes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void ProcessRequestSeatCodeChange(XmlElement action)
        {
            string seat = String.Empty;
            foreach (XmlAttribute a in action.Attributes)
            {
                if (a.Name == "seatGuid")
                {
                    seat = a.Value;
                }
            }

            this.seatGuid = new Guid(seat);
        }
    }
}

