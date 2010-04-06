﻿// <copyright file="WebActionCreateAccountTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>This is a test class for WebActionCreateAccount and is intended to contain all WebActionCreateAccount Unit Tests</summary>
namespace CardUnitTests
{
    using CardWeb;
    using CardWeb.WebComponents.WebActions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for WebActionCreateAccount and is intended
    /// to contain all WebActionCreateAccount Unit Tests
    /// </summary>
    [TestClass()]
    public class WebActionCreateAccountTest
    {
        /// <summary>
        /// The TestContext instance.
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
        /// A test for GetHeader
        /// </summary>
        [TestMethod()]
        public void GetHeaderTest()
        {
            WebRequest request = null; // TODO: Initialize to an appropriate value
            WebActionCreateAccount target = new WebActionCreateAccount(request); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetHeader();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for Execute
        /// </summary>
        [TestMethod()]
        public void ExecuteTest()
        {
            WebRequest request = null; // TODO: Initialize to an appropriate value
            WebActionCreateAccount target = new WebActionCreateAccount(request); // TODO: Initialize to an appropriate value
            target.Execute();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        /// A test for WebActionCreateAccount Constructor
        /// </summary>
        [TestMethod()]
        public void WebActionCreateAccountConstructorTest()
        {
            WebRequest request = null; // TODO: Initialize to an appropriate value
            WebActionCreateAccount target = new WebActionCreateAccount(request);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
