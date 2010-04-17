// <copyright file="GameUpdater.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Updates an instance of a game based on a GameMessage.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
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
            // 1) Update the components unique to the Game (id, GamingArea, DeckId)
            this.game.Update(gameMessage);

            // 2) Update all of the components unique to a seat including all of the sub objects.
            for (int i = 0; i < this.game.Seats.Count; i++)
            {
                Seat.SeatLocation loc = this.game.Seats[i].Location;
                this.game.Seats[i].Update(gameMessage.GetSeat(loc));
            }

            // 3) Add any misssing physical objects to the appropriate pile and make sure physical objects are in the correct pile
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
                if (!this.game.Seats[i].IsEmpty && !gameMessage.Seats[i].IsEmpty)
                {
                    Player p = gameMessage.Seats[i].Player;
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

            // 4) Remove all of the physical objects that no longer exist
            for (int i = 0; i < this.game.GamingArea.Cards.Count; i++)
            {
                this.PurgeCardPileContents(this.game.GamingArea.Cards[i], gameMessage);
            }

            for (int i = 0; i < this.game.GamingArea.Chips.Count; i++)
            {
                this.PurgeChipPileContents(this.game.GamingArea.Chips[i], gameMessage);
            }

            for (int i = 0; i < this.game.Seats.Count; i++)
            {
                if (!this.game.Seats[i].IsEmpty)
                {
                    Player p = this.game.Seats[i].Player;
                    this.PurgeCardPileContents(p.Hand, gameMessage);
                    this.PurgeChipPileContents(p.BankPile, gameMessage);

                    for (int j = 0; j < p.PlayerArea.Cards.Count; j++)
                    {
                        this.PurgeCardPileContents(p.PlayerArea.Cards[j], gameMessage);
                    }

                    for (int j = 0; j < p.PlayerArea.Chips.Count; j++)
                    {
                        this.PurgeChipPileContents(p.PlayerArea.Chips[j], gameMessage);
                    }
                }
            }

            // 5) Remove all of the piles that no longer exist
            this.game.GamingArea.CleanupPiles(gameMessage.GamingArea);

            for (int i = 0; i < this.game.Seats.Count; i++)
            {
                if (!this.game.Seats[i].IsEmpty && !gameMessage.Seats[i].IsEmpty)
                {
                    this.game.Seats[i].Player.PlayerArea.CleanupPiles(gameMessage.Seats[i].Player.PlayerArea);
                }
            }

            // 6) Remove players that are no longer playing
            for (int i = 0; i < this.game.Seats.Count; i++)
            {
                if (this.game.Seats[i].Player != null && gameMessage.Seats[i].Player == null)
                {
                    this.game.Seats[i].PlayerLeft();
                }
            }

            // TODO: 7) Make sure the piles are in the same order
            // NOTE: For blackjack, this is not a high priority feature, but for other card games this is very important.

            // TODO: 8) Make sure all of the physical objects are in the same order
            // NOTE: For blackjack, this is not a high priority feature, but for other card games this is very important.
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
                    // Attempt to locate the card (this will throw the CardGamePhysicalObjectNotFoundException if it doesn't exist)
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
                        this.game.MovePhysicalObject(existing.Id, pile.Id);
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
                    // Attempt to locate the card (this will throw the CardGamePhysicalObjectNotFoundException if it doesn't exist)
                    IChip existing = this.game.GetPhysicalObject(chip.Id) as IChip;

                    // Now move it to the correct pile!
                    Guid currentPileId = this.game.GetPileContaining(existing.Id).Id;
                    if (!pile.Id.Equals(currentPileId))
                    {
                        this.game.MovePhysicalObject(existing.Id, pile.Id);
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

        /// <summary>
        /// Purges the pile contents that no longer belong.
        /// </summary>
        /// <param name="pile">The pile to clean.</param>
        /// <param name="gameMessage">The game message.</param>
        private void PurgeCardPileContents(CardPile pile, Game gameMessage)
        {
            Collection<Guid> deleteMe = new Collection<Guid>();
            for (int i = 0; i < pile.Cards.Count; i++)
            {
                IPhysicalObject physicalObject = pile.Cards[i];
                try
                {
                    // Attempt to locate the pile, this will throw a CardGamePhysicalObjectNotFoundException
                    gameMessage.GetPhysicalObject(physicalObject.Id);
                }
                catch (CardGamePhysicalObjectNotFoundException)
                {
                    // Remove the pile because we know it no longer exists.
                    deleteMe.Add(physicalObject.Id);
                }
            }

            // Now delete all of those objects
            for (int i = 0; i < deleteMe.Count; i++)
            {
                // We do this in a saparate loop because it is hard to modify the collection you are looping through
                pile.RemoveItem(deleteMe[i]);
            }
        }

        /// <summary>
        /// Purges the chip pile contents that no longer belong.
        /// </summary>
        /// <param name="pile">The pile to clean.</param>
        /// <param name="gameMessage">The game message.</param>
        private void PurgeChipPileContents(ChipPile pile, Game gameMessage)
        {
            Collection<Guid> deleteMe = new Collection<Guid>();
            for (int i = 0; i < pile.Chips.Count; i++)
            {
                IPhysicalObject physicalObject = pile.Chips[i];
                try
                {
                    // Attempt to locate the pile, this will throw a CardGamePhysicalObjectNotFoundException
                    gameMessage.GetPhysicalObject(physicalObject.Id);
                }
                catch (CardGamePhysicalObjectNotFoundException)
                {
                    // Remove the pile because we know it no longer exists.
                    deleteMe.Add(physicalObject.Id);
                }
            }

            // Now delete all of those objects
            for (int i = 0; i < deleteMe.Count; i++)
            {
                // We do this in a saparate loop because it is hard to modify the collection you are looping through
                pile.RemoveItem(deleteMe[i]);
            }
        }
    }
}
