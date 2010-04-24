// <copyright file="SurfaceCardFactory.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Implements the SurfaceCardFactory.</summary>
namespace CardTable.GameFactory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;
    using CardGame.GameException;
    using CardGame.GameFactory;
    using CardTable.GameObjects;

    /// <summary>
    /// The factory for creating surface cards.
    /// </summary>
    internal class SurfaceCardFactory : CardFactory
    {
        /// <summary>
        /// The singleton instance of the SurfaceCardFactory.
        /// </summary>
        private static SurfaceCardFactory instance;

        /// <summary>
        /// Returns an instance of the SurfaceCardFactory.
        /// </summary>
        /// <returns>The singleton instance of SurfaceCardFactory.</returns>
        public static new SurfaceCardFactory Instance()
        {
            if (SurfaceCardFactory.instance == null)
            {
                SurfaceCardFactory.instance = new SurfaceCardFactory();
            }

            return SurfaceCardFactory.instance;
        }

        /// <summary>
        /// Creates a new ICard.
        /// </summary>
        /// <param name="id">The card's id.</param>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        /// <returns>An ICard with a a specified Guid.</returns>
        protected override ICard MakeCard(
            Guid id,
            Card.CardSuit suit,
            Card.CardFace face,
            Card.CardStatus status)
        {
            return new SurfaceCard(id, suit, face, status);
        }

        /// <summary>
        /// Creates a new ICard.
        /// </summary>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        /// <returns>An ICard with a new Guid.</returns>
        protected override ICard MakeCard(
            Card.CardSuit suit, 
            Card.CardFace face, 
            Card.CardStatus status)
        {
            return new SurfaceCard(suit, face, status);
        }
    }
}
