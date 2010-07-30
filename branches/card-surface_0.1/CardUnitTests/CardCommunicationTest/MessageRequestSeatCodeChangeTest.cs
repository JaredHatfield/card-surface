// <copyright file="MessageRequestSeatCodeChangeTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the RequestSeatCodeChange Message.</summary>
namespace CardUnitTests
{
    using System;
    using System.Collections.ObjectModel;
    using CardCommunication.Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for MessageRequestSeatCodeChangeTest and is intended
    /// to contain all MessageRequestSeatCodeChangeTest Unit Tests
    /// </summary>
    [TestClass()]
    public class MessageRequestSeatCodeChangeTest
    {
        /// <summary>
        /// A test for MessageRequestSeatCodeChange Constructor
        /// </summary>
        [TestMethod()]
        public void MessageRequestSeatCodeChangeConstructorTest()
        {
            MessageRequestSeatCodeChange target = new MessageRequestSeatCodeChange();
            bool expected = true;
            bool actual = target.BuildMessage(Guid.Empty);

            Assert.AreEqual(expected, actual);
        }
    }
}
