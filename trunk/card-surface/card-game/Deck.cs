// <copyright file="Deck.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A collection of cards that the game uses.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
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

            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.King, Card.CardStatus.FaceDown, CardGame.Properties.Resources.KingHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Queen, Card.CardStatus.FaceDown, CardGame.Properties.Resources.QueenHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Jack, Card.CardStatus.FaceDown, CardGame.Properties.Resources.JackHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Ten, Card.CardStatus.FaceDown, CardGame.Properties.Resources.TenHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Nine, Card.CardStatus.FaceDown, CardGame.Properties.Resources.NineHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Eight, Card.CardStatus.FaceDown, CardGame.Properties.Resources.EightHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Seven, Card.CardStatus.FaceDown, CardGame.Properties.Resources.SevenHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Six, Card.CardStatus.FaceDown, CardGame.Properties.Resources.SixHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Five, Card.CardStatus.FaceDown, CardGame.Properties.Resources.FiveHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Four, Card.CardStatus.FaceDown, CardGame.Properties.Resources.FourHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Three, Card.CardStatus.FaceDown, CardGame.Properties.Resources.ThreeHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Two, Card.CardStatus.FaceDown, CardGame.Properties.Resources.TwoHearts));
            cardPile.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Ace, Card.CardStatus.FaceDown, CardGame.Properties.Resources.AceHearts));

            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.King, Card.CardStatus.FaceDown, CardGame.Properties.Resources.KingClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Queen, Card.CardStatus.FaceDown, CardGame.Properties.Resources.QueenClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Jack, Card.CardStatus.FaceDown, CardGame.Properties.Resources.JackClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Ten, Card.CardStatus.FaceDown, CardGame.Properties.Resources.TenClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Nine, Card.CardStatus.FaceDown, CardGame.Properties.Resources.NineClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Eight, Card.CardStatus.FaceDown, CardGame.Properties.Resources.EightClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Seven, Card.CardStatus.FaceDown, CardGame.Properties.Resources.SevenClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Six, Card.CardStatus.FaceDown, CardGame.Properties.Resources.SixClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Five, Card.CardStatus.FaceDown, CardGame.Properties.Resources.FiveClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Four, Card.CardStatus.FaceDown, CardGame.Properties.Resources.FourClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Three, Card.CardStatus.FaceDown, CardGame.Properties.Resources.ThreeClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Two, Card.CardStatus.FaceDown, CardGame.Properties.Resources.TwoClubs));
            cardPile.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Ace, Card.CardStatus.FaceDown, CardGame.Properties.Resources.AceClubs));

            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.King, Card.CardStatus.FaceDown, CardGame.Properties.Resources.KingDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Queen, Card.CardStatus.FaceDown, CardGame.Properties.Resources.QueenDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Jack, Card.CardStatus.FaceDown, CardGame.Properties.Resources.JackDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Ten, Card.CardStatus.FaceDown, CardGame.Properties.Resources.TenDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Nine, Card.CardStatus.FaceDown, CardGame.Properties.Resources.NineDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Eight, Card.CardStatus.FaceDown, CardGame.Properties.Resources.EightDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Seven, Card.CardStatus.FaceDown, CardGame.Properties.Resources.SevenDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Six, Card.CardStatus.FaceDown, CardGame.Properties.Resources.SixDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Five, Card.CardStatus.FaceDown, CardGame.Properties.Resources.FiveDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Four, Card.CardStatus.FaceDown, CardGame.Properties.Resources.FourDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Three, Card.CardStatus.FaceDown, CardGame.Properties.Resources.ThreeDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Two, Card.CardStatus.FaceDown, CardGame.Properties.Resources.TwoDiamonds));
            cardPile.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Ace, Card.CardStatus.FaceDown, CardGame.Properties.Resources.AceDiamonds));

            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.King, Card.CardStatus.FaceDown, CardGame.Properties.Resources.KingSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Queen, Card.CardStatus.FaceDown, CardGame.Properties.Resources.QueenSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Jack, Card.CardStatus.FaceDown, CardGame.Properties.Resources.JackSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Ten, Card.CardStatus.FaceDown, CardGame.Properties.Resources.TenSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Nine, Card.CardStatus.FaceDown, CardGame.Properties.Resources.NineSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Eight, Card.CardStatus.FaceDown, CardGame.Properties.Resources.EightSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Seven, Card.CardStatus.FaceDown, CardGame.Properties.Resources.SevenSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Six, Card.CardStatus.FaceDown, CardGame.Properties.Resources.SixSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Five, Card.CardStatus.FaceDown, CardGame.Properties.Resources.FiveSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Four, Card.CardStatus.FaceDown, CardGame.Properties.Resources.FourSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Three, Card.CardStatus.FaceDown, CardGame.Properties.Resources.ThreeSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Two, Card.CardStatus.FaceDown, CardGame.Properties.Resources.TwoSpades));
            cardPile.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceDown, CardGame.Properties.Resources.AceSpades));

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
