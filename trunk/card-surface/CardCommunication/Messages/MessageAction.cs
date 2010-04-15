// <copyright file="MessageAction.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A message for an action that was performed on the table.</summary>
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

    /// <summary>
    /// A message for an action that was performed on the table.
    /// </summary>
    public class MessageAction : Message
    {
        /// <summary>
        /// Action and Parameters.
        /// </summary>
        private Collection<string> action = new Collection<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageAction"/> class.
        /// </summary>
        public MessageAction()
        {
            MessageTypeName = MessageType.Action.ToString();
        }

        /// <summary>
        /// Enumeration of all Action Types.
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// Move Action.
            /// </summary>
            Move,

            /// <summary>
            /// Custom defined Action.
            /// </summary>
            Custom
        }

        /// <summary>
        /// Gets the action and parameters.
        /// </summary>
        /// <value>The action and parameters.</value>
        public Collection<string> Action
        {
            get { return this.action; }
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        /// <param name="action">The action, then parameters</param>
        /// <returns>whether the message was built.</returns>
        public bool BuildMessage(Collection<string> action)
        {
            bool success = true;

            this.action = action;
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

        /////// <summary>
        /////// Builds the header.
        /////// </summary>
        /////// <param name="message">The message.</param>
        ////protected override void BuildHeader(ref XmlElement message)
        ////{
        ////    XmlElement header = this.MessageDocument.CreateElement("Header");
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
            XmlElement body = this.MessageDocument.CreateElement("Body");
      
            this.BuildAction(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="element">The element to be processed.</param>
        protected override void ProcessBody(XmlElement element)
        {
            XmlElement action = this.MessageDocument.CreateElement("Action");
            
            action.InnerXml = element.InnerXml;
            this.ProcessAction(action);
        }

        /// <summary>
        /// Builds the action.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildAction(ref XmlElement message)
        {
            XmlElement action = this.MessageDocument.CreateElement("Action");
            
            this.BuildActionParam(ref action);
            
            ////game.action.game);
            action.SetAttribute("game", String.Empty); 

            message.AppendChild(action);
        }

        /// <summary>
        /// Processes the action.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void ProcessAction(XmlElement message)
        {
            XmlElement action = (XmlElement)message.FirstChild;

            this.ProcessParam(action);
        }
        
        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildActionParam(ref XmlElement message)
        {
            XmlElement command = this.MessageDocument.CreateElement(this.action[0]);

            for (int i = 1; i < this.action.Count; i++)
            {
                this.BuildParam(ref command, i);
            }

            message.AppendChild(command);
        }

        /// <summary>
        /// Processes the action.
        /// </summary>
        /// <param name="action">The action.</param>
        protected void ProcessActionParam(XmlElement action)
        {
            string gameGuid = String.Empty;
            string actionType = String.Empty;

            foreach (XmlAttribute a in action.Attributes)
            {
                if (a.Name == "Game")
                {
                    gameGuid = a.Value;
                }
            }

            this.action.Add(gameGuid);

            foreach (XmlNode node in action.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlElement actionParam = (XmlElement)node;

                    actionType = node.Name;
                    this.ProcessParam(actionParam);
                }
            }       
    
            this.action.Add(actionType);
       }

        /// <summary>
        /// Builds the param.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="counter">The counter.</param>
        protected void BuildParam(ref XmlElement message, int counter)
        {
            XmlElement param = this.MessageDocument.CreateElement("Param");

            param.SetAttribute("value", this.action[counter]);

            message.AppendChild(param);
        }

        /// <summary>
        /// Processes the param.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void ProcessParam(XmlElement message)
        {
            XmlElement child = (XmlElement)message.FirstChild;

            XmlElement subChild = (XmlElement)child.FirstChild;

            this.ProcessParamSubElements(subChild);
        }

        /// <summary>
        /// Processes the action param.
        /// </summary>
        /// <param name="action">The action parameter to be processed.</param>
        protected void ProcessParamSubElements(XmlElement action)
        {
            string name = String.Empty;

            foreach (XmlNode node in action.Attributes)
            {
                XmlAttribute childAttribute = this.MessageDocument.CreateAttribute(node.Name);

                childAttribute.InnerXml = node.InnerXml;

                this.action.Add(childAttribute.Value);
            }
        }
    }
}
