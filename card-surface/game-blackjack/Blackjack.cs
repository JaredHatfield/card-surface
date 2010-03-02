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
        protected Blackjack() : base()
        {
            this.SubscribeAction(new GameActionHit());
            this.SubscribeAction(new GameActionStand());
            this.SubscribeAction(new GameActionSplit());
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
