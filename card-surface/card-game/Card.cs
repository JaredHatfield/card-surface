// <copyright file="Card.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A single playing card.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A single playing card.
    /// </summary>
    public class Card : PhysicalObject
    {
        public enum CardSuit { Hearts, Clubs, Spades, Diamonds };
        public enum CardFace { Ace, Two, Three, Four, Five, Siz, Seven, Eight, Nine, Ten, Jack, Queen, King };
        public enum CardStatus { FaceUp, FaceDown, Disabled, Hidden };

        /// <summary>
        /// 
        /// </summary>
        private CardSuit suit;

        /// <summary>
        /// 
        /// </summary>
        private CardFace face;

        /// <summary>
        /// 
        /// </summary>
        private CardStatus status;

        /// <summary>
        /// Gets or sets the suit.
        /// </summary>
        /// <value>The suit.</value>
        public CardSuit Suit
        {
            get { return this.suit; }
            set { this.suit = value; }
        }

        /// <summary>
        /// Gets or sets the face.
        /// </summary>
        /// <value>The face.</value>
        public CardFace Face
        {
            get { return this.face; }
            set { this.face = value; }
        }

        public CardStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

    }
}
