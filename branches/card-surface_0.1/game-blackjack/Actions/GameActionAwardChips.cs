// <copyright file="GameActionAwardChips.cs" company="University of Louisville Speed School of Engineering">
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
    using CardGame.GameFactory;

    /// <summary>
    /// Clears the table of chips.
    /// </summary>
    internal class GameActionAwardChips : GameAction
    {
        /// <summary>
        /// We need to be able to create new chips in here to add to the player's bank
        /// </summary>
        private PhysicalObjectFactory factory = PhysicalObjectFactory.Instance();

        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "AwardChips";
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

            for (int i = 0; i < game.Seats.Count; i++)
            {
                if (!game.Seats[i].IsEmpty)
                {
                    Player p = game.Seats[i].Player;
                    PlayerState playerState = blackjack.GetPlayerState(p);
                    if (playerState.IsPlaying && playerState.IsDealt && playerState.IsFinished)
                    {
                        if (playerState.HasSplit)
                        {
                            // The player has split so check both piles
                            BlackjackRules.Result handOneResult = BlackjackRules.HandOutcome(p.PlayerArea.Cards[0], blackjack.House);
                            this.ProcessBet(handOneResult, p.PlayerArea.Chips[0]);
                            BlackjackRules.Result handTwoResult = BlackjackRules.HandOutcome(p.PlayerArea.Cards[1], blackjack.House);
                            this.ProcessBet(handTwoResult, p.PlayerArea.Chips[1]);
                        }
                        else
                        {
                            // The player has not split so only check the hand
                            BlackjackRules.Result handResult = BlackjackRules.HandOutcome(p.Hand, blackjack.House);
                            this.ProcessBet(handResult, p.PlayerArea.Chips[0]);
                        }
                    }
                }
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
            // This is a special action that is executed from within the Next command.
            return false;
        }

        /// <summary>
        /// Processes the player's bet.
        /// </summary>
        /// <param name="results">The hand's results.</param>
        /// <param name="chips">The player's chips.</param>
        private void ProcessBet(BlackjackRules.Result results, ChipPile chips)
        {
            if (results == BlackjackRules.Result.Win)
            {
                this.PlayerWon(chips);
            }
            else if (results == BlackjackRules.Result.Lose)
            {
                this.PlayerLost(chips);
            }
            else if (results == BlackjackRules.Result.Push)
            {
                // On a push, the player gets back their bet
            }
        }

        /// <summary>
        /// Awards the chips to the player.
        /// </summary>
        /// <param name="chip">The player's bet.</param>
        private void PlayerWon(ChipPile chip)
        {
            int targetBet = chip.Amount * 2;
            while (chip.Amount < targetBet)
            {
                int needed = targetBet - chip.Amount;
                if (needed >= 100)
                {
                    chip.AddItem(this.factory.MakeChip(100));
                }
                else if (needed >= 25)
                {
                    chip.AddItem(this.factory.MakeChip(25));
                }
                else if (needed >= 10)
                {
                    chip.AddItem(this.factory.MakeChip(10));
                }
                else if (needed >= 5)
                {
                    chip.AddItem(this.factory.MakeChip(5));
                }
                else if (needed >= 1)
                {
                    chip.AddItem(this.factory.MakeChip(1));
                }
            }
        }

        /// <summary>
        /// Remove the chips from the player.
        /// </summary>
        /// <param name="chips">The player's bet.</param>
        private void PlayerLost(ChipPile chips)
        {
            while (chips.NumberOfItems > 0)
            {
                // TODO: We also want to remove reference of this item from the factory.
                Guid id = chips.TopItem.Id;
                chips.RemoveItem(id);
            }
        }
    }
}
