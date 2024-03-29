﻿// <copyright file="PlayingArea.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A playing area for chips and cards.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;

    /// <summary>
    /// A playing area for chips and cards.
    /// </summary>
    [Serializable] public class PlayingArea
    {
        /// <summary>
        /// The piles of chips.
        /// </summary>
        private ObservableCollection<ChipPile> chips;

        /// <summary>
        /// The piles of cards.
        /// </summary>
        private ObservableCollection<CardPile> cards;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingArea"/> class.
        /// </summary>
        internal PlayingArea()
        {
            this.chips = new ObservableCollection<ChipPile>();
            this.cards = new ObservableCollection<CardPile>();
        }

        /// <summary>
        /// Gets the chips.
        /// </summary>
        /// <value>The chips.</value>
        public ReadOnlyObservableCollection<ChipPile> Chips
        {
            get { return new ReadOnlyObservableCollection<ChipPile>(this.chips); }
        }

        /// <summary>
        /// Gets the bindable chips.
        /// </summary>
        /// <value>The bindable chips.</value>
        public ObservableCollection<ChipPile> BindableChips
        {
            get { return this.chips; }
        }

        /// <summary>
        /// Gets the cards.
        /// </summary>
        /// <value>The cards.</value>
        public ReadOnlyObservableCollection<CardPile> Cards
        {
            get { return new ReadOnlyObservableCollection<CardPile>(this.cards); }
        }

        /// <summary>
        /// Gets the bindable cards.
        /// </summary>
        /// <value>The bindable cards.</value>
        public ObservableCollection<CardPile> BindableCards
        {
            get { return this.cards; }
        }

        /// <summary>
        /// Flips a card from face up to face down or face down to face up.
        /// </summary>
        /// <param name="id">The id of the card.</param>
        /// <returns>
        /// True if card was flipped; otherwise false.
        /// </returns>
        public bool FlipCard(Guid id)
        {
            for (int i = 0; i < this.cards.Count; i++)
            {
                if (this.cards[i].ContainsPhysicalObject(id))
                {
                    return this.cards[i].Flip(id);
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified chip id is part of playing area.
        /// </summary>
        /// <param name="chipId">The unique chip id.</param>
        /// <returns>
        ///     <c>true</c> if the specified chip id is contained within the playing area; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsChip(Guid chipId)
        {
            for (int i = 0; i < this.Chips.Count; i++)
            {
                if (this.Chips[i].ContainsPhysicalObject(chipId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified card id is part of the playing area.
        /// </summary>
        /// <param name="cardId">The unique card id.</param>
        /// <returns>
        ///     <c>true</c> if the specified card id is contained within the playing area; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsCard(Guid cardId)
        {
            for (int i = 0; i < this.Cards.Count; i++)
            {
                if (this.Cards[i].ContainsPhysicalObject(cardId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified pile is in the playing area.
        /// </summary>
        /// <param name="pileId">The unique pile id.</param>
        /// <returns>
        ///     <c>true</c> if the specified pile is in the playing area; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsPile(Guid pileId)
        {
            for (int i = 0; i < this.Cards.Count; i++)
            {
                if (this.Cards[i].Id.Equals(pileId))
                {
                    return true;
                }
            }

            for (int i = 0; i < this.Chips.Count; i++)
            {
                if (this.Chips[i].Id.Equals(pileId))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the specified playing area.
        /// This does not update the PhysicalObjects.
        /// </summary>
        /// <param name="playingArea">The playing area to reflect.</param>
        internal void Update(PlayingArea playingArea)
        {
            // Add all of the missing card piles
            ObservableCollection<CardPile> cardPileMessage = playingArea.cards;
            for (int i = 0; i < cardPileMessage.Count; i++)
            {
                if (!this.ContainsPile(cardPileMessage[i].Id))
                {
                    // The pile does not exist so add it
                    CardPile newPile = new CardPile();
                    newPile.Update(cardPileMessage[i]);
                    this.cards.Add(newPile);
                }
                else
                {
                    // Update the other piles
                    Pile oldPile = this.GetPile(cardPileMessage[i].Id);
                    oldPile.Update(cardPileMessage[i]);
                }
            }

            // Add all of the missing chip piles
            ObservableCollection<ChipPile> chipPileMessage = playingArea.chips;
            for (int i = 0; i < chipPileMessage.Count; i++)
            {
                if (!this.ContainsPile(chipPileMessage[i].Id))
                {
                    // The pile does not exist so add it
                    ChipPile newPile = new ChipPile();
                    newPile.Update(chipPileMessage[i]);
                    this.chips.Add(newPile);
                }
                else
                {
                    // Update the other piles
                    Pile oldPile = this.GetPile(chipPileMessage[i].Id);
                    oldPile.Update(chipPileMessage[i]);
                }
            }
        }

        /// <summary>
        /// Cleanups the piles that are no longer necessary.
        /// </summary>
        /// <param name="playingArea">The playing area to reflect.</param>
        internal void CleanupPiles(PlayingArea playingArea)
        {
            // The list of card piles to delete
            Collection<Guid> deleteCard = new Collection<Guid>();

            // Go through all of the piles that exist
            for (int i = 0; i < this.cards.Count; i++)
            {
                try
                {
                    playingArea.GetPile(this.cards[i].Id);
                }
                catch (CardGamePileNotFoundException)
                {
                    deleteCard.Add(this.cards[i].Id);
                }
            }

            // Now delete those piles
            for (int i = 0; i < deleteCard.Count; i++)
            {
                this.cards.Remove(this.GetPile(deleteCard[i]) as CardPile);
            }

            // The list of chip piles to delete
            Collection<Guid> deleteChip = new Collection<Guid>();

            // Go through all of the piles that exist
            for (int i = 0; i < this.chips.Count; i++)
            {
                try
                {
                    playingArea.GetPile(this.chips[i].Id);
                }
                catch (CardGamePileNotFoundException)
                {
                    deleteCard.Add(this.chips[i].Id);
                }
            }

            // Now delete those piles
            for (int i = 0; i < deleteChip.Count; i++)
            {
                this.chips.Remove(this.GetPile(deleteChip[i]) as ChipPile);
            }
        }

        /// <summary>
        /// Gets the physical object specified by a unique id.
        /// </summary>
        /// <param name="id">The unique id.</param>
        /// <returns>The IPhysicalObject specified or null if it is not in the PlayingArea.</returns>
        internal IPhysicalObject GetPhysicalObject(Guid id)
        {
            for (int i = 0; i < this.Cards.Count; i++)
            {
                if (this.Cards[i].ContainsPhysicalObject(id))
                {
                    return this.Cards[i].GetPhysicalObject(id);
                }
            }

            for (int i = 0; i < this.Chips.Count; i++)
            {
                if (this.Chips[i].ContainsPhysicalObject(id))
                {
                    return this.Chips[i].GetPhysicalObject(id);
                }
            }

            throw new CardGamePhysicalObjectNotFoundException();
        }

        /// <summary>
        /// Gets the pile with the specified id.
        /// </summary>
        /// <param name="pileId">The pile id.</param>
        /// <returns>The instance of the specified pile if it exists; otherwise null.</returns>
        internal Pile GetPile(Guid pileId)
        {
            for (int i = 0; i < this.Cards.Count; i++)
            {
                if (this.Cards[i].Id.Equals(pileId))
                {
                    return this.Cards[i];
                }
            }

            for (int i = 0; i < this.Chips.Count; i++)
            {
                if (this.Chips[i].Id.Equals(pileId))
                {
                    return this.Chips[i];
                }
            }

            throw new CardGamePileNotFoundException();
        }

        /// <summary>
        /// Gets the pile containing a physical object with the specified id.
        /// </summary>
        /// <param name="physicalObjectId">The physical object id.</param>
        /// <returns>The instance of the pile containing the specified physical object; otherwise null.</returns>
        internal Pile GetPileContaining(Guid physicalObjectId)
        {
            for (int i = 0; i < this.Cards.Count; i++)
            {
                if (this.Cards[i].ContainsPhysicalObject(physicalObjectId))
                {
                    return this.Cards[i];
                }
            }

            for (int i = 0; i < this.Chips.Count; i++)
            {
                if (this.Chips[i].ContainsPhysicalObject(physicalObjectId))
                {
                    return this.Chips[i];
                }
            }

            throw new CardGamePileNotFoundException();
        }

        /// <summary>
        /// Adds a card pile.
        /// </summary>
        /// <param name="cardPile">The card pile.</param>
        internal void AddCardPile(CardPile cardPile)
        {
            this.cards.Add(cardPile);
        }

        /// <summary>
        /// Adds a chip pile.
        /// </summary>
        /// <param name="chipPile">The chip pile.</param>
        internal void AddChipPile(ChipPile chipPile)
        {
            this.chips.Add(chipPile);
        }

        /// <summary>
        /// Empties the cards in this area to a destination CardPile.
        /// </summary>
        /// <param name="destination">The destination CardPile.</param>
        internal void EmptyCardPileTo(CardPile destination)
        {
            for (int i = 0; i < this.cards.Count; i++)
            {
                this.cards[i].EmptyCardPileTo(destination);
            }
        }
    }
}
