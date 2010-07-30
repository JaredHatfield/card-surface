// <copyright file="MessageActionTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit tests for the ActionMessage.</summary>
namespace CardUnitTests
{
    using System.Collections.ObjectModel;
    using CardCommunication.Messages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// This is a test class for MessageActionTest and is intended
    /// to contain all MessageActionTest Unit Tests
    /// </summary>
    [TestClass()]
    public class MessageActionTest
    {
        /// <summary>
        /// A test for BuildMessage
        /// </summary>
        [TestMethod()]
        public void BuildMessageActionTest()
        {
            MessageAction target = new MessageAction(); // TODO: Initialize to an appropriate value
            Collection<string> action1 = new Collection<string>();
            Collection<string> action2 = new Collection<string>();
            action1.Add("Move");
            action1.Add("12345678-1234-1234-123456789012");
            action1.Add("43218765-1234-1234-123456789012"); // TODO: Initialize to an appropriate value
            action2.Add("Custom");
            action2.Add("Hit");
            action2.Add("Bob");
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            target.BuildMessage(action2);
            actual = target.Validated;

            Assert.AreEqual(expected, actual);
        }
    }
}
