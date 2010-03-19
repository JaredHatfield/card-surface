// <copyright file="GameActionStand.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The stand action for blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The stand action for blackjack.
    /// </summary>
    internal class GameActionStand : GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "Stand";
            }
        }

        /// <summary>
        /// Perform the action on the specified game.
        /// </summary>
        /// <param name="game">The game to modify.</param>
        /// <param name="player">The player that triggered this action.</param>
        /// <returns>True if the action was successful; otherwise false.</returns>
        public override bool Action(Game game, string player)
        {
            Blackjack blackjack = (Blackjack)game;

            // Indicate that this player has finished their turn and move on to the next player.
            int pid = blackjack.GetPlayerIndex(player);
            blackjack.HandFinished[pid] = 1;
            blackjack.MoveToNextPlayersTurn();

            return true;
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
            Blackjack blackjack = game as Blackjack;
            int pid = blackjack.GetPlayerIndex(player);

            if (player.IsTurn &&
                BlackjackRules.GetPileVale(player.Hand) < 21
                && blackjack.HandFinished[pid] == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
