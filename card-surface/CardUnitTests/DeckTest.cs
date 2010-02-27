// <copyright file="DeckTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit test for Deck Class.</summary>
namespace CardUnitTests
{
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for DeckTest and is intended
    /// to contain all DeckTest Unit Tests
    /// </summary>
    [TestClass()]
    public class DeckTest
    {
        /// <summary>
        /// Test context instance.
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }

            set
            {
                this.testContextInstance = value;
            }
        }

        /// <summary>
        /// A test for StandardDeck
        /// </summary>
        [TestMethod()]
        public void StandardDeckTest()
        {
            CardPile expected = new CardPile();
            expected.Open = true;
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.King, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Queen, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Ten, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Nine, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Eight, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Seven, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Six, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Five, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Four, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Three, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Two, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Ace, Card.CardStatus.FaceDown));

            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.King, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Queen, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Ten, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Nine, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Eight, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Seven, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Six, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Five, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Four, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Three, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Two, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Clubs, Card.CardFace.Ace, Card.CardStatus.FaceDown));

            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.King, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Queen, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Ten, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Nine, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Eight, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Seven, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Six, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Five, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Four, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Three, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Two, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Ace, Card.CardStatus.FaceDown));

            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.King, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Queen, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Ten, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Nine, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Eight, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Seven, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Six, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Five, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Four, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Three, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Two, Card.CardStatus.FaceDown));
            expected.AddItem(new Card(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceDown));

            CardPile actual = Deck.StandardDeck();
            Assert.AreEqual(expected, actual, "The default order for a new deck of cards is incorrect.");
        }
    }
}
