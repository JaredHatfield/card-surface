// <copyright file="CardFactory.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Class that is used to create new Cards.</summary>
namespace CardGame.GameFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>s
    /// Class that is used to create new Cards.
    /// </summary>
    public class CardFactory
    {
        /// <summary>
        /// The singleton instance of the CardFactory.
        /// </summary>
        private static CardFactory instance;

        /// <summary>
        /// Prevents a default instance of the CardFactory class from being created.
        /// </summary>
        private CardFactory()
        {
            // TODO: Implement Me!
        }

        /// <summary>
        /// Returns an instance of the CardFactory.
        /// </summary>
        /// <returns>The singleton instance of CardFactory.</returns>
        public static CardFactory Instance()
        {
            if (CardFactory.instance == null)
            {
                CardFactory.instance = new CardFactory();
            }

            return CardFactory.instance;
        }

        /// <summary>
        /// Creates a new ICard.
        /// </summary>
        /// <param name="id">The card's id.</param>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        /// <returns>An ICard with a a specified Guid.</returns>
        protected internal virtual ICard MakeCard(
            Guid id,
            Card.CardSuit suit,
            Card.CardFace face,
            Card.CardStatus status)
        {
            return new Card(id, suit, face, status);
        }

        /// <summary>
        /// Creates a new ICard.
        /// </summary>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        /// <returns>An ICard with a new Guid.</returns>
        protected internal virtual ICard MakeCard(
            Card.CardSuit suit, 
            Card.CardFace face, 
            Card.CardStatus status)
        {
            return new Card(suit, face, status);
        }
    }
}
