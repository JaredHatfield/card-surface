// <copyright file="CardTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the Card class.</summary>
namespace CardUnitTests
{
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for CardTest and is intended
    /// to contain all CardTest Unit Tests
    /// </summary>
    [TestClass()]
    public class CardTest
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
            get { return this.testContextInstance; }
            set { this.testContextInstance = value; }
        }

        /// <summary>
        /// A test for Equals
        /// </summary>
        [TestMethod()]
        public void EqualsTest()
        {
            Card card1 = new Card(Card.CardSuit.Diamonds, Card.CardFace.Nine, Card.CardStatus.FaceUp);
            Card card2 = new Card(Card.CardSuit.Diamonds, Card.CardFace.Nine, Card.CardStatus.FaceDown);
            Assert.AreEqual(card1, card2, "Cards with different status are still equal.");

            Card card3 = new Card(Card.CardSuit.Clubs, Card.CardFace.Nine, Card.CardStatus.FaceUp);
            Assert.AreNotEqual(card1, card3, "Cards with different suits are not the same");

            Card card4 = new Card(Card.CardSuit.Clubs, Card.CardFace.Ace, Card.CardStatus.FaceUp);
            Assert.AreNotEqual(card3, card4, "Cards with different faces are not the same");

            Assert.AreNotEqual(card1, card4, "Cards with different face and suit are not equal");
        }
    }
}
