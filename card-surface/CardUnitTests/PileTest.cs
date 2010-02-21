// <copyright file="PileTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit test for Pile Class.</summary>
namespace CardUnitTests
{
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for PileTest and is intended
    /// to contain all PileTest Unit Tests
    /// </summary>
    [TestClass()]
    public class PileTest
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
        /// A test for ContainsPhysicalObject
        /// </summary>
        [TestMethod()]
        public void ContainsTest()
        {
            Pile target = this.CreatePile();
            PhysicalObject physicalObject = new Card();
            Assert.IsFalse(target.ContainsPhysicalObject(physicalObject.Id), "Pile does not contain the object.");
            target.Open = true;
            target.AddItem(physicalObject);
            Assert.IsTrue(target.ContainsPhysicalObject(physicalObject.Id), "Pile contains the object.");
        }

        /// <summary>
        /// Creates the pile.
        /// </summary>
        /// <returns>An instance of the CardPile.</returns>
        internal virtual Pile CreatePile()
        {
            Pile target = new CardPile();
            return target;
        }
    }
}
