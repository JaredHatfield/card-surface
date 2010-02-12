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
            CardPile expected = null; // TODO: Initialize to an appropriate value
            CardPile actual;
            actual = Deck.StandardDeck();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
