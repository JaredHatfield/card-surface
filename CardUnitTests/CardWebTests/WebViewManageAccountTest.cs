// <copyright file="WebViewManageAccountTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>This is a test class for WebViewManageAccount and is intended to contain all WebViewManageAccount Unit Tests</summary>
namespace CardUnitTests
{
    using CardWeb.WebComponents.WebViews;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for WebViewManageAccount and is intended
    /// to contain all WebViewManageAccount Unit Tests
    /// </summary>
    [TestClass()]
    public class WebViewManageAccountTest
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
        /// A test for SendResponse
        /// </summary>
        [TestMethod()]
        public void SendResponseTest()
        {
            WebViewManageAccount target = new WebViewManageAccount(); // TODO: Initialize to an appropriate value
            target.SendResponse();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        /// A test for GetHeader
        /// </summary>
        [TestMethod()]
        public void GetHeaderTest()
        {
            WebViewManageAccount target = new WebViewManageAccount(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetHeader();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for GetContentType
        /// </summary>
        [TestMethod()]
        public void GetContentTypeTest()
        {
            WebViewManageAccount target = new WebViewManageAccount(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetContentType();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for GetContentLength
        /// </summary>
        [TestMethod()]
        public void GetContentLengthTest()
        {
            WebViewManageAccount target = new WebViewManageAccount(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.GetContentLength();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for GetContent
        /// </summary>
        [TestMethod()]
        [DeploymentItem("CardWeb.dll")]
        public void GetContentTest()
        {
            WebViewManageAccount_Accessor target = new WebViewManageAccount_Accessor(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetContent();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for WebViewManageAccount Constructor
        /// </summary>
        [TestMethod()]
        public void WebViewManageAccountConstructorTest()
        {
            WebViewManageAccount target = new WebViewManageAccount();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
