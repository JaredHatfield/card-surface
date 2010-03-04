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

    /// <summary>
    /// A reflection of the current game state.
    /// </summary>
    public class MessageGameState : Message
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
        public override void MessageConstructSend(Game gameState)
        {
            this.game = gameState;

            // This may change depending on how a Message is called.
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
            this.BuildBody(ref message);

            this.messageDoc.DocumentElement.AppendChild(message);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns>whether or not the message sent successfully</returns>
        public override bool SendMessage()
        {
            bool sent = false;

            /* XmlSchema schema =;
             ValidationEventHandler schemaCheck;
             ValidationEventHandler schemaCheck = new ValidationEventHandler(ValidateSchema);
             messageDoc.Schemas.Add(
            messageDoc.Validate(schemaCheck);*/

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

            this.BuildGame(ref body);

            message.AppendChild(body);
        }

        /// <summary>
        /// Builds the game state.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildGame(ref XmlElement message)
        {
            XmlElement gameState = this.messageDoc.CreateElement("Game");

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
            XmlElement status = this.messageDoc.CreateElement("Status");

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
            XmlElement value = this.messageDoc.CreateElement("Message");

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
            XmlElement players = this.messageDoc.CreateElement("Players");

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
            XmlElement player = this.messageDoc.CreateElement("Player");
            
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
            XmlElement area = this.messageDoc.CreateElement("Area");

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
            XmlElement cardCollection = this.messageDoc.CreateElement("CardCollection");

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
            XmlElement chipPile = this.messageDoc.CreateElement("ChipPile");

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
            XmlElement cardPile = this.messageDoc.CreateElement("CardPile");

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
            XmlElement card = this.messageDoc.CreateElement("Card");

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
            XmlElement chip = this.messageDoc.CreateElement("Chip");

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
            XmlElement hand = this.messageDoc.CreateElement("Hand");

            this.BuildCardPile(ref hand);

            message.AppendChild(hand);
        }

        /// <summary>
        /// Builds the commands.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildAction(ref XmlElement message)
        {
            XmlElement action = this.messageDoc.CreateElement("Action");

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
        protected override void BuildActionParam(ref XmlElement message)
        {
            XmlElement param = this.messageDoc.CreateElement("Param");

            ////game.player.command.name);
            param.SetAttribute("Name", String.Empty);

            ////game.player.command.action);
            param.SetAttribute("Value", String.Empty); 

            message.AppendChild(param);
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
