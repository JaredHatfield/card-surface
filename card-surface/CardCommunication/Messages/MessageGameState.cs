// <copyright file="MessageGameState.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A reflection of the current game state.</summary>
namespace CardCommunication.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using CardGame;
    using GameBlackjack;
    ////using GameObject;

    /// <summary>
    /// A reflection of the current game state.
    /// </summary>
    public class MessageGameState : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageGameState"/> class.
        /// </summary>
        public MessageGameState()
        {
            ////MessageTypeName = MessageType.GameState.ToString();
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageDoc">The message document.</param>
        public override void ProcessMessage(XmlDocument messageDoc)
        {
            //// Do not implement; this code is never executed.
            throw new NotImplementedException();
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

        /// <summary>
        /// Builds the body.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void BuildBody(ref XmlElement message)
        {
            XmlElement body = this.MessageDocument.CreateElement("Body");

            this.BuildGame(ref body);

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
        /// Builds the game state.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildGame(ref XmlElement message)
        {
            XmlElement gameState = this.MessageDocument.CreateElement("Game");

            this.BuildStatus(ref gameState);
            this.BuildMessageValue(ref gameState);
            this.BuildPlayers(ref gameState);
            this.BuildArea(ref gameState);

            message.AppendChild(gameState);
        }

        /// <summary>
        /// Builds the status.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildStatus(ref XmlElement message)
        {
            XmlElement status = this.MessageDocument.CreateElement("Status");

            ////game.turn);
            status.SetAttribute("turn", String.Empty); 

            message.AppendChild(status);
        }

        /// <summary>
        /// Builds the message value.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildMessageValue(ref XmlElement message)
        {
            XmlElement value = this.MessageDocument.CreateElement("Message");

            ////game.value);
            value.SetAttribute("Value", String.Empty); 

            message.AppendChild(value);
        }

        /// <summary>
        /// Builds the players.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildPlayers(ref XmlElement message)
        {
            XmlElement players = this.MessageDocument.CreateElement("Players");

            ////foreach (Player player in game.players)
            ////{
                this.BuildPlayer(ref players);
            ////}

            message.AppendChild(players);
        }

        /// <summary>
        /// Builds the player.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildPlayer(ref XmlElement message)
        {
            XmlElement player = this.MessageDocument.CreateElement("Player");
            
            ////game.player.id);
            player.SetAttribute("ID", String.Empty);
            ////game.player.balance);
            player.SetAttribute("Balance", String.Empty); 

            this.BuildArea(ref player);
            this.BuildHand(ref player);
            
            ////this.BuildAction(ref player);
            message.AppendChild(player);
        }

        /// <summary>
        /// Builds the area.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildArea(ref XmlElement message)
        {
            XmlElement area = this.MessageDocument.CreateElement("Area");

            this.BuildCardCollection(ref area);
            this.BuildChipPile(ref area);

            message.AppendChild(area);
        }

        /// <summary>
        /// Builds the collection of card piles.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildCardCollection(ref XmlElement message)
        {
            XmlElement cardCollection = this.MessageDocument.CreateElement("CardCollection");

            ////foreach (CardPile cardPile in game.player.cardPiles)
            ////{
            this.BuildCardPile(ref cardCollection);
            ////}
            message.AppendChild(cardCollection);
        }

        /// <summary>
        /// Builds the collection of chip pile.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildChipPile(ref XmlElement message)
        {
            XmlElement chipPile = this.MessageDocument.CreateElement("ChipPile");

            ////foreach (Chip chip in game.player.chips)
            ////{
                this.BuildChip(ref chipPile);

                ////game.player.area.chippile.open);
                chipPile.SetAttribute("open", String.Empty); 
                
            ////}
                message.AppendChild(chipPile);
        }

        /// <summary>
        /// Builds the card pile.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildCardPile(ref XmlElement message)
        {
            XmlElement cardPile = this.MessageDocument.CreateElement("CardPile");

            ////foreach (Card card in game.player.cardPiles.cardPile)
            ////{
            this.BuildCard(ref cardPile);

            ////}

            ////game.player.area.cardpiles.cardpile.id);
            cardPile.SetAttribute("id", String.Empty);
            ////game.player.area.cardpiles.cardpile.playable);
            cardPile.SetAttribute("playable", String.Empty);
            ////game.player.area.cardpiles.cardpile.style);
            cardPile.SetAttribute("style", String.Empty);
            ////game.player.area.cardpiles.cardpile.expandable);
            cardPile.SetAttribute("expandable", String.Empty);

            message.AppendChild(cardPile);
        }

        /// <summary>
        /// Builds the card.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildCard(ref XmlElement message)
        {
            XmlElement card = this.MessageDocument.CreateElement("Card");

            ////game.player.area.cardpiles.cardpile.card.id);
            card.SetAttribute("guid", String.Empty);
            ////game.player.area.cardpiles.cardpile.card.status);
            card.SetAttribute("status", String.Empty);
            ////game.player.area.cardpiles.cardpile.card.moveable);
            card.SetAttribute("moveable", String.Empty); 

            message.AppendChild(card);
        }

        /// <summary>
        /// Builds the chip.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildChip(ref XmlElement message)
        {
            XmlElement chip = this.MessageDocument.CreateElement("Chip");

            ////game.player.area.chippile.chip.guid);
            chip.SetAttribute("guid", String.Empty);
            ////game.player.area.chippile.chip.value);
            chip.SetAttribute("value", String.Empty);
            ////game.player.area.chippile.chip.color);
            chip.SetAttribute("color", String.Empty);
            ////game.player.area.chippile.chip.moveable);
            chip.SetAttribute("moveable", String.Empty); 

            message.AppendChild(chip);
        }

        /// <summary>
        /// Builds the hand.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildHand(ref XmlElement message)
        {
            XmlElement hand = this.MessageDocument.CreateElement("Hand");

            this.BuildCardPile(ref hand);

            message.AppendChild(hand);
        }

        /// <summary>
        /// Builds the commands.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildAction(ref XmlElement message)
        {
            XmlElement action = this.MessageDocument.CreateElement("Action");

            ////foreach (Command com in game.player.commands)
            ////{
                this.BuildActionParam(ref action);
            ////}
                
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
            XmlElement param = this.MessageDocument.CreateElement("Param");

            ////game.player.command.name);
            param.SetAttribute("Name", String.Empty);

            ////game.player.command.action);
            param.SetAttribute("Value", String.Empty); 

            message.AppendChild(param);
        }

        ////protected bool ProcessXMLElement(ref GameMessage game, XmlElement element)
        ////{
        ////    bool success = true;

        ////    try
        ////    {
        ////        foreach (XmlNode node in element.ChildNodes)
        ////        {
        ////            XmlElement childElement = MessageDocument.CreateElement(node.Name);
        ////            childElement.InnerXml = node.InnerXml;

        ////            switch (node.Name)
        ////            {
        ////                case "Body":
        ////                    success = ProcessBody(ref game, childElement);
        ////                    break;
        ////                case "Status":
        ////                    success = ProcessStatus(ref game, childElement, element.Name);
        ////                    break;
        ////                case "Message":
        ////                    success = ProcessMessage
        ////            }
        ////        }
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        Console.WriteLine("Error while processing the body.", e);
        ////    }

        ////    return success;
        ////}

        /// <summary>
        /// Processes the body.
        /// </summary>
        /// <param name="game">The game to be created.</param>
        /// <param name="body">The body element to be processed.</param>
        /// <returns>whether the body was processed.</returns>
        protected bool ProcessBody(ref Game game, XmlElement body)
        {
            bool success = true;
             
            foreach (XmlNode node in body.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element && success)
                {
                    XmlElement child = this.MessageDocument.CreateElement(node.Name);
                    child.InnerXml = node.InnerXml;

                    switch (node.Name)
                    {
                        case "Status":
                            success = this.ProcessStatus(child, node.Name);
                            break;
                        case "Message":
                            success = this.ProcessGameMessage(child);
                            break;
                        case "Players":
                            success = this.ProcessPlayers(child);
                            break;
                        case "Area":
                            ////PlayingArea area = new PlayingArea();
                            ////success = ProcessArea(ref area, child);
                            ////gameObject.GamingArea = area;
                            break;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Processes the status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <returns>whether the Status element was processed.</returns>
        protected bool ProcessStatus(XmlElement status, string parentName)
        {
            bool success = true;

            foreach (XmlAttribute attribute in status.Attributes)
            {
                switch (attribute.Name)
                {
                    case "turn":
                        ////game.
                        break;
                }
            }

            return success;
        }

        /// <summary>
        /// Processes the game message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>whether the message was processed.</returns>
        protected bool ProcessGameMessage(XmlElement message)
        {
            bool success = true;

            foreach (XmlAttribute attribute in message.Attributes)
            {
                switch (attribute.Name)
                {
                    case "value":
                        ////game.
                        break;
                }
            }

            return success;
        }

        /// <summary>
        /// Processes the players.
        /// </summary>
        /// <param name="players">The players.</param>
        /// <returns>whether the players element was processed.</returns>
        protected bool ProcessPlayers(XmlElement players)
        {
            bool success = true;

            foreach (XmlNode node in players.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlElement child = this.MessageDocument.CreateElement(node.Name);
                    child.InnerXml = node.InnerXml;

                    switch (node.Name)
                    {
                        case "Player":
                            ////PlayerMessage player = new PlayerMessage();
                            ////success = ProcessPlayer(ref player, child);
                            ////gameObject.Players.Add(player);
                            break;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Processes the player.
        /// </summary>
        /// <param name="playerObject">The player object.</param>
        /// <param name="player">The player.</param>
        /// <returns>whether the player element was processed.</returns>
        protected bool ProcessPlayer(ref Player playerObject, XmlElement player)
        {
            bool success = true;

            foreach (XmlNode node in player.ChildNodes)
            {                
                try
                {
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        XmlElement child = this.MessageDocument.CreateElement(node.Name);
                        child.InnerXml = node.InnerXml;

                        switch (node.Name)
                        {
                            case "id":
                                ////playerObject.Id = new Guid(node.Value);
                                break;
                            case "balance":
                                playerObject.Balance = Convert.ToInt32(node.Value);
                                break;
                            case "Hand":
                                ////CardPileMessage hand = new CardPileMessage();
                                
                                ////success = ProcessHand(ref hand, child);
                                ////nextPlayer.Hand = hand;
                                break;
                            case "Area":
                                ////AreaMessage area = new AreaMessage();

                                ////success = ProcessArea(ref area, child);
                                ////playerObject.PlayerArea = area;
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error processing Player from XML.", e);
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Processes the hand.
        /// </summary>
        /// <param name="cp">The cardpile.</param>
        /// <param name="hand">The hand element.</param>
        /// <returns>whether the Hand element was processed.</returns>
        protected bool ProcessHand(ref CardPile cp, XmlElement hand)
        {
            bool success = true;

            ////foreach (XmlNode node in players.ChildNodes)
            ////{
            ////    if (node.NodeType == XmlNodeType.Element)
            ////    {
            ////        XmlElement child = MessageDocument.CreateElement(node.Name);
            ////        child.InnerXml = node.InnerXml;

            ////        switch (node.Name)
            ////        {
            ////            case "Player":
            ////                success = ProcessPlayer(ref game, child, node.Name);
            ////                break;
            ////        }
            ////    }
            ////}

            return success;
        }

        /// <summary>
        /// Processes the area.
        /// </summary>
        /// <param name="areaObject">The area object.</param>
        /// <param name="area">The area element to be processed.</param>
        /// <returns>whether the area element was processed.</returns>
        protected bool ProcessArea(ref PlayingArea areaObject, XmlElement area)
        {
            bool success = true;

            foreach (XmlNode node in area.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    XmlElement child = this.MessageDocument.CreateElement(node.Name);
                    child.InnerXml = node.InnerXml;

                    switch (node.Name)
                    {
                        case "CardCollection":
                            ////success = ProcessCardCollection(child, node.Name);
                            break;
                        case "ChipPile":
                            ////success = ProcessGameMessage(child);
                            break;                        
                    }
                }
                else if (node.NodeType == XmlNodeType.Attribute)
                {
                    XmlElement child = this.MessageDocument.CreateElement(node.Name);
                    child.InnerXml = node.InnerXml;

                    switch (node.Name)
                    {
                        case "Status":
                            ////success = ProcessStatus(ref game, child, node.Name);
                            break;
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Validates the schema.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Xml.Schema.ValidationEventArgs"/> instance containing the event data.</param>
        private void ValidateSchema(object sender, ValidationEventArgs e)
        {
        }
    }
}
