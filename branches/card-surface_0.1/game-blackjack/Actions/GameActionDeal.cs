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

            // Deal the cards to the house
            blackjack.House.Open = true;
            blackjack.MoveAction(deck.TopItem.Id, blackjack.House.Id);
            blackjack.MoveAction(deck.TopItem.Id, blackjack.House.Id);
            (blackjack.House.Cards[0] as ICard).Status = Card.CardStatus.FaceUp;
            (blackjack.House.Cards[1] as ICard).Status = Card.CardStatus.FaceDown;

            // Deal the cards to the player
            for (int i = 0; i < blackjack.Seats.Count; i++)
            {
                if (!blackjack.Seats[i].IsEmpty)
                {
                    Player p = blackjack.Seats[i].Player;
                    PlayerState playerState = blackjack.GetPlayerState(p);
                    playerState.Reset();

                    // Only deal cards if the player has met the minimum bet
                    if (p.PlayerArea.Chips[0].Amount >= blackjack.MinimumBet)
                    {
                        // Deal the player two cards, the second being face up
                        p.Hand.Open = true;
                        blackjack.MoveAction(deck.TopItem.Id, p.Hand.Id);
                        blackjack.MoveAction(deck.TopItem.Id, p.Hand.Id);
                        (p.Hand.Cards[0] as ICard).Status = Card.CardStatus.FaceDown;
                        (p.Hand.Cards[1] as ICard).Status = Card.CardStatus.FaceUp;
                        
                        // Indicate that this player has been dealt cards
                        playerState.Deal();

                        // Test to see if the player has 21 and stand if they do have 21
                        if (BlackjackRules.GetPileVale(p.Hand) == 21)
                        {
                            playerState.StandHandOne();
                        }
                    }
                }
            }

            // Reset the turn pointer
            blackjack.ResetPlayerTurn();

            // Move to the next game state
            blackjack.State.Advance();

            // We will advance the state again if we are not in the hand after we dealt, this would be the case if all player got 21.
            if (!blackjack.InHand)
            {
                blackjack.State.Advance();
            }

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
                if (blackjack.State.Current == GameState.State.NotInGame)
                {
                    return true;
                }

                return false;
            }
            else
            {
                // The minimum bet has not been placed
                return false;
            }
        }
    }
}
