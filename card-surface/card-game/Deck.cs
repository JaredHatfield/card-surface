// <copyright file="Deck.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A collection of cards that the game uses.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A collection of cards that the game uses.
    /// The alphabet, the universal set, what we are working with!
    /// </summary>
    public class Deck
    {
        /// <summary>
        /// Create a CardPile containing a standard deck of cards.
        /// </summary>
        /// <returns>Standard deck of cards.</returns>
        public static CardPile StandardDeck()
        {
            CardPile cardPile = new CardPile();
            cardPile.Open = true;
            Card.CardFace[] faces = Deck.GetFaces();
            Card.CardSuit[] suits = Deck.GetSuits();
            for (int i = suits.Length - 1; i >= 0; i--)
            {
                for (int j = faces.Length - 1; j >= 0; j--)
                {
                    cardPile.AddItem(new Card(suits[i], faces[j], Card.CardStatus.FaceDown));
                }
            }

            return cardPile;
        }

        /// <summary>
        /// Gets the default list of suits.
        /// </summary>
        /// <returns>List of suits.</returns>
        private static Card.CardSuit[] GetSuits()
        {
            Card.CardSuit[] suits = new Card.CardSuit[4];
            suits[0] = Card.CardSuit.Spades;
            suits[1] = Card.CardSuit.Diamonds;
            suits[2] = Card.CardSuit.Clubs;
            suits[3] = Card.CardSuit.Hearts;
            return suits;
        }

        /// <summary>
        /// Gets the default list of faces.
        /// </summary>
        /// <returns>List of faces.</returns>
        private static Card.CardFace[] GetFaces()
        {
            Card.CardFace[] faces = new Card.CardFace[13];
            faces[0] = Card.CardFace.Ace;
            faces[1] = Card.CardFace.Two;
            faces[2] = Card.CardFace.Three;
            faces[3] = Card.CardFace.Four;
            faces[4] = Card.CardFace.Five;
            faces[5] = Card.CardFace.Six;
            faces[6] = Card.CardFace.Seven;
            faces[7] = Card.CardFace.Eight;
            faces[8] = Card.CardFace.Nine;
            faces[9] = Card.CardFace.Ten;
            faces[10] = Card.CardFace.Jack;
            faces[11] = Card.CardFace.Queen;
            faces[12] = Card.CardFace.King;
            return faces;
        }
    }
}
