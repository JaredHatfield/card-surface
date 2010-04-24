// <copyright file="PlayingAreaTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit test for PlayingArea Class.</summary>
namespace CardUnitTests
{
    using System;
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for PlayingAreaTest and is intended
    /// to contain all PlayingAreaTest Unit Tests
    /// </summary>
    [TestClass()]
    public class PlayingAreaTest
    {
        /// <summary>
        /// The test context instance.
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// inforrmation about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return this.testContextInstance; }
            set { this.testContextInstance = value; }
        }

        /// <summary>
        /// A test for PlayingArea Constructor
        /// </summary>
        [TestMethod()]
        public void PlayingAreaConstructorTest()
        {
            PlayingArea target = new PlayingArea();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
