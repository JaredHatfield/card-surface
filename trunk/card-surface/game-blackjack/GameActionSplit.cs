// <copyright file="GameActionSplit.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The split action for blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The split action for blackjack.
    /// </summary>
    [Serializable]
    internal class GameActionSplit : GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "Split";
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
            this.PlayerCanExecuteAction(game.GetPlayer(player));
            Blackjack blackjack = (Blackjack)game;

            // TODO: GameActionSplit - implement a player spliting in the game
            throw new NotImplementedException("GameAction not implemented.");
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
            // TODO: GameActionSplit - is executable
            return false;
        }
    }
}
