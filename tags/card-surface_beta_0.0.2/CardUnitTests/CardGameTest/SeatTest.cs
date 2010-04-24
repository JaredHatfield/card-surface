// <copyright file="SeatTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit test for Seat Class.</summary>
namespace CardUnitTests
{
    using System;
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for SeatTest and is intended
    /// to contain all SeatTest Unit Tests
    /// </summary>
    [TestClass()]
    public class SeatTest
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
        /// A test for Seat Constructor
        /// </summary>
        [TestMethod()]
        public void SeatConstructorTest()
        {
            Seat.SeatLocation location = new Seat.SeatLocation(); // TODO: Initialize to an appropriate value
            Seat target = new Seat(location);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
