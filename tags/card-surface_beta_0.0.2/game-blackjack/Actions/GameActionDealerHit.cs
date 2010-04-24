// <copyright file="GameActionDealerHit.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Have the house hit if they are required to.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardGame;
    using CardGame.GameFactory;

    /// <summary>
    /// Have the house hit if they are required to.
    /// </summary>
    internal class GameActionDealerHit : GameAction
    {
        /// <summary>
        /// Gets this actions name.
        /// </summary>
        /// <value>The actions name.</value>
        public override string Name
        {
            get
            {
                return "DealerHit";
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
            CardPile deck = blackjack.GetPile(blackjack.DeckPile) as CardPile;

            // Flip the Dealers card so it if face up
            (blackjack.House.Cards[0] as ICard).Status = Card.CardStatus.FaceUp;

            // Go through all of the players and determine what the house needs to beat
            int target = 0;
            for (int i = 0; i < blackjack.Seats.Count; i++)
            {
                if (!blackjack.Seats[i].IsEmpty)
                {
                    Player p = blackjack.Seats[i].Player;
                    PlayerState playerState = blackjack.GetPlayerState(p);

                    if (playerState.IsPlaying && playerState.IsDealt && playerState.IsFinished)
                    {
                        if (playerState.HasSplit)
                        {
                            int one = BlackjackRules.GetPileVale(p.PlayerArea.Cards[0]);
                            if (one <= 21 && one > target)
                            {
                                target = one;
                            }

                            int two = BlackjackRules.GetPileVale(p.PlayerArea.Cards[1]);
                            if (two <= 21 && two > target)
                            {
                                target = two;
                            }
                        }
                        else
                        {
                            int val = BlackjackRules.GetPileVale(p.Hand);
                            if (val <= 21 && val > target)
                            {
                                target = val;
                            }
                        }
                    }
                }
            }

            // If the house needs to take a hit and can take a hit, take a hit
            while (BlackjackRules.GetPileVale(blackjack.House) < target && BlackjackRules.GetPileVale(blackjack.House) < 17)
            {
                // Take a hit
                blackjack.House.Open = true;
                blackjack.MoveAction(deck.TopItem.Id, blackjack.House.Id);
            }

            // Make sure all of the cards in the dealers hand are faceup.
            for (int i = 0; i < blackjack.House.Cards.Count; i++)
            {
                ICard card = blackjack.House.Cards[i] as ICard;
                card.Status = Card.CardStatus.FaceUp;
            }

            return false;
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
