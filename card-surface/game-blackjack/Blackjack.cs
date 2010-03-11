﻿// <copyright file="Blackjack.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The game of Blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The game of Blackjack.
    /// </summary>
    public class Blackjack : Game
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Blackjack"/> class.
        /// </summary>
        public Blackjack() : base()
        {
            // Subscribe all of the possible game actions
            this.SubscribeAction(new GameActionHit());
            this.SubscribeAction(new GameActionStand());
            this.SubscribeAction(new GameActionSplit());
            this.SubscribeAction(new GameActionDeal());
            this.SubscribeAction(new GameActionDouble());

            // Add a deck of cards to the game
            CardPile destinationDeck = (CardPile)this.GetPile(this.DeckPile);
            destinationDeck.Open = true;
            CardPile sourceDeck = Deck.StandardDeck();
            this.EmptySpecifiedCardPileTo(sourceDeck, destinationDeck);
        }

        /// <summary>
        /// Gets the pile containing a physical object with the specified id.
        /// </summary>
        /// <param name="physicalObjectId">The physical object id.</param>
        /// <returns>
        /// The instance of the pile containing the specified physical object; otherwise null.
        /// </returns>
        internal new Pile GetPileContaining(Guid physicalObjectId)
        {
            return base.GetPileContaining(physicalObjectId);
        }

        /// <summary>
        /// Clears the game board.
        /// </summary>
        internal new void ClearGameBoard()
        {
            base.ClearGameBoard();
        }

        /// <summary>
        /// Tests if a move of a PhysicalObject to a specified Pile is valid for the specific game.
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <param name="destinationPile">The destination pile.</param>
        /// <returns>
        /// True if the move if valid; otherwise false.
        /// </returns>
        protected override bool MoveTest(Guid physicalObject, Guid destinationPile)
        {
            return base.MoveTest(physicalObject, destinationPile);
        }
    }
}
