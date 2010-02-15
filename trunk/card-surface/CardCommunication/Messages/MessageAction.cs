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
    using CardGame;

    /// <summary>
    /// A message for an action that was performed on the table.
    /// </summary>
    public class MessageAction : Message
    {
        /// <summary>
        /// Document containing xml message.
        /// </summary>
        XmlDocument messageDoc;

        /// <summary>
        /// game state.
        /// </summary>
        Game game;

        /// <summary>
        /// Messages the specified game state.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        public override void MessageConstructSend(Game gameState)
        {
            game = gameState;
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        public override void BuildMessage()
        {
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns></returns>
        public override bool SendMessage()
        {
        }

        /// <summary>
        /// Builds the header.
        /// </summary>
        protected override void BuildHeader()
        {
            XmlElement header = messageDoc.CreateElement("Header");

            header.SetAttribute("TimeStamp", val);
        }

        /// <summary>
        /// Builds the action.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildAction(ref XmlElement message)
        {
            XmlElement action = messageDoc.CreateElement("Action");

            BuildCommand(ref action);

            action.SetAttribute("Table", game.action.table);
            messageDoc.AppendChild(action);
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void BuildCommand(ref XmlElement message)
        {
            XmlElement command = messageDoc.CreateElement("Command");

            BuildParam(ref command);

            messageDoc.AppendChild(command);
        }

        /// <summary>
        /// Builds the param.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildParam(ref XmlElement message)
        {
            XmlElement param = messageDoc.CreateElement("Param");

            param.SetAttribute("Name", game.action.command.param.name);
            param.SetAttribute("Value", game.action.command.param.value);

            message.AppendChild(param);
        }
    }
}
