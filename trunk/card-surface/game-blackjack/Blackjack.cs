// <copyright file="Blackjack.cs" company="University of Louisville Speed School of Engineering">
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
        /// Gets or sets the deck pile.
        /// </summary>
        /// <value>The deck pile.</value>
        internal new Guid DeckPile
        {
            get { return base.DeckPile; }
            set { base.DeckPile = value; }
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
        /// Gets the pile with the specified id.
        /// </summary>
        /// <param name="pileId">The pile id.</param>
        /// <returns>
        /// The instance of the specified pile if it exists; otherwise null.
        /// </returns>
        internal new Pile GetPile(Guid pileId)
        {
            return base.GetPile(pileId);
        }

        /// <summary>
        /// Clears the game board.
        /// </summary>
        internal new void ClearGameBoard()
        {
            base.ClearGameBoard();
        }

        /// <summary>
        /// Resets the player turn so that it is currently the first playesr turn.
        /// </summary>
        protected internal new void ResetPlayerTurn()
        {
            base.ResetPlayerTurn();
        }

        /// <summary>
        /// Moves the pointer that determines whoes turn it is to next player.
        /// </summary>
        protected internal new void MoveToNextPlayersTurn()
        {
            base.MoveToNextPlayersTurn();
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
