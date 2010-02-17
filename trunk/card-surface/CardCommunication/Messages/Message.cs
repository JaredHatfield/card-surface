// <copyright file="Message.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An abstract message that is sent between the server and table communication controllers.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using CardGame;

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
        /// Messages all relevent players/tables of the specified game state.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        public abstract void MessageConstructSend(Game gameState);

        /// <summary>
        /// Builds the message.
        /// </summary>
        public abstract void BuildMessage();

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns>whether or not the message was sent successfully</returns>
        public abstract bool SendMessage();

        /// <summary>
        /// Builds the header.
        /// </summary>
        protected abstract void BuildHeader();

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="message">The message.</param>
        protected abstract void BuildCommand(ref XmlElement message);
    }
}
