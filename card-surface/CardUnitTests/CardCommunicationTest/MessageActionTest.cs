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
            Collection<string> action = new Collection<string>(); 
            action.Add("Move");
            action.Add("12345678-1234-1234-123456789012");
            action.Add("43218765-1234-1234-123456789012"); // TODO: Initialize to an appropriate value
            bool expected = true; // TODO: Initialize to an appropriate value
            bool actual;
            target.BuildMessage(action);
            actual = target.Validated;

            Assert.AreEqual(expected, actual);
        }
    }
}
