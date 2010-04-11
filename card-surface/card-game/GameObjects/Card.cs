// <copyright file="Card.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A single playing card.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A single playing card.
    /// </summary>
    [Serializable]
    public class Card : PhysicalObject, ICard
    {
        /// <summary>
        /// The suit of the card.
        /// </summary>
        private CardSuit suit;

        /// <summary>
        /// The face of the card.
        /// </summary>
        private CardFace face;

        /// <summary>
        /// The status of the card.
        /// </summary>
        private CardStatus status;

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="id">The card's id.</param>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        internal Card(Guid id, CardSuit suit, CardFace face, CardStatus status)
            : base(id)
        {
            this.suit = suit;
            this.face = face;
            this.status = status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        internal Card(CardSuit suit, CardFace face, CardStatus status)
            : base()
        {
            this.suit = suit;
            this.face = face;
            this.status = status;
        }

        /// <summary>
        /// Prevents a default instance of the Card class from being created.
        /// </summary>
        private Card()
            : base()
        {
            // DO NOT USE THIS CONSTRUCTOR!
        }

        /// <summary>
        /// The suits for a card.
        /// </summary>
        public enum CardSuit
        {
            /// <summary>
            /// The suit of hearts.
            /// </summary>
            Hearts,

            /// <summary>
            /// The suit of clubs.
            /// </summary>
            Clubs,

            /// <summary>
            /// The suit of spades.
            /// </summary>
            Spades,

            /// <summary>
            /// The suit of diamonds.
            /// </summary>
            Diamonds
        }

        /// <summary>
        /// The face value for a card.
        /// </summary>
        public enum CardFace
        {
            /// <summary>
            /// A card that is an ace.
            /// </summary>
            Ace,

            /// <summary>
            /// A card that is a two.
            /// </summary>
            Two,

            /// <summary>
            /// A card that is a three.
            /// </summary>
            Three,

            /// <summary>
            /// A card that is a four.
            /// </summary>
            Four,

            /// <summary>
            /// A card that is a five.
            /// </summary>
            Five,

            /// <summary>
            /// A card that is a six.
            /// </summary>
            Six,

            /// <summary>
            /// A card that is a seven.
            /// </summary>
            Seven,

            /// <summary>
            /// A card that is an eight.
            /// </summary>
            Eight,

            /// <summary>
            /// A card that is a nine.
            /// </summary>
            Nine,

            /// <summary>
            /// A card that is a ten.
            /// </summary>
            Ten,

            /// <summary>
            /// A card that is a jack.
            /// </summary>
            Jack,

            /// <summary>
            /// A card that is a queen.
            /// </summary>
            Queen,

            /// <summary>
            /// A card that is a king.
            /// </summary>
            King
        }

        /// <summary>
        /// The status of a card.
        /// </summary>
        public enum CardStatus
        {
            /// <summary>
            /// A face up card that everyone can see.
            /// </summary>
            FaceUp,

            /// <summary>
            /// A face down card that only the player can see.
            /// </summary>
            FaceDown,

            /// <summary>
            /// A disabled card that is not used in the game.
            /// </summary>
            Disabled,

            /// <summary>
            /// A hidden card that is not seen by anyone.
            /// </summary>
            Hidden
        }

        /// <summary>
        /// Gets the suit of the card.
        /// </summary>
        /// <value>The suit of the card.</value>
        public CardSuit Suit
        {
            get { return this.suit; }
        }

        /// <summary>
        /// Gets the face of the card.
        /// </summary>
        /// <value>The face of the card.</value>
        public CardFace Face
        {
            get { return this.face; }
        }

        /// <summary>
        /// Gets or sets the status of the card.
        /// </summary>
        /// <value>The status of the card.</value>
        public CardStatus Status
        {
            get
            {
                return this.status;
            }

            set
            {
                this.status = value;
                this.NotifyPropertyChanged("Status");
            }
        }

        /// <summary>
        /// Flips this card from face up to face down or face down to face up.
        /// </summary>
        /// <returns>True if card was flipped; otherwise false.</returns>
        public bool Flip()
        {
            if (this.status.Equals(CardStatus.FaceDown))
            {
                // FaceDown -> FaceUp
                this.status = CardStatus.FaceUp;
                return true;
            }
            else if (this.status.Equals(CardStatus.FaceUp))
            {
                // FaceUp -> FaceDown
                this.status = CardStatus.FaceDown;
                return true;
            }
            else
            {
                // If card is hidden or disabled, we can't flip it!
                return false;
            }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception>
        public override int CompareTo(object obj)
        {
            if (obj is Card)
            {
                Card temp = (Card)obj;
                if (this.face.CompareTo(temp.face) != 0)
                {
                    return this.face.CompareTo(temp.face);
                }
                else if (this.suit.CompareTo(temp.suit) != 0)
                {
                    return this.suit.CompareTo(temp.suit);
                }
                else
                {
                    return 0;
                }
            }

            throw new ArgumentException("object is not a Card");
        }

        /// <summary>
        /// Equals the specified card.
        /// </summary>
        /// <param name="card">The card to compare.</param>
        /// <returns>True if the cards face and suit is the same.</returns>
        public bool Equals(Card card)
        {
            if (this.CompareTo(card) == 0)
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
            else if (obj is Card)
            {
                return this.Equals(obj as Card);
            }
            else
            {
                throw new InvalidCastException("The 'obj' argument is not a Card object.");
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
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this.status == CardStatus.FaceUp)
            {
                return this.face + " of " + this.suit;
            }
            else
            {
                return "Face down.";
            }
        }
    }
}
