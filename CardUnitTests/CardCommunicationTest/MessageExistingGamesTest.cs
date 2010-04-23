// <copyright file="MessageExistingGamesTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the ExistingGamesMessage.</summary>
namespace CardUnitTests
{
    using System.Collections.ObjectModel;
    using CardCommunication.Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for MessageExistingGamesTest and is intended
    /// to contain all MessageExistingGamesTest Unit Tests
    /// </summary>
    [TestClass()]
    public class MessageExistingGamesTest
    {
        /// <summary>
        /// A test for BuildMessage
        /// </summary>
        [TestMethod()]
        public void BuildMessageExistingGamesTest()
        {
            MessageExistingGames target = new MessageExistingGames(); // TODO: Initialize to an appropriate value
            Collection<Collection<string>> action = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            target.BuildMessage(action);
            
            actual = target.Validated;

            Assert.AreEqual(expected, actual);
        }
    }
}
