// <copyright file="MessageFlipCardTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the FlipCardMessage.</summary>
namespace CardUnitTests
{
    using System;
    using System.Collections.ObjectModel;
    using CardCommunication.Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for MessageFlipCardTest and is intended
    /// to contain all MessageFlipCardTest Unit Tests
    /// </summary>
    [TestClass()]
    public class MessageFlipCardTest
    {
        /// <summary>
        /// A test for BuildMessage
        /// </summary>
        [TestMethod()]
        public void BuildMessageFlipCardTest()
        {
            MessageFlipCard target = new MessageFlipCard(); // TODO: Initialize to an appropriate value
            Guid card = new Guid("12345678-1234-1234-1234-123456789012"); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;

            target.BuildMessage(card);

            actual = target.Validated;

            Assert.AreEqual(expected, actual);
        }
    }
}
