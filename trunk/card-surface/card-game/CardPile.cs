// <copyright file="CardPile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A pile of cards.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A pile of cards
    /// </summary>
    public class CardPile : Pile, IComparable
    {
        /// <summary>
        /// A card pile can be visually expanded to see what is in the pile.
        /// </summary>
        private bool expandable;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPile"/> class.
        /// </summary>
        internal CardPile()
            : base()
        {
            this.expandable = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPile"/> class.
        /// </summary>
        /// <param name="expandable">if set to <c>true</c> [expandable].</param>
        internal CardPile(bool expandable)
            : base()
        {
            this.expandable = expandable;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CardPile"/> is expandable.
        /// </summary>
        /// <value><c>true</c> if expandable; otherwise, <c>false</c>.</value>
        public bool Expandable
        {
            get { return this.expandable; }
        }

        /// <summary>
        /// Gets the cards that are in the pile.
        /// </summary>
        /// <value>The cards in the pile.</value>
        public ReadOnlyObservableCollection<PhysicalObject> Cards
        {
            get { return new ReadOnlyObservableCollection<PhysicalObject>(this.Items); }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// The cards in the pile must be the same face and suit and in the same order in the pile.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj is CardPile)
            {
                CardPile temp = (CardPile)obj;
                if (this.NumberOfItems != temp.NumberOfItems)
                {
                    return this.NumberOfItems - temp.NumberOfItems;
                }
                else
                {
                    for (int i = 0; i < this.NumberOfItems; i++)
                    {
                        if (this.Items[i].CompareTo(temp.Items[i]) != 0)
                        {
                            return -1;
                        }
                    }

                    return 0;
                }
            }

            throw new ArgumentException("object is not a Chip");
        }
    }
}
