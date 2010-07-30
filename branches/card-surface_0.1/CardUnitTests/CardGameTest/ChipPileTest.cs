// <copyright file="ChipPileTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the ChipPile class.</summary>
namespace CardUnitTests
{
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for ChipPileTest and is intended
    /// to contain all ChipPileTest Unit Tests
    /// </summary>
    [TestClass()]
    public class ChipPileTest
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
            ChipPile pile1 = new ChipPile();
            pile1.AddItem(new Chip(10, System.Drawing.Color.Blue));
            pile1.AddItem(new Chip(20, System.Drawing.Color.Green));
            ChipPile pile2 = new ChipPile();
            pile2.AddItem(new Chip(30, System.Drawing.Color.Red));
            Assert.AreEqual(pile1, pile2, "Two piles with the same amount are equal regardless of the number of chips in each pile.");
        }
    }
}
