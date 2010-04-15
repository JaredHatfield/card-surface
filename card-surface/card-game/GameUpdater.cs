// <copyright file="GameUpdater.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Updates an instance of a game based on a GameMessage.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;
    using CardGame.GameFactory;

    /// <summary>
    /// Updates an instance of a game based on a GameMessage.
    /// </summary>
    public class GameUpdater
    {
        /// <summary>
        /// The factory for creating physical objects
        /// </summary>
        private static PhysicalObjectFactory factory = PhysicalObjectFactory.Instance();

        /// <summary>
        /// The game that will be updated.
        /// </summary>
        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameUpdater"/> class.
        /// </summary>
        /// <param name="game">The game that will be updated.</param>
        public GameUpdater(Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Updates the specified game message.
        /// </summary>
        /// <param name="gameMessage">The game message.</param>
        public void Update(Game gameMessage)
        {
            // 1) Make the game pass
            this.game.Update(gameMessage);

            // 2) Make a constructive pass on all of the seats making sure all of the objects match.
            for (int i = 0; i < this.game.Seats.Count; i++)
            {
                Seat.SeatLocation loc = this.game.Seats[i].Location;
                this.game.Seats[i].Update(gameMessage.GetSeat(loc));
            }

            // 3) Add any misssing physical objects to the appropriate pile and make sure cards are in correct state
            for (int i = 0; i < gameMessage.GamingArea.Cards.Count; i++)
            {
                this.ProcessCardPileFromMessage(gameMessage.GamingArea.Cards[i]);
            }

            for (int i = 0; i < gameMessage.GamingArea.Chips.Count; i++)
            {
                this.ProcessChipPileFromMessage(gameMessage.GamingArea.Chips[i]);
            }

            for (int i = 0; i < this.game.Seats.Count; i++)
            {
                if (!this.game.Seats[i].IsEmpty)
                {
                    Player p = this.game.Seats[i].Player;
                    this.ProcessCardPileFromMessage(p.Hand);
                    this.ProcessChipPileFromMessage(p.BankPile);

                    for (int j = 0; j < p.PlayerArea.Cards.Count; j++)
                    {
                        this.ProcessCardPileFromMessage(p.PlayerArea.Cards[j]);
                    }

                    for (int j = 0; j < p.PlayerArea.Chips.Count; j++)
                    {
                        this.ProcessChipPileFromMessage(p.PlayerArea.Chips[j]);
                    }
                }
            }

            // TODO: 4) For each physical object, find the destination pile
            //    If the object is in the same pile, done
            //    If the object is in a different pile, move it
            //    If the object is in no pile, delete it
            //    Special: Make sure all cards match their state

            // TODO: 5) Remove players that are no longer playing

            // TODO: 6) Make sure the piles are in the same order

            // TODO: 7) Make sure all of the physical objects are in the same order
        }

        /// <summary>
        /// Go through all of the cards in a pile and make sure that every object exists in the game and is in the correct state.
        /// </summary>
        /// <param name="pile">The pile to process.</param>
        private void ProcessCardPileFromMessage(CardPile pile)
        {
            for (int i = 0; i < pile.Cards.Count; i++)
            {
                ICard card = pile.Cards[i] as ICard;
                try
                {
                    // Attempt to locate the card
                    ICard existing = this.game.GetPhysicalObject(card.Id) as ICard;

                    // Make sure the card's state matches
                    if (!card.Status.Equals(existing.Status))
                    {
                        existing.Status = card.Status;
                    }

                    // Now move it to the correct pile!
                    Guid currentPileId = this.game.GetPileContaining(existing.Id).Id;
                    if (!pile.Id.Equals(currentPileId))
                    {
                        this.game.MoveAction(existing.Id, pile.Id);
                    }
                }
                catch (CardGamePhysicalObjectNotFoundException)
                {
                    // Add the missing card to the game

                    // 1) Create the missing card
                    ICard newCard = GameUpdater.factory.MakeCard(card.Id, card.Suit, card.Face, card.Status);

                    // 2) Find the pile that the card belongs in
                    Pile destination = this.game.GetPile(pile.Id);

                    // 3) Add the new card to that pile
                    destination.AddItem(newCard);
                }
            }
        }

        /// <summary>
        /// Go through all of the chips in a pile and make sure that every object exists in the game and is in the correct state.
        /// </summary>
        /// <param name="pile">The pile to process.</param>
        private void ProcessChipPileFromMessage(ChipPile pile)
        {
            for (int i = 0; i < pile.Chips.Count; i++)
            {
                IChip chip = pile.Chips[i] as IChip;
                try
                {
                    // Attempt to locate the card
                    IChip existing = this.game.GetPhysicalObject(chip.Id) as IChip;

                    // Now move it to the correct pile!
                    Guid currentPileId = this.game.GetPileContaining(existing.Id).Id;
                    if (!pile.Id.Equals(currentPileId))
                    {
                        this.game.MoveAction(existing.Id, pile.Id);
                    }
                }
                catch (CardGamePhysicalObjectNotFoundException)
                {
                    // Add the missing card to the game

                    // 1) Create the missing chip
                    IChip newChip = GameUpdater.factory.MakeChip(chip.Id, chip.Amount);
                    
                    // 2) Find the pile that the card belongs in
                    Pile destination = this.game.GetPile(pile.Id);

                    // 3) Add the new card to that pile
                    destination.AddItem(newChip);
                }
            }
        }
    }
}
