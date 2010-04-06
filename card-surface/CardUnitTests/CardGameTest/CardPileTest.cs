// <copyright file="CardPileTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the CardPile class.</summary>
namespace CardUnitTests
{
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for CardPileTest and is intended
    /// to contain all CardPileTest Unit Tests
    /// </summary>
    [TestClass()]
    public class CardPileTest
    {
        /// <summary>
        /// The test context instance.
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
        /// A test for Expandable
        /// </summary>
        [TestMethod()]
        public void ExpandableTest()
        {
            // Checks default for expandable
            CardPile target = new CardPile();
            Assert.AreEqual(target.Expandable, false, "Default constructor for CardPile is not expandable.");
        }

        /// <summary>
        /// A test for CardPile Constructor
        /// </summary>
        [TestMethod()]
        public void CardPileConstructorTest1()
        {
            // Forcing a CardPile to be expandable
            CardPile target = new CardPile(true, true);
            Assert.IsTrue(target.Expandable, "Set value of expandable to true.");

            // Forcing a CardPile to not be expandable
            CardPile target2 = new CardPile(false, true);
            Assert.IsFalse(target2.Expandable, "Set value of expandable to false.");
        }

        /// <summary>
        /// A test for CardPile Constructor
        /// </summary>
        [TestMethod()]
        public void CardPileConstructorTest()
        {
            CardPile target = new CardPile();

            Assert.IsFalse(target.Open, "CardPile defaults to not open.");
            Assert.AreEqual(target.NumberOfItems, 0, "CardPile defaults to no items.");
            Assert.IsNull(target.TopItem, "CardPile top item is null.");

            // Test adding an item to the CardPile
            Card card = new Card(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceUp);
            target.Open = true;
            target.AddItem(card);
            Assert.AreEqual(target.NumberOfItems, 1, "When a card is added to a pile it has an item in it.");
            Assert.AreEqual(target.TopItem, card, "The card just added to a pile is the top card.");
        }

        /// <summary>
        /// A test for DrawCard
        /// </summary>
        [TestMethod()]
        public void DrawCardTest()
        {
            CardPile target = new CardPile();
            target.Open = true;
            Card card1 = new Card(Card.CardSuit.Clubs, Card.CardFace.Five, Card.CardStatus.FaceDown);
            Card card2 = new Card(Card.CardSuit.Spades, Card.CardFace.Five, Card.CardStatus.FaceDown);
            target.AddItem(card1);
            target.AddItem(card2);
            Assert.AreEqual(card2, target.DrawCard(), "The last card added is the first one drawn.");
            Assert.AreEqual(card1, target.DrawCard(), "The previous card added is the next one drawn.");
        }

        /// <summary>
        /// A test for Equals
        /// </summary>
        [TestMethod()]
        public void EqualsTest()
        {
            CardPile pile1 = new CardPile();
            pile1.Open = true;

            CardPile pile2 = new CardPile();
            pile2.Open = true;

            Card card1 = new Card(Card.CardSuit.Clubs, Card.CardFace.Ace, Card.CardStatus.FaceDown);
            Card card2 = new Card(Card.CardSuit.Diamonds, Card.CardFace.King, Card.CardStatus.FaceDown);

            pile1.AddItem(card1);
            pile1.AddItem(card2);

            Assert.AreNotEqual(pile1, pile2, "Two CardPiles with different cards are not the same.");

            pile2.AddItem(card1);
            pile2.AddItem(card2);

            Assert.AreEqual(pile1, pile2, "Two CardPiles are the same.");
        }
    }
}
