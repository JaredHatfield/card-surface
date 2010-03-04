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

        /// <summary>
        /// Messages the specified game state.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        public override void MessageConstructSend(Game gameState)
        {
            this.game = gameState;

            this.BuildMessage();
            this.SendMessage();
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        public override void BuildMessage()
        {
            XmlElement message = this.messageDoc.DocumentElement;

            this.BuildHeader(ref message);
            this.BuildActionParam(ref message);
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
        /// Builds the header.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void BuildHeader(ref XmlElement message)
        {
            XmlElement header = this.messageDoc.CreateElement("Header");
            DateTime time = DateTime.UtcNow;

            header.SetAttribute("TimeStamp", time.ToString());
            message.AppendChild(header);
        }

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
        protected override void BuildActionParam(ref XmlElement message)
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
    }
}
