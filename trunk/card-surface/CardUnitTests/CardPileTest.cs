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
            Assert.AreEqual(target.Expandable, false);
        }

        /// <summary>
        /// A test for CardPile Constructor
        /// </summary>
        [TestMethod()]
        public void CardPileConstructorTest1()
        {
            // Forcing a CardPile to be expandable
            CardPile target = new CardPile(true);
            Assert.IsTrue(target.Expandable);

            // Forcing a CardPile to not be expandable
            CardPile target2 = new CardPile(false);
            Assert.IsFalse(target2.Expandable);
        }

        /// <summary>
        /// A test for CardPile Constructor
        /// </summary>
        [TestMethod()]
        public void CardPileConstructorTest()
        {
            CardPile target = new CardPile();

            Assert.IsFalse(target.Open);
            Assert.AreEqual(target.NumberOfItems, 0);
            Assert.IsNull(target.TopItem);

            // Test adding an item to the CardPile
            Card card = new Card(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceUp);
            target.AddItem(card);
            Assert.AreEqual(target.NumberOfItems, 1);
            Assert.AreEqual(target.TopItem, card);
        }
    }
}
