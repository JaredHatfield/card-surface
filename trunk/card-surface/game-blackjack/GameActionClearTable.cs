// <copyright file="GameActionClearTable.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Clears the table of chips.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// Clears the table of chips.
    /// </summary>
    internal class GameActionClearTable : GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "ClearTable";
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
            // This is a special action that is executed from within the Next command.
            Blackjack blackjack = game as Blackjack;

            // Clear all of the chips off of the table
            for (int i = 0; i < blackjack.Seats.Count; i++)
            {
                if (!blackjack.Seats[i].IsEmpty)
                {
                    Player p = blackjack.Seats[i].Player;

                    // Empty chip0 into the bank
                    while (p.PlayerArea.Chips[0].NumberOfItems > 0)
                    {
                        p.BankPile.Open = true;
                        
                        blackjack.MoveAction(p.PlayerArea.Chips[0].TopItem.Id, p.BankPile.Id);
                    }

                    // Empty chip1 into the bank
                    while (p.PlayerArea.Chips[1].NumberOfItems > 0)
                    {
                        blackjack.MoveAction(p.PlayerArea.Chips[1].TopItem.Id, p.BankPile.Id);
                    }
                }
            }

            // Clear all of the cards off of the table
            blackjack.ClearGameBoard();

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
            // This is a special action that is executed from within the Next command.
            return false;
        }
    }
}
