// <copyright file="WebActionManageAccountTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>This is a test class for WebActionManageAccount and is intended to contain all WebActionManageAccount Unit Tests</summary>
namespace CardUnitTests
{
    using CardWeb.WebComponents.WebActions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for WebActionManageAccount and is intended
    /// to contain all WebActionManageAccount Unit Tests
    /// </summary>
    [TestClass()]
    public class WebActionManageAccountTest
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
            WebActionManageAccount target = new WebActionManageAccount(); // TODO: Initialize to an appropriate value
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
            WebActionManageAccount target = new WebActionManageAccount(); // TODO: Initialize to an appropriate value
            target.Execute();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        /// A test for WebActionManageAccount Constructor
        /// </summary>
        [TestMethod()]
        public void WebActionManageAccountConstructorTest()
        {
            WebActionManageAccount target = new WebActionManageAccount();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
