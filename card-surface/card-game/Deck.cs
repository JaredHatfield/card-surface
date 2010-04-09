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
    using CardGame.GameException;
    using CardGame.GameFactory;

    /// <summary>
    /// A collection of cards that the game uses.
    /// The alphabet, the universal set, what we are working with!
    /// </summary>
    [Serializable]
    public class Deck
    {
        /// <summary>
        /// The means to create new cards.
        /// </summary>
        private static PhysicalObjectFactory factory = PhysicalObjectFactory.Instance();

        /// <summary>
        /// Create a CardPile containing a standard deck of cards.
        /// </summary>
        /// <returns>Standard deck of cards.</returns>
        public static CardPile StandardDeck()
        {
            CardPile cardPile = new CardPile();
            cardPile.Open = true;
            
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.King, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Queen, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Ten, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Nine, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Eight, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Seven, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Six, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Five, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Four, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Three, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Two, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Ace, Card.CardStatus.FaceDown));

            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.King, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Queen, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Ten, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Nine, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Eight, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Seven, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Six, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Five, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Four, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Three, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Two, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Ace, Card.CardStatus.FaceDown));

            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.King, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Queen, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Ten, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Nine, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Eight, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Seven, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Six, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Five, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Four, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Three, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Two, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Ace, Card.CardStatus.FaceDown));

            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.King, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Queen, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Ten, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Nine, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Eight, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Seven, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Six, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Five, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Four, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Three, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Two, Card.CardStatus.FaceDown));
            cardPile.AddItem(Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceDown));

            return cardPile;
        }

        /// <summary>
        /// Gets the image for a card
        /// </summary>
        /// <param name="cardname">The string representation of the card name.</param>
        /// <returns>The bitmap image for the card</returns>
        public static Bitmap CardImage(string cardname)
        {
            ICard card = null;
            switch (cardname)
            {
                case "AceClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Ace, Card.CardStatus.FaceDown);
                    break;
                case "AceDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Ace, Card.CardStatus.FaceDown);
                    break;
                case "AceHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Ace, Card.CardStatus.FaceDown);
                    break;
                case "AceSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceDown);
                    break;
                case "EightClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Eight, Card.CardStatus.FaceDown);
                    break;
                case "EightDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Eight, Card.CardStatus.FaceDown);
                    break;
                case "EightHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Eight, Card.CardStatus.FaceDown);
                    break;
                case "EightSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Eight, Card.CardStatus.FaceDown);
                    break;
                case "FiveClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Five, Card.CardStatus.FaceDown);
                    break;
                case "FiveDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Five, Card.CardStatus.FaceDown);
                    break;
                case "FiveHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Five, Card.CardStatus.FaceDown);
                    break;
                case "FiveSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Five, Card.CardStatus.FaceDown);
                    break;
                case "FourClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Four, Card.CardStatus.FaceDown);
                    break;
                case "FourDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Four, Card.CardStatus.FaceDown);
                    break;
                case "FourHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Four, Card.CardStatus.FaceDown);
                    break;
                case "FourSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Four, Card.CardStatus.FaceDown);
                    break;
                case "JackClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Jack, Card.CardStatus.FaceDown);
                    break;
                case "JackDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Jack, Card.CardStatus.FaceDown);
                    break;
                case "JackHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Jack, Card.CardStatus.FaceDown);
                    break;
                case "JackSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Jack, Card.CardStatus.FaceDown);
                    break;
                case "KingClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.King, Card.CardStatus.FaceDown);
                    break;
                case "KingDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.King, Card.CardStatus.FaceDown);
                    break;
                case "KingHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.King, Card.CardStatus.FaceDown);
                    break;
                case "KingSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.King, Card.CardStatus.FaceDown);
                    break;
                case "NineClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Nine, Card.CardStatus.FaceDown);
                    break;
                case "NineDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Nine, Card.CardStatus.FaceDown);
                    break;
                case "NineHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Nine, Card.CardStatus.FaceDown);
                    break;
                case "NineSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Nine, Card.CardStatus.FaceDown);
                    break;
                case "QueenClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Queen, Card.CardStatus.FaceDown);
                    break;
                case "QueenDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Queen, Card.CardStatus.FaceDown);
                    break;
                case "QueenHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Queen, Card.CardStatus.FaceDown);
                    break;
                case "QueenSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Queen, Card.CardStatus.FaceDown);
                    break;
                case "SevenClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Seven, Card.CardStatus.FaceDown);
                    break;
                case "SevenDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Seven, Card.CardStatus.FaceDown);
                    break;
                case "SevenHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Seven, Card.CardStatus.FaceDown);
                    break;
                case "SevenSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Seven, Card.CardStatus.FaceDown);
                    break;
                case "SixClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Six, Card.CardStatus.FaceDown);
                    break;
                case "SixDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Six, Card.CardStatus.FaceDown);
                    break;
                case "SixHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Six, Card.CardStatus.FaceDown);
                    break;
                case "SixSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Six, Card.CardStatus.FaceDown);
                    break;
                case "TenClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Ten, Card.CardStatus.FaceDown);
                    break;
                case "TenDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Ten, Card.CardStatus.FaceDown);
                    break;
                case "TenHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Ten, Card.CardStatus.FaceDown);
                    break;
                case "TenSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Ten, Card.CardStatus.FaceDown);
                    break;
                case "ThreeClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Three, Card.CardStatus.FaceDown);
                    break;
                case "ThreeDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Three, Card.CardStatus.FaceDown);
                    break;
                case "ThreeHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Three, Card.CardStatus.FaceDown);
                    break;
                case "ThreeSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Three, Card.CardStatus.FaceDown);
                    break;
                case "TwoClubs":
                    card = Deck.factory.MakeCard(Card.CardSuit.Clubs, Card.CardFace.Two, Card.CardStatus.FaceDown);
                    break;
                case "TwoDiamonds":
                    card = Deck.factory.MakeCard(Card.CardSuit.Diamonds, Card.CardFace.Two, Card.CardStatus.FaceDown);
                    break;
                case "TwoHearts":
                    card = Deck.factory.MakeCard(Card.CardSuit.Hearts, Card.CardFace.Two, Card.CardStatus.FaceDown);
                    break;
                case "TwoSpades":
                    card = Deck.factory.MakeCard(Card.CardSuit.Spades, Card.CardFace.Two, Card.CardStatus.FaceDown);
                    break;
                default:
                    throw new CardGamePhysicalObjectNotFoundException();
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
                            return CardGame.Properties.Resources.TenDiamonds;
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

            throw new CardGameException("Card image could not be returned.");
        }
    }
}
