// <copyright file="ICard.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interface for a single playing card.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Interface for a single playing card.
    /// </summary>
    public interface ICard : IPhysicalObject, IEquatable<Card>
    {
        /// <summary>
        /// Gets the suit of the card.
        /// </summary>
        /// <value>The suit of the card.</value>
        Card.CardSuit Suit
        {
            get;
        }

        /// <summary>
        /// Gets the face of the card.
        /// </summary>
        /// <value>The face of the card.</value>
        Card.CardFace Face
        {
            get;
        }

        /// <summary>
        /// Gets or sets the status of the card.
        /// </summary>
        /// <value>The status of the card.</value>
        Card.CardStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Flips this card from face up to face down or face down to face up.
        /// </summary>
        /// <returns>True if card was flipped; otherwise false.</returns>
        bool Flip();
    }
}
