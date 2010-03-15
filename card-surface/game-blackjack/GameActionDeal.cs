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
            Blackjack blackjack = (Blackjack)game;

            // Put all of the cards back
            blackjack.ClearGameBoard();

            // Shuffle the Deck
            CardPile deck = blackjack.GetPile(blackjack.DeckPile) as CardPile;
            deck.Shuffle();

            // Deal the cards to the player
            for (int i = 0; i < blackjack.Seats.Count; i++)
            {
                if (!blackjack.Seats[i].IsEmpty)
                {
                    Player p = blackjack.Seats[i].Player;
                    ICard card1 = deck.DrawCard();
                    ICard card2 = deck.DrawCard();
                    card2.Status = Card.CardStatus.FaceUp;
                    p.Hand.AddItem(card1);
                    p.Hand.AddItem(card2);
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
            // TODO: GameActionDeal - is executable
            return false;
        }
    }
}
