// <copyright file="GameActionDouble.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The double action for blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The double action for blackjack.
    /// </summary>
    internal class GameActionDouble : GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "double";
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

            // TODO: GameActionDouble - implement a player doubling in the game
            throw new NotImplementedException("GameAction not implemented.");
        }
    }
}
