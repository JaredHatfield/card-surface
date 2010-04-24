// <copyright file="GameActionNext.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Advances the game state.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// Advances the game state.
    /// </summary>
    internal class GameActionNext : GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "Next";
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

            if (blackjack.State.Current == GameState.State.Dealer)
            {
                GameActionDealerHit g = new GameActionDealerHit();
                g.Action(game, player);
                blackjack.State.Advance();
                return true;
            }
            else if (blackjack.State.Current == GameState.State.Award)
            {
                GameActionAwardChips g = new GameActionAwardChips();
                g.Action(game, player);
                blackjack.State.Advance();
                return true;
            }
            else if (blackjack.State.Current == GameState.State.Clear)
            {
                GameActionClearTable g = new GameActionClearTable();
                g.Action(game, player);
                blackjack.State.Advance();
                return true;
            }
            else
            {
                return false;
            }
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
            if (blackjack.State.Current == GameState.State.Dealer)
            {
                return true;
            }
            else if (blackjack.State.Current == GameState.State.Award)
            {
                return true;
            }
            else if (blackjack.State.Current == GameState.State.Clear)
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
