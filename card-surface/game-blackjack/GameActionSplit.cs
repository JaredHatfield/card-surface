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

            // Get the player and add a card to their hand
            Player p = blackjack.GetPlayer(player);
            PlayerState playerState = blackjack.GetPlayerState(p); // Will this throw an exception if player is not found?

            // Place the bet on the table
            int targetBet = p.PlayerArea.Chips[0].Amount;
            while (p.PlayerArea.Chips[1].Amount < targetBet)
            {
                int needed = targetBet - p.PlayerArea.Chips[1].Amount;
                if (needed >= 100)
                {
                    game.MoveAction(p.BankPile.GetChip(100), p.PlayerArea.Chips[1].Id);
                }
                else if (needed >= 25)
                {
                    game.MoveAction(p.BankPile.GetChip(25), p.PlayerArea.Chips[1].Id);
                }
                else if (needed >= 10)
                {
                    game.MoveAction(p.BankPile.GetChip(10), p.PlayerArea.Chips[1].Id);
                }
                else if (needed >= 5)
                {
                    game.MoveAction(p.BankPile.GetChip(5), p.PlayerArea.Chips[1].Id);
                }
                else if (needed >= 1)
                {
                    game.MoveAction(p.BankPile.GetChip(1), p.PlayerArea.Chips[1].Id);
                }
            }

            // Move the first card to Card0
            p.PlayerArea.Cards[0].Open = true;
            (p.Hand.TopItem as ICard).Status = Card.CardStatus.FaceUp;
            blackjack.MoveAction(p.Hand.TopItem.Id, p.PlayerArea.Cards[0].Id);

            // Move the second card to Card1
            p.PlayerArea.Cards[1].Open = true;
            (p.Hand.TopItem as ICard).Status = Card.CardStatus.FaceUp;
            blackjack.MoveAction(p.Hand.TopItem.Id, p.PlayerArea.Cards[1].Id);

            // Indicate that we performed the split action
            playerState.Split();

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
            Blackjack blackjack = (Blackjack)game;
            PlayerState playerState = blackjack.GetPlayerState(player);

            // It is the players turn, they have cards, and they are not finished
            if (player.IsTurn && playerState.IsDealt && !playerState.IsFinished)
            {
                if (playerState.HasSplit)
                {
                    // The player has already split
                    return false;
                }
                else
                {
                    // The player has not split so lets perform a few checks
                    if (!playerState.HasHandOneStand && player.Hand.Cards.Count == 2 && player.PlayerArea.Chips[0].Amount <= player.Balance)
                    {
                        ICard card0 = player.Hand.Cards[0] as ICard;
                        ICard card1 = player.Hand.Cards[1] as ICard;
                        if (card0.Face.Equals(card1.Face))
                        {
                            // You are only allowed to split if the cards are the same.
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
