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
        XmlDocument messageDoc;

        /// <summary>
        /// game state.
        /// </summary>
        Game game;

        /// <summary>
        /// Messages all relevent players/tables of the specified game state.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        public void Message(Game gameState)
        {
            game = gameState;
            // This may change depending on how a Message is called.
            BuildMessage();
            SendMessage();
        }

        /// <summary>
        /// Builds the message.
        /// </summary>
        public void BuildMessage()
        {
            BuildHeader();
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <returns></returns>
        public bool SendMessage()
        {

        }

        /// <summary>
        /// Builds the header.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        private void BuildHeader()
        {
            XmlElement header = messageDoc.CreateElement("Header");

            header.SetAttribute("TimeStamp", val);
        }

        /// <summary>
        /// Builds the game state.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildGame(ref XmlElement message)
        {
            XmlElement gameState = messageDoc.CreateElement("Game");

            BuildStatus(gameState);
            BuildMessageValue(gameState);
            BuildPlayers(gameState);
            BuildArea(gameState);

            message.AppendChild(gameState);
        }

        /// <summary>
        /// Builds the status.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildStatus(ref XmlElement message)
        {
            XmlElement status = messageDoc.CreateElement("Status");

            status.SetAttribute("turn", game.turn);

            message.AppendChild(status);
        }

        /// <summary>
        /// Builds the message value.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildMessageValue(ref XmlElement message)
        {
            XmlElement value = messageDoc.CreateElement("Message");

            value.SetAttribute("Value", game.value);

            message.AppendChild(value);
        }

        /// <summary>
        /// Builds the players.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildPlayers( ref XmlElement message)
        {
            XmlElement players = messageDoc.CreateElement("Players");

            foreach (Player player in game.players)
            {
                BuildPlayer(players);
            }

            message.AppendChild(players);
        }

        /// <summary>
        /// Builds the player.
        /// </summary>
        /// <param name="player">The player.</param>
        private void BuildPlayer(ref XmlElement message)
        {
            XmlElement player = messageDoc.CreateElement("Player");
            
            BuildArea(player);
            player.SetAttribute("ID", game.player.id);
            player.SetAttribute("Balance", game.player.balance);

            BuildHand(player);
            BuildCommands(player);

            message.AppendChild(player);
        }

        /// <summary>
        /// Builds the area.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildArea(ref XmlElement message)
        {
            XmlElement area = messageDoc.CreateElement("Area");

            BuildGraphicCardPile(area);
            BuildChipPile(area);

            message.AppendChild(area);
        }

        /// <summary>
        /// Builds the card pile.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildGraphicCardPile(XmlElement message)
        {
            foreach (Card card in game.player.cards)
            {
                XmlElement cardGraphicPile = messageDoc.CreateElement("CardGraphicPile");

                BuildCard(cardGraphicPile);

                cardGraphicPile.SetAttribute("ID", game.player.area.cardgpile.id);
                cardGraphicPile.SetAttribute("Open", game.player.area.cardgpile.open);
                cardGraphicPile.SetAttribute("Expandable", game.player.area.cardgpile.expandable);

                message.AppendChild(cardGraphicPile);
            }
        }

        /// <summary>
        /// Builds the chip pile.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildChipPile(XmlElement message)
        {
            foreach (Chip chip in game.player.chips)
            {
                XmlElement chipPile = messageDoc.CreateElement("ChipPile");

                BuildChip(chipPile);

                chipPile.SetAttribute("Open", game.player.area.chippile.open);
                
                message.AppendChild(chipPile);
            }
        }

        /// <summary>
        /// Builds the card.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildCard(XmlElement message)
        {
            XmlElement card = messageDoc.CreateElement("Card");

            card.SetAttribute("ID", game.player.area.cardgpile.card.id);
            card.SetAttribute("Status", game.player.area.cardgpile.card.status);
            card.SetAttribute("Moveable", game.player.area.cardgpile.card.moveable);

            messageDoc.AppendChild(card);
        }

        /// <summary>
        /// Builds the chip.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildChip(XmlElement message)
        {
            XmlElement chip = messageDoc.CreateElement("Chip");

            chip.SetAttribute("GUID", game.player.area.chippile.chip.guid);
            chip.SetAttribute("Value", game.player.area.chippile.chip.value);
            chip.SetAttribute("Color", game.player.area.chippile.chip.color);
            chip.SetAttribute("Moveable", game.player.area.chippile.chip.moveable);

            messageDoc.AppendChild(chip);
        }

        /// <summary>
        /// Builds the hand.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildHand(XmlElement message)
        {
            XmlElement hand = messageDoc.CreateElement("Hand");

            BuildCardPile(hand);

            messageDoc.AppendChild(hand);
        }

        /// <summary>
        /// Builds the card pile.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildCardPile(XmlElement message)
        {
            XmlElement cardPile = messageDoc.CreateElement("CardPile");

            foreach (Card card in game.player.hand.cards)
            {
                BuildCard(cardPile);
            }

            cardPile.SetAttribute("ID", game.player.hand.id);
            cardPile.SetAttribute("Playable", game.player.hand.playable);
            cardPile.SetAttribute("Style", game.player.hand.style);

            message.AppendChild(cardPile);
        }

        /// <summary>
        /// Builds the commands.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildCommands(XmlElement message)
        {
            XmlElement commands = messageDoc.CreateElement("Commands");

            foreach (Command commands in game.player.commands)
            {
                BuildCommand(commands);
            }

            message.AppendChild(commands);
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="message">The message.</param>
        private void BuildCommand(XmlElement message)
        {
            XmlElement command = messageDoc.CreateElement("Command");

            command.SetAttribute("Name", game.player.command.name);
            command.SetAttribute("Action", game.player.command.action);

            message.AppendChild(command);
        }
    }
}
