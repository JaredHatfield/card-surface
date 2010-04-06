// <copyright file="WebViewCreateAccountTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>This is a test class for WebViewCreateAccount and is intended to contain all WebViewCreateAccount Unit Tests</summary>
namespace CardUnitTests
{
    using CardWeb;
    using CardWeb.WebComponents.WebViews;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for WebViewCreateAccount and is intended
    /// to contain all WebViewCreateAccount Unit Tests
    /// </summary>
    [TestClass()]
    public class WebViewCreateAccountTest
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
            WebRequest request = null; // TODO: Initialize to an appropriate value
            WebViewCreateAccount target = new WebViewCreateAccount(request); // TODO: Initialize to an appropriate value
            target.SendResponse();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        /// A test for GetHeader
        /// </summary>
        [TestMethod()]
        public void GetHeaderTest()
        {
            WebRequest request = null; // TODO: Initialize to an appropriate value
            WebViewCreateAccount target = new WebViewCreateAccount(request); // TODO: Initialize to an appropriate value
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
            WebRequest request = null; // TODO: Initialize to an appropriate value
            WebViewCreateAccount target = new WebViewCreateAccount(request); // TODO: Initialize to an appropriate value
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
            WebRequest request = null; // TODO: Initialize to an appropriate value
            WebViewCreateAccount target = new WebViewCreateAccount(request); // TODO: Initialize to an appropriate value
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
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            WebViewCreateAccount_Accessor target = new WebViewCreateAccount_Accessor(param0); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.GetContent();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        /// A test for WebViewCreateAccount Constructor
        /// </summary>
        [TestMethod()]
        public void WebViewCreateAccountConstructorTest1()
        {
            WebRequest request = null; // TODO: Initialize to an appropriate value
            WebViewCreateAccount target = new WebViewCreateAccount(request);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        /// A test for WebViewCreateAccount Constructor
        /// </summary>
        [TestMethod()]
        public void WebViewCreateAccountConstructorTest()
        {
            WebRequest request = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            WebViewCreateAccount target = new WebViewCreateAccount(request, message);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
