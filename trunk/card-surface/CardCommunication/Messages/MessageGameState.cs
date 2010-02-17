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
            this.BuildHeader();
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns>whether or not the message sent successfully</returns>
        public override bool SendMessage()
        {
            bool sent = false;

            return sent;
        }

        /// <summary>
        /// Builds the header.
        /// </summary>
        protected override void BuildHeader()
        {
            XmlElement header = this.messageDoc.CreateElement("Header");
            DateTime time;

            ////time.ToString);
            header.SetAttribute("TimeStamp", String.Empty);
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
            
            this.BuildArea(ref player);
            ////game.player.id);
            player.SetAttribute("ID", String.Empty);
            ////game.player.balance);
            player.SetAttribute("Balance", String.Empty); 

            this.BuildHand(ref player);
            this.BuildCommands(ref player);

            message.AppendChild(player);
        }

        /// <summary>
        /// Builds the area.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildArea(ref XmlElement message)
        {
            XmlElement area = this.messageDoc.CreateElement("Area");

            this.BuildGraphicCardPile(ref area);
            this.BuildChipPile(ref area);

            message.AppendChild(area);
        }

        /// <summary>
        /// Builds the card pile.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildGraphicCardPile(ref XmlElement message)
        {
            ////foreach (Card card in game.player.cards)
            ////{
                XmlElement cardGraphicPile = this.messageDoc.CreateElement("CardGraphicPile");

                this.BuildCard(ref cardGraphicPile);

                ////game.player.area.cardgpile.id);
                cardGraphicPile.SetAttribute("ID", String.Empty);
                ////game.player.area.cardgpile.open);
                cardGraphicPile.SetAttribute("Open", String.Empty);
                ////game.player.area.cardgpile.expandable);
                cardGraphicPile.SetAttribute("Expandable", String.Empty); 

                message.AppendChild(cardGraphicPile);
            ////}
        }

        /// <summary>
        /// Builds the chip pile.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildChipPile(ref XmlElement message)
        {
            ////foreach (Chip chip in game.player.chips)
            ////{
                XmlElement chipPile = this.messageDoc.CreateElement("ChipPile");

                this.BuildChip(ref chipPile);

                ////game.player.area.chippile.open);
                chipPile.SetAttribute("Open", String.Empty); 
                
                message.AppendChild(chipPile);
            ////}
        }

        /// <summary>
        /// Builds the card.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildCard(ref XmlElement message)
        {
            XmlElement card = this.messageDoc.CreateElement("Card");

            ////game.player.area.cardgpile.card.id);
            card.SetAttribute("ID", String.Empty);
            ////game.player.area.cardgpile.card.status);
            card.SetAttribute("Status", String.Empty);
            ////game.player.area.cardgpile.card.moveable);
            card.SetAttribute("Moveable", String.Empty); 

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
            chip.SetAttribute("GUID", String.Empty);
            ////game.player.area.chippile.chip.value);
            chip.SetAttribute("Value", String.Empty);
            ////game.player.area.chippile.chip.color);
            chip.SetAttribute("Color", String.Empty);
            ////game.player.area.chippile.chip.moveable);
            chip.SetAttribute("Moveable", String.Empty); 

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
        /// Builds the card pile.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildCardPile(ref XmlElement message)
        {
            XmlElement cardPile = this.messageDoc.CreateElement("CardPile");

            ////foreach (Card card in game.player.hand.cards)
            ////{
                this.BuildCard(ref cardPile);
            ////}

            ////game.player.hand.id);
            cardPile.SetAttribute("ID", String.Empty);

            ////game.player.hand.playable);
            cardPile.SetAttribute("Playable", String.Empty);

            ////game.player.hand.style);
            cardPile.SetAttribute("Style", String.Empty);

            message.AppendChild(cardPile);
        }

        /// <summary>
        /// Builds the commands.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void BuildCommands(ref XmlElement message)
        {
            XmlElement commands = this.messageDoc.CreateElement("Commands");

            ////foreach (Command com in game.player.commands)
            ////{
                this.BuildCommand(ref commands);
            ////}

            message.AppendChild(commands);
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void BuildCommand(ref XmlElement message)
        {
            XmlElement command = this.messageDoc.CreateElement("Command");

            ////game.player.command.name);
            command.SetAttribute("Name", String.Empty);

            ////game.player.command.action);
            command.SetAttribute("Action", String.Empty); 

            message.AppendChild(command);
        }
    }
}
