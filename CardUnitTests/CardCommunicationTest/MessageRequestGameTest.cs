// <copyright file="MessageRequestGameTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the RequestGameMessage.</summary>
namespace CardUnitTests
{
    using System.Collections.ObjectModel;
    using CardCommunication.Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for MessageRequestGameTest and is intended
    /// to contain all MessageRequestGameTest Unit Tests
    /// </summary>
    [TestClass()]
    public class MessageRequestGameTest
    {
        /// <summary>
        /// A test for BuildMessage
        /// </summary>
        [TestMethod()]
        public void BuildMessageRequestGameTest()
        {
            MessageRequestGame target = new MessageRequestGame(); // TODO: Initialize to an appropriate value
            string gameType = null; // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            target.BuildMessage(gameType);

            actual = target.Validated;

            Assert.AreEqual(expected, actual);
        }
    }
}
