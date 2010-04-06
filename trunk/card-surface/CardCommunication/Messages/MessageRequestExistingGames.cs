// <copyright file="MessageRequestExistingGames.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A request for a list of existing games.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// A Message request for a list of existing games.
    /// </summary>
    public class MessageRequestExistingGames : Message
    {
        /// <summary>
        /// String representation of the selected Game.
        /// </summary>
        private string selectedGame = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestExistingGames"/> class.
        /// </summary>
        public MessageRequestExistingGames()
        {
            MessageTypeName = MessageType.RequestExistingGames.ToString();
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="selectedGame">The selected game.</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(string selectedGame)
        {
            XmlElement message = this.MessageDocument.CreateElement("Message");
            bool success = true;

            this.selectedGame = selectedGame;

            try
            {
                message.SetAttribute("MessageType", this.MessageTypeName);
                this.BuildHeader(ref message);
                this.BuildBody(ref message);

                this.MessageDocument.InnerXml = message.OuterXml;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Building Message", e);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageDoc">The message document.</param>
        public override void ProcessMessage(XmlDocument messageDoc)
        {            
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        protected override void BuildBody(ref XmlElement message)
        {
            XmlElement body = MessageDocument.CreateElement("Body");

            this.BuildExistingGames(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="message">The message to be processed.</param>
        protected override void ProcessBody(XmlElement message)
        {
        }

        /// <summary>
        /// Builds the existing games.
        /// </summary>
        /// <param name="existingGames">The existing games.</param>
        protected void BuildExistingGames(ref XmlElement existingGames)
        {
            existingGames.SetAttribute("SelectedType", this.selectedGame);
        }
    }
}
