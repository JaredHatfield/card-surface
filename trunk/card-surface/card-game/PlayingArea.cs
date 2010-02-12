// <copyright file="PlayingArea.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A playing area for chips and cards.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
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
        /// Gets the cards.
        /// </summary>
        /// <value>The cards.</value>
        public ReadOnlyObservableCollection<CardPile> Cards
        {
            get { return new ReadOnlyObservableCollection<CardPile>(this.cards); }
        }
    }
}
