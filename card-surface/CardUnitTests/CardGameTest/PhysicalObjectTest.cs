// <copyright file="PhysicalObjectTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit test for PhysicalObject Class.</summary>
namespace CardUnitTests
{
    using System;
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for PhysicalObjectTest and is intended
    /// to contain all PhysicalObjectTest Unit Tests
    /// </summary>
    [TestClass()]
    public class PhysicalObjectTest
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
        /// A test for PhysicalObject Constructor
        /// </summary>
        [TestMethod()]
        public void PhysicalObjectConstructorTest1()
        {
            PhysicalObject target = new PhysicalObject();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        /// A test for PhysicalObject Constructor
        /// </summary>
        [TestMethod()]
        public void PhysicalObjectConstructorTest()
        {
            bool moveable = false; // TODO: Initialize to an appropriate value
            Guid id = new Guid(); // TODO: Initialize to an appropriate value
            PhysicalObject target = new PhysicalObject(moveable, id);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
