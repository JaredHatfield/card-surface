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

            // TODO: GameActionDeal - implement the dealing game action

            // Put all of the cards back
            blackjack.ClearGameBoard();
        }

        /// <summary>
        /// Determines whether [is executable by player] [the specified game].
        /// </summary>
        /// <param name="game">The game to check.</param>
        /// <param name="player">The player to modify.</param>
        /// <returns>
        /// <c>true</c> if [is executable by player] [the specified game]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsExecutableByPlayer(Game game, Player player)
        {
            // TODO: GameActionDeal - is executable
            throw new NotImplementedException();
        }
    }
}
