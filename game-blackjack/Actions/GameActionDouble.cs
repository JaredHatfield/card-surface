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

            // Right now we only allow the player to double if they have not split!
            // TODO: Allow the player to double after they have split...

            // Double the Player's bet
            int targetBet = p.PlayerArea.Chips[0].Amount * 2;
            while (p.PlayerArea.Chips[0].Amount < targetBet)
            {
                int needed = targetBet - p.PlayerArea.Chips[0].Amount;
                if (needed >= 100 && p.BankPile.ChipExists(100))
                {
                    game.MoveAction(p.BankPile.GetChip(100), p.PlayerArea.Chips[0].Id);
                }
                else if (needed >= 25 && p.BankPile.ChipExists(25))
                {
                    game.MoveAction(p.BankPile.GetChip(25), p.PlayerArea.Chips[0].Id);
                }
                else if (needed >= 10 && p.BankPile.ChipExists(10))
                {
                    game.MoveAction(p.BankPile.GetChip(10), p.PlayerArea.Chips[0].Id);
                }
                else if (needed >= 5 && p.BankPile.ChipExists(5))
                {
                    game.MoveAction(p.BankPile.GetChip(5), p.PlayerArea.Chips[0].Id);
                }
                else if (needed >= 1 && p.BankPile.ChipExists(1))
                {
                    game.MoveAction(p.BankPile.GetChip(1), p.PlayerArea.Chips[0].Id);
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
            Blackjack blackjack = game as Blackjack;
            PlayerState playerState = blackjack.GetPlayerState(player);

            // It is the players turn, they have cards, and they are not finished
            if (player.IsTurn && playerState.IsDealt && !playerState.IsFinished)
            {
                if (playerState.HasSplit)
                {
                    // For now, if the player has split we do not allow them to double
                    // TODO: Allow a player to double after they split
                    return false;
                }
                else
                {
                    // The player has not split so things are easy
                    if (!playerState.HasHandOneStand)
                    {
                        // Make sure the player can take a hit and only has two cards and sufficient funds
                        GameActionHit hit = new GameActionHit();
                        if (hit.IsExecutableByPlayer(game, player) && player.Hand.Cards.Count == 2 && player.PlayerArea.Chips[0].Amount <= player.Balance + player.BankPile.Amount)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
