// <copyright file="GameActionDeal.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Deals the hands for a blackjack game.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// Deals the hands for a blackjack game.
    /// </summary>
    [Serializable]
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
                return "Deal";
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
            Blackjack blackjack = game as Blackjack;

            // Put all of the cards back
            blackjack.ClearGameBoard();

            // Shuffle the Deck
            CardPile deck = blackjack.GetPile(blackjack.DeckPile) as CardPile;
            deck.Shuffle();

            // Deal the cards to the player
            for (int i = 0; i < blackjack.Seats.Count; i++)
            {
                if (!blackjack.Seats[i].IsEmpty &&
                    blackjack.Seats[i].Player.PlayerArea.Chips[0].Amount >= blackjack.MinimumBet)
                {
                    // Deal the player two cards, the second being face up
                    Player p = blackjack.Seats[i].Player;
                    p.Hand.Open = true;
                    blackjack.MoveAction(deck.TopItem.Id, p.Hand.Id);
                    blackjack.MoveAction(deck.TopItem.Id, p.Hand.Id);
                    (p.Hand.Cards[0] as ICard).Status = Card.CardStatus.FaceDown;
                    (p.Hand.Cards[1] as ICard).Status = Card.CardStatus.FaceUp;

                    if (BlackjackRules.GetPileVale(p.Hand) < 21)
                    {
                        blackjack.HandFinished[i] = 0;
                    }
                    else
                    {
                        blackjack.HandFinished[i] = 1;
                    }
                }
                else
                {
                    blackjack.HandFinished[i] = -1;
                }
            }

            // Reset the turn pointer
            blackjack.ResetPlayerTurn();

            // TODO: Deal the cards to the house
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
            if (blackjack.InHand)
            {
                // We are currently in a game
                return false;
            }
            else if (player.PlayerArea.Chips.Count > 0 &&
                player.PlayerArea.Chips[0].Amount > blackjack.MinimumBet)
            {
                // The minimum bet has been placed
                return true;
            }
            else
            {
                // The minimum bet has not been placed
                return false;
            }
        }
    }
}
