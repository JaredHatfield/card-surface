// <copyright file="CardPile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A pile of cards.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A pile of cards
    /// </summary>
    public class CardPile : Pile
    {
        /// <summary>
        /// A card pile can be visually expanded to see what is in the pile.
        /// </summary>
        private bool expandable;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPile"/> class.
        /// </summary>
        internal CardPile()
        {
            this.expandable = false;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CardPile"/> is expandable.
        /// </summary>
        /// <value><c>true</c> if expandable; otherwise, <c>false</c>.</value>
        public bool Expandable
        {
            get { return this.expandable; }
        }
    }
}
