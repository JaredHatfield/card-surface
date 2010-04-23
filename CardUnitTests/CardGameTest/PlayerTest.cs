// <copyright file="PlayerTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit test for Player Class.</summary>
namespace CardUnitTests
{
    using System;
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for PlayerTest and is intended
    /// to contain all PlayerTest Unit Tests
    /// </summary>
    [TestClass()]
    public class PlayerTest
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
        /// A test for Player Constructor
        /// </summary>
        [TestMethod()]
        public void PlayerConstructorTest()
        {
            Player target = new Player();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
