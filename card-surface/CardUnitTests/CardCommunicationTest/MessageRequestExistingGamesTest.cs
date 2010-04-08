// <copyright file="MessageRequestExistingGamesTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the GameListMessage.</summary>
namespace CardUnitTests
{
    using System.Collections.ObjectModel;
    using CardCommunication.Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for MessageRequestExistingGamesTest and is intended
    /// to contain all MessageRequestExistingGamesTest Unit Tests
    /// </summary>
    [TestClass()]
    public class MessageRequestExistingGamesTest
    {
        /// <summary>
        /// A test for BuildMessage
        /// </summary>
        [TestMethod()]
        public void BuildMessageTest()
        {
            MessageRequestExistingGames target = new MessageRequestExistingGames(); // TODO: Initialize to an appropriate value
            string selectedGame = string.Empty; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.BuildMessage(selectedGame);
            actual = target.Validated;
            Assert.AreEqual(expected, actual);
        }
    }
}
