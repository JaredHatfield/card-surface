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
        /// Gets the image for a card
        /// </summary>
        /// <param name="cardname">The string representation of the card name.</param>
        /// <returns>The bitmap image for the card</returns>
        public static Bitmap CardImage(string cardname)
        {
            Card card = new Card();
            switch (cardname)
            {
                case "AceClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Ace, Card.CardStatus.FaceDown);
                    break;
                case "AceDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Ace, Card.CardStatus.FaceDown);
                    break;
                case "AceHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Ace, Card.CardStatus.FaceDown);
                    break;
                case "AceSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceDown);
                    break;
                case "EightClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Eight, Card.CardStatus.FaceDown);
                    break;
                case "EightDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Eight, Card.CardStatus.FaceDown);
                    break;
                case "EightHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Eight, Card.CardStatus.FaceDown);
                    break;
                case "EightSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Eight, Card.CardStatus.FaceDown);
                    break;
                case "FiveClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Five, Card.CardStatus.FaceDown);
                    break;
                case "FiveDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Five, Card.CardStatus.FaceDown);
                    break;
                case "FiveHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Five, Card.CardStatus.FaceDown);
                    break;
                case "FiveSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Five, Card.CardStatus.FaceDown);
                    break;
                case "FourClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Four, Card.CardStatus.FaceDown);
                    break;
                case "FourDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Four, Card.CardStatus.FaceDown);
                    break;
                case "FourHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Four, Card.CardStatus.FaceDown);
                    break;
                case "FourSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Four, Card.CardStatus.FaceDown);
                    break;
                case "JackClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Jack, Card.CardStatus.FaceDown);
                    break;
                case "JackDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Jack, Card.CardStatus.FaceDown);
                    break;
                case "JackHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Jack, Card.CardStatus.FaceDown);
                    break;
                case "JackSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Jack, Card.CardStatus.FaceDown);
                    break;
                case "KingClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.King, Card.CardStatus.FaceDown);
                    break;
                case "KingDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.King, Card.CardStatus.FaceDown);
                    break;
                case "KingHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.King, Card.CardStatus.FaceDown);
                    break;
                case "KingSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.King, Card.CardStatus.FaceDown);
                    break;
                case "NineClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Nine, Card.CardStatus.FaceDown);
                    break;
                case "NineDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Nine, Card.CardStatus.FaceDown);
                    break;
                case "NineHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Nine, Card.CardStatus.FaceDown);
                    break;
                case "NineSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Nine, Card.CardStatus.FaceDown);
                    break;
                case "QueenClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Queen, Card.CardStatus.FaceDown);
                    break;
                case "QueenDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Queen, Card.CardStatus.FaceDown);
                    break;
                case "QueenHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Queen, Card.CardStatus.FaceDown);
                    break;
                case "QueenSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Queen, Card.CardStatus.FaceDown);
                    break;
                case "SevenClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Seven, Card.CardStatus.FaceDown);
                    break;
                case "SevenDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Seven, Card.CardStatus.FaceDown);
                    break;
                case "SevenHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Seven, Card.CardStatus.FaceDown);
                    break;
                case "SevenSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Seven, Card.CardStatus.FaceDown);
                    break;
                case "SixClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Six, Card.CardStatus.FaceDown);
                    break;
                case "SixDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Six, Card.CardStatus.FaceDown);
                    break;
                case "SixHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Six, Card.CardStatus.FaceDown);
                    break;
                case "SixSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Six, Card.CardStatus.FaceDown);
                    break;
                case "TenClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Ten, Card.CardStatus.FaceDown);
                    break;
                case "TenDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Ten, Card.CardStatus.FaceDown);
                    break;
                case "TenHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Ten, Card.CardStatus.FaceDown);
                    break;
                case "TenSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Ten, Card.CardStatus.FaceDown);
                    break;
                case "ThreeClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Three, Card.CardStatus.FaceDown);
                    break;
                case "ThreeDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Three, Card.CardStatus.FaceDown);
                    break;
                case "ThreeHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Three, Card.CardStatus.FaceDown);
                    break;
                case "ThreeSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Three, Card.CardStatus.FaceDown);
                    break;
                case "TwoClubs":
                    card = new Card(Card.CardSuit.Clubs, Card.CardFace.Two, Card.CardStatus.FaceDown);
                    break;
                case "TwoDiamonds":
                    card = new Card(Card.CardSuit.Diamonds, Card.CardFace.Two, Card.CardStatus.FaceDown);
                    break;
                case "TwoHearts":
                    card = new Card(Card.CardSuit.Hearts, Card.CardFace.Two, Card.CardStatus.FaceDown);
                    break;
                case "TwoSpades":
                    card = new Card(Card.CardSuit.Spades, Card.CardFace.Two, Card.CardStatus.FaceDown);
                    break;
            }

            return Deck.CardImage(card.Face, card.Suit);
        }

        /// <summary>
        /// Gets the image for a specified face and suit.
        /// </summary>
        /// <param name="face">The card face.</param>
        /// <param name="suit">The card suit.</param>
        /// <returns>The image for the card.</returns>
        internal static Bitmap CardImage(Card.CardFace face, Card.CardSuit suit)
        {
            switch (suit)
            {
                case Card.CardSuit.Clubs:
                    switch (face)
                    {
                        case Card.CardFace.Ace:
                            return CardGame.Properties.Resources.AceClubs;
                        case Card.CardFace.King:
                            return CardGame.Properties.Resources.KingClubs;
                        case Card.CardFace.Queen:
                            return CardGame.Properties.Resources.QueenClubs;
                        case Card.CardFace.Jack:
                            return CardGame.Properties.Resources.JackClubs;
                        case Card.CardFace.Ten:
                            return CardGame.Properties.Resources.TenClubs;
                        case Card.CardFace.Nine:
                            return CardGame.Properties.Resources.NineClubs;
                        case Card.CardFace.Eight:
                            return CardGame.Properties.Resources.EightClubs;
                        case Card.CardFace.Seven:
                            return CardGame.Properties.Resources.SevenClubs;
                        case Card.CardFace.Six:
                            return CardGame.Properties.Resources.SixClubs;
                        case Card.CardFace.Five:
                            return CardGame.Properties.Resources.FiveClubs;
                        case Card.CardFace.Four:
                            return CardGame.Properties.Resources.FourClubs;
                        case Card.CardFace.Three:
                            return CardGame.Properties.Resources.ThreeClubs;
                        case Card.CardFace.Two:
                            return CardGame.Properties.Resources.TwoClubs;
                    }

                    break;
                case Card.CardSuit.Diamonds:
                    switch (face)
                    {
                        case Card.CardFace.Ace:
                            return CardGame.Properties.Resources.AceDiamonds;
                        case Card.CardFace.King:
                            return CardGame.Properties.Resources.KingDiamonds;
                        case Card.CardFace.Queen:
                            return CardGame.Properties.Resources.QueenDiamonds;
                        case Card.CardFace.Jack:
                            return CardGame.Properties.Resources.JackDiamonds;
                        case Card.CardFace.Ten:
                            return CardGame.Properties.Resources.TwoDiamonds;
                        case Card.CardFace.Nine:
                            return CardGame.Properties.Resources.NineDiamonds;
                        case Card.CardFace.Eight:
                            return CardGame.Properties.Resources.EightDiamonds;
                        case Card.CardFace.Seven:
                            return CardGame.Properties.Resources.SevenDiamonds;
                        case Card.CardFace.Six:
                            return CardGame.Properties.Resources.SixDiamonds;
                        case Card.CardFace.Five:
                            return CardGame.Properties.Resources.FiveDiamonds;
                        case Card.CardFace.Four:
                            return CardGame.Properties.Resources.FourDiamonds;
                        case Card.CardFace.Three:
                            return CardGame.Properties.Resources.ThreeDiamonds;
                        case Card.CardFace.Two:
                            return CardGame.Properties.Resources.TwoDiamonds;
                    }

                    break;
                case Card.CardSuit.Hearts:
                    switch (face)
                    {
                        case Card.CardFace.Ace:
                            return CardGame.Properties.Resources.AceHearts;
                        case Card.CardFace.King:
                            return CardGame.Properties.Resources.KingHearts;
                        case Card.CardFace.Queen:
                            return CardGame.Properties.Resources.QueenHearts;
                        case Card.CardFace.Jack:
                            return CardGame.Properties.Resources.JackHearts;
                        case Card.CardFace.Ten:
                            return CardGame.Properties.Resources.TenHearts;
                        case Card.CardFace.Nine:
                            return CardGame.Properties.Resources.NineHearts;
                        case Card.CardFace.Eight:
                            return CardGame.Properties.Resources.EightHearts;
                        case Card.CardFace.Seven:
                            return CardGame.Properties.Resources.SevenHearts;
                        case Card.CardFace.Six:
                            return CardGame.Properties.Resources.SixHearts;
                        case Card.CardFace.Five:
                            return CardGame.Properties.Resources.FiveHearts;
                        case Card.CardFace.Four:
                            return CardGame.Properties.Resources.FourHearts;
                        case Card.CardFace.Three:
                            return CardGame.Properties.Resources.ThreeHearts;
                        case Card.CardFace.Two:
                            return CardGame.Properties.Resources.TwoHearts;
                    }

                    break;
                case Card.CardSuit.Spades:
                    switch (face)
                    {
                        case Card.CardFace.Ace:
                            return CardGame.Properties.Resources.AceSpades;
                        case Card.CardFace.King:
                            return CardGame.Properties.Resources.KingSpades;
                        case Card.CardFace.Queen:
                            return CardGame.Properties.Resources.QueenSpades;
                        case Card.CardFace.Jack:
                            return CardGame.Properties.Resources.JackSpades;
                        case Card.CardFace.Ten:
                            return CardGame.Properties.Resources.TenSpades;
                        case Card.CardFace.Nine:
                            return CardGame.Properties.Resources.NineSpades;
                        case Card.CardFace.Eight:
                            return CardGame.Properties.Resources.EightSpades;
                        case Card.CardFace.Seven:
                            return CardGame.Properties.Resources.SevenSpades;
                        case Card.CardFace.Six:
                            return CardGame.Properties.Resources.SixSpades;
                        case Card.CardFace.Five:
                            return CardGame.Properties.Resources.FiveSpades;
                        case Card.CardFace.Four:
                            return CardGame.Properties.Resources.FourSpades;
                        case Card.CardFace.Three:
                            return CardGame.Properties.Resources.ThreeSpades;
                        case Card.CardFace.Two:
                            return CardGame.Properties.Resources.TwoSpades;
                    }

                    break;
            }

            throw new Exception("Card image could not be returned.");
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
