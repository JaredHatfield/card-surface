// <copyright file="BlackjackRules.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The supporting functions that help implement the rules of Blackjack.</summary>
namespace GameBlackjack
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The supporting functions that help implement the rules of Blackjack.
    /// </summary>
    [Serializable]
    internal class BlackjackRules
    {
        /// <summary>
        /// Gets the pile vale.  Automatically handels soft aces.
        /// </summary>
        /// <param name="pile">The CardPile..</param>
        /// <returns>The value of the pile.</returns>
        internal static int GetPileVale(CardPile pile)
        {
            int total = 0;
            int acecount = 0;
            for (int i = 0; i < pile.Cards.Count; i++)
            {
                ICard card = pile.Cards[i] as ICard;
                total += BlackjackRules.GetCardValue(card);
                if (card.Face == Card.CardFace.Ace)
                {
                    acecount++;
                }
            }

            // Keep converting an ace to a soft ace until we are at or below 21 or run out of aces
            while (total > 21 && acecount > 0)
            {
                total -= 10;
                acecount--;
            }

            return total;
        }

        /// <summary>
        /// Gets the card value. (Assumes ace is always 11).
        /// </summary>
        /// <param name="card">The card to check.</param>
        /// <returns>The value of the card.</returns>
        private static int GetCardValue(ICard card)
        {
            switch (card.Face)
            {
                case Card.CardFace.Ace:
                    return 11;
                case Card.CardFace.Two:
                    return 2;
                case Card.CardFace.Three:
                    return 3;
                case Card.CardFace.Four:
                    return 4;
                case Card.CardFace.Five:
                    return 5;
                case Card.CardFace.Six:
                    return 6;
                case Card.CardFace.Seven:
                    return 7;
                case Card.CardFace.Eight:
                    return 8;
                case Card.CardFace.Nine:
                    return 9;
                case Card.CardFace.Ten:
                    return 10;
                case Card.CardFace.Jack:
                    return 10;
                case Card.CardFace.Queen:
                    return 10;
                case Card.CardFace.King:
                    return 10;
            }

            return 0;
        }
    }
}
