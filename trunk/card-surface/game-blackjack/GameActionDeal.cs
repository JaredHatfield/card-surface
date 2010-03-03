// <copyright file="GameActionDeal.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Deals the hands for a blackjack game.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// Deals the hands for a blackjack game.
    /// </summary>
    internal class GameActionDeal : GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "deal";
            }
        }

        /// <summary>
        /// Perform the action on the specified game.
        /// </summary>
        /// <param name="game">The game to modify.</param>
        /// <param name="player">The player that triggered this action.</param>
        public override void Action(Game game, Guid player)
        {
            Blackjack blackjack = (Blackjack)game;

            // TODO: GameActionHit - implement the dealing game action
            throw new NotImplementedException("GameAction not implemented.");
        }
    }
}
