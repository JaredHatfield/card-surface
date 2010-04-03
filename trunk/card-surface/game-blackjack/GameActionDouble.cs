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
    [Serializable]
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
                return "Double";
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
            Player p = blackjack.GetPlayer(player);

            // Double the Player's bet
            int targetBet = p.PlayerArea.Chips[0].Amount * 2;
            while (p.PlayerArea.Chips[0].Amount < targetBet)
            {
                int needed = targetBet - p.PlayerArea.Chips[0].Amount;
                if (needed >= 100)
                {
                    game.MoveAction(p.BankPile.GetChip(100), p.PlayerArea.Chips[0].Id);
                }
                else if (needed >= 25)
                {
                    game.MoveAction(p.BankPile.GetChip(100), p.PlayerArea.Chips[0].Id);
                }
                else if (needed >= 10)
                {
                    game.MoveAction(p.BankPile.GetChip(100), p.PlayerArea.Chips[0].Id);
                }
                else if (needed >= 5)
                {
                    game.MoveAction(p.BankPile.GetChip(100), p.PlayerArea.Chips[0].Id);
                }
                else if (needed >= 1)
                {
                    game.MoveAction(p.BankPile.GetChip(100), p.PlayerArea.Chips[0].Id);
                }
            }

            // Have the player take a hit
            blackjack.ExecuteAction("Hit", player);

            // Have the player stand
            if (p.IsTurn)
            {
                blackjack.ExecuteAction("Stand", player);
            }

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
            // Make sure the player can take a hit
            GameActionHit hit = new GameActionHit();
            if (hit.IsExecutableByPlayer(game, player) && player.Hand.Cards.Count == 2)
            {
                return true;
            }

            return false;
        }
    }
}
