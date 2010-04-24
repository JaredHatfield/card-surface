// <copyright file="AccountControllerTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A test for AccountController.</summary>
namespace CardUnitTests
{
    using CardAccount;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for AccountControllerTest and is intended
    /// to contain all AccountControllerTest Unit Tests
    /// </summary>
    [TestClass()]
    public class AccountControllerTest
    {
        /// <summary>
        /// A test for CreateAccount
        /// </summary>
        [TestMethod()]
        public void CreateAccountTest()
        {
            AccountController_Accessor target = new AccountController_Accessor(); // TODO: Initialize to an appropriate value
            string username = "Anybody"; // TODO: Initialize to an appropriate value
            string password = "anything"; // TODO: Initialize to an appropriate value
            string imagePath = "http://www.gravatar.com/avatar/"; // TODO: Initialize to an appropriate value
            ////AccountController_Accessor expected = false; // TODO: Initialize to an appropriate value
            target.CreateAccount(username, password, imagePath);

            target.CreateFlatFile("file");
            
            AccountController_Accessor actualTarget = new AccountController_Accessor();
            actualTarget.ReadFlatFile("file");
 
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
