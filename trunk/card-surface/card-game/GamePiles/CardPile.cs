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
    public class CardPile : Pile, IComparable, IEquatable<CardPile>
    {
        /// <summary>
        /// A card pile can be visually expanded to see what is in the pile.
        /// </summary>
        private bool expandable;

        /// <summary>
        /// A flag indicating that this card pile should not be removed, even if it is empty.
        /// </summary>
        private bool persistent;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPile"/> class.
        /// </summary>
        internal CardPile()
            : base()
        {
            this.expandable = false;
            this.persistent = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPile"/> class.
        /// </summary>
        /// <param name="expandable">if set to <c>true</c> [expandable].</param>
        /// <param name="persistent">if set to <c>true</c> [persistent].</param>
        internal CardPile(bool expandable, bool persistent)
            : base()
        {
            this.expandable = expandable;
            this.persistent = persistent;
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
        /// Gets a value indicating whether this <see cref="CardPile"/> is persistent.
        /// </summary>
        /// <value><c>true</c> if persistent; otherwise, <c>false</c>.</value>
        public bool Persistent
        {
            get { return this.persistent; }
        }

        /// <summary>
        /// Gets the cards that are in the pile.
        /// </summary>
        /// <value>The cards in the pile.</value>
        public ReadOnlyObservableCollection<IPhysicalObject> Cards
        {
            get { return new ReadOnlyObservableCollection<IPhysicalObject>(this.Items); }
        }

        /// <summary>
        /// Draws the card on the top of the pile and removes it.
        /// </summary>
        /// <returns>The card on the top of the pile</returns>
        public ICard DrawCard()
        {
            if (this.NumberOfItems > 0)
            {
                int i = this.NumberOfItems - 1;
                ICard card = this.Items[i] as ICard;
                this.Items.RemoveAt(i);
                return card;
            }
            else
            {
                return null;
            }
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

            throw new ArgumentException("object is not a CardPile");
        }

        /// <summary>
        /// Equalses the specified card pile.
        /// </summary>
        /// <param name="cardPile">The card pile.</param>
        /// <returns>True if the piles have the same cards in the same order.</returns>
        public bool Equals(CardPile cardPile)
        {
            if (this.CompareTo(cardPile) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return base.Equals(obj);
            }
            else if (obj is CardPile)
            {
                return this.Equals(obj as CardPile);
            }
            else
            {
                throw new InvalidCastException("The 'obj' argument is not a CardPile object.");
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Empties the pile of cards into another pile of cards.
        /// </summary>
        /// <param name="destination">The destination pile of cards.</param>
        internal void EmptyCardPileTo(CardPile destination)
        {
            // Lets make sure we are not doing this to the same pile and avoid a nasty infinite loop.
            if (!this.Id.Equals(destination.Id))
            {
                ICard card = this.DrawCard();
                while (card != null)
                {
                    destination.AddItem(card);
                }
            }
        }
    }
}
