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
    ////using GameObject;

    /// <summary>
    /// An abstract message that is sent between the server and table communication controllers.
    /// </summary>
    public abstract class Message
    {
        public enum MessageType
        {
            Action,
            GameState
        }

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
        /// Gets the message document.
        /// </summary>
        /// <value>The message document.</value>
        public XmlDocument MessageDocument
        {
            get{ return this.messageDoc; }
        }            

        /////// <summary>
        /////// Messages all relevent players/tables of the specified game state.
        /////// </summary>
        /////// <param name="gameState">State of the game.</param>
        /////// <returns>whether the message was constructed and sent.</returns>
        ////public bool MessageConstructSend(Game gameState);

        ////public void BuildMessage(MessageType type)
        ////{
        ////    switch(type)
        ////    {
        ////        case MessageType.Action:
        ////            MessageAction action = new MessageAction();
        ////            action.BuildMessage(
        ////}

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="game">The game to be converted into a message.</param>
        /// <returns>whether message was built.</returns>
        public abstract bool BuildMessage(Game game);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns>whether or not the message was sent successfully</returns>
        public abstract bool SendMessage();

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageDoc">The message document.</param>
        /// <returns>whether the message was processed.</returns>
        protected abstract Game ProcessMessage(XmlDocument messageDoc);

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
                catch (Exception e)
                {
                    Console.WriteLine("Error while processing the body.", e);
                }
            }

            return this.game;
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
    }
}
