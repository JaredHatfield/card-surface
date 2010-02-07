// <copyright file="PlayingArea.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary></summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class PlayingArea
    {
        /// <summary>
        /// 
        /// </summary>
        private List<ChipPile> chips;

        /// <summary>
        /// 
        /// </summary>
        private List<CardPile> cards;

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
