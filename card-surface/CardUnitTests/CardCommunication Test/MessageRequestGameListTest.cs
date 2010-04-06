// <copyright file="MessageRequestGameListTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the GameListMessage.</summary>
namespace CardUnitTests
{
    using System.Collections.ObjectModel;
    using CardCommunication.Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
        
    /// <summary>
    /// This is a test class for MessageRequestGameListTest and is intended
    /// to contain all MessageRequestGameListTest Unit Tests
    /// </summary>
    [TestClass()]
    public class MessageRequestGameListTest
    {
        /// <summary>
        /// A test for BuildMessage
        /// </summary>
        [TestMethod()]
        public void BuildMessageTest()
        {
            MessageRequestGameList target = new MessageRequestGameList(); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.BuildMessage();
            actual = target.Validated;
            Assert.AreEqual(expected, actual);
        }
    }
}
