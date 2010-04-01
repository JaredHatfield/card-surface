﻿// <copyright file="MessageGameList.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An message that is sent to the server communication controllers
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
    public class MessageGameList : Message
    {
        /////// <summary>
        /////// Initializes a new instance of the <see cref="MessageGameList"/> class.
        /////// </summary>
        ////public MessageGameList()
        ////{
        ////}

        /////// <summary>
        /////// Sends the message.
        /////// </summary>
        /////// <returns>whether or not message was sent</returns>
        ////public override bool SendMessage()
        ////{
        ////    bool sent = false;

        ////    // ValidationEventHandler schemaCheck;
        ////    // MessageDocument.Validate(schemaCheck);
        ////    return sent;
        ////}

        /// <summary>
        /// ReadOnlyCollection of games that can be played.
        /// </summary>
        private ReadOnlyCollection<string> gameNameList;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageGameList"/> class.
        /// </summary>
        public MessageGameList()
        {
            MessageTypeName = "MessageGameList";
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
                this.gameNameList = gameNameList;
                
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

            return this.Game;
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
                XmlElement childElement = MessageDocument.CreateElement(node.Name);
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

            foreach (string n in this.gameNameList)
            {
                name.Insert(0, n + ".");
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
        }
    }
}