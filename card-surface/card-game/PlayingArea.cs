// <copyright file="PlayingArea.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A playing area for chips and cards.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A playing area for chips and cards.
    /// </summary>
    public class PlayingArea
    {
        /// <summary>
        /// The piles of chips.
        /// </summary>
        private List<ChipPile> chips;

        /// <summary>
        /// The piles of cards.
        /// </summary>
        private List<CardPile> cards;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayingArea"/> class.
        /// </summary>
        internal PlayingArea()
        {
            this.chips = new List<ChipPile>();
            this.cards = new List<CardPile>();
        }

        /// <summary>
        /// Gets the chips.
        /// </summary>
        /// <value>The chips.</value>
        public List<ChipPile> Chips
        {
            get { return this.chips; }
        }

        /// <summary>
        /// Gets the cards.
        /// </summary>
        /// <value>The cards.</value>
        public List<CardPile> Cards
        {
            get { return this.cards; }
        }
    }
}
