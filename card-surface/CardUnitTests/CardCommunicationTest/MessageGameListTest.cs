// <copyright file="MessageGameListTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the GameListMessage.</summary>
namespace CardUnitTests
{
    using System.Collections.ObjectModel;
    using CardCommunication.Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for MessageGameListTest and is intended
    /// to contain all MessageGameListTest Unit Tests
    /// </summary>
    [TestClass()]
    public class MessageGameListTest
    {
        /// <summary>
        /// A test for BuildMessage
        /// </summary>
        [TestMethod()]
        public void BuildMessageTest()
        {
            MessageGameList target = new MessageGameList(); // TODO: Initialize to an appropriate value
            ReadOnlyCollection<string> action = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.BuildMessage(action);
            actual = true;
            Assert.AreEqual(expected, actual);
        }
    }
}
