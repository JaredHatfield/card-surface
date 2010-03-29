// <copyright file="MessageAction.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A message for an action that was performed on the table.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using CardGame;
    ////using GameObject;

    /// <summary>
    /// A message for an action that was performed on the table.
    /// </summary>
    public class MessageAction : Message
    {
        /// <summary>
        /// Document containing xml message.
        /// </summary>
        private XmlDocument messageDoc;

        /// <summary>
        /// game state.
        /// </summary>
        private Game game;

        /////// <summary>
        /////// game object
        /////// </summary>
        ////private GameMessage gameObject;

        /// <summary>
        /// Messages the specified game state.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        /////// <returns>
        /////// whether the message was constructed and sent.
        /////// </returns>
        ////private override bool MessageConstructSend(Game gameState)
        ////{
        ////    bool success = false;
        ////    this.game = gameState;

        ////    this.BuildMessage();
        ////    success = this.SendMessage();

        ////    return success;
        ////}

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        /// <returns>whether the Message was built.</returns>
        public override bool BuildMessage(Game gameState)
        {
            XmlElement message = this.messageDoc.DocumentElement;
            bool success = true;

            try
            {
                this.game = gameState;

                this.BuildHeader(ref message);
                this.BuildBody(ref message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Building Message", e);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns>whether or not message was sent</returns>
        public override bool SendMessage()
        {
            bool sent = false;
            
            // ValidationEventHandler schemaCheck;
            // messageDoc.Validate(schemaCheck);
            return sent;
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageDoc">The message document.</param>
        /// <returns>whether the message was processed.</returns>
        protected override Game ProcessMessage(XmlDocument messageDoc)
        {
            XmlTextReader tx = new XmlTextReader(messageDoc.InnerText);
                        
            while (tx.Read())
            {
                XmlElement element = messageDoc.CreateElement(tx.Name);
                element.InnerXml = tx.ReadInnerXml();

                switch (tx.Name)
                {
                    case "Header":
                        this.ProcessHeader(element);
                        break;
                    case "Body":
                        this.ProcessBody(element);
                        break;
                }
            }

            return this.game;
        }

        /////// <summary>
        /////// Builds the header.
        /////// </summary>
        /////// <param name="message">The message.</param>
        ////protected override void BuildHeader(ref XmlElement message)
        ////{
        ////    XmlElement header = this.messageDoc.CreateElement("Header");
        ////    DateTime time = DateTime.UtcNow;
            
        ////    header.SetAttribute("TimeStamp", time.ToString());
        ////    message.AppendChild(header);
        ////}

        /////// <summary>
        /////// Processes the header.
        /////// </summary>
        /////// <param name="element">The element to be processed.</param>
        ////protected override void ProcessHeader(XmlElement element)
        ////{            
        ////}

        /// <summary>
        /// Builds the body.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void BuildBody(ref XmlElement message)
        {
            XmlElement body = this.messageDoc.CreateElement("Body");
            
            this.BuildAction(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="element">The element to be processed.</param>
        protected override void ProcessBody(XmlElement element)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Builds the action.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildAction(ref XmlElement message)
        {
            XmlElement action = this.messageDoc.CreateElement("Action");
            
            this.BuildActionParam(ref action);
            
            ////game.action.game);
            action.SetAttribute("game", String.Empty); 

            message.AppendChild(action);
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildActionParam(ref XmlElement message)
        {
            XmlElement command = this.messageDoc.CreateElement("Command");

            this.BuildParam(ref command);

            message.AppendChild(command);
        }

        /// <summary>
        /// Builds the param.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildParam(ref XmlElement message)
        {
            XmlElement param = this.messageDoc.CreateElement("Param");

            ////game.action.command.param.name);
            param.SetAttribute("Name", String.Empty);

            ////game.action.command.param.value);
            param.SetAttribute("Value", String.Empty);

            message.AppendChild(param);
        }

        /// <summary>
        /// Processes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="actionType">Type of the action.</param>
        protected void ProcessAction(XmlElement action, string actionType)
        {
            //// Call ExecuteAction for Custom and MoveAction for Action
            foreach (XmlNode node in action.Attributes)
            {
                XmlAttribute childAttribute = this.messageDoc.CreateAttribute(node.Name);
                childAttribute.InnerXml = node.InnerXml;

                switch (actionType)
                {
                    case "Move":
                        ////ProcessMoveAction();
                        ////gameObject.MoveAction();
                        break;
                    case "Custom":
                        ////gameObject.ExecuteAction();
                        break;
                }
            }
        }
    }
}
