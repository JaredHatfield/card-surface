// <copyright file="ChipTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the Chip class.</summary>
namespace CardUnitTests
{
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for ChipTest and is intended
    /// to contain all ChipTest Unit Tests
    /// </summary>
    [TestClass()]
    public class ChipTest
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
            Chip chip1 = new Chip(10, System.Drawing.Color.Blue);
            Chip chip2 = new Chip(10, System.Drawing.Color.Blue);
            Assert.AreEqual(chip1, chip2, "Chip with same value and color are equal");

            Chip chip3 = new Chip(20, System.Drawing.Color.Blue);
            Assert.AreNotEqual(chip1, chip3, "Chips with different values are not equal");

            Chip chip4 = new Chip(20, System.Drawing.Color.Green);
            Assert.AreNotEqual(chip3, chip4, "Chips with different colors are not equal");

            Assert.AreNotEqual(chip1, chip4, "Chips with different values and colors are not equal.");
        }
    }
}
