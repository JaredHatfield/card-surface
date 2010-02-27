// <copyright file="PileTest.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Unit test for Pile Class.</summary>
namespace CardUnitTests
{
    using System;
    using CardGame;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This is a test class for PileTest and is intended
    /// to contain all PileTest Unit Tests
    /// </summary>
    [TestClass()]
    public class PileTest
    {
        /// <summary>
        /// The test context instance.
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }

            set
            {
                this.testContextInstance = value;
            }
        }

        /// <summary>
        /// A test for ContainsPhysicalObject
        /// </summary>
        [TestMethod()]
        public void ContainsTest()
        {
            Pile target = this.CreatePile();
            PhysicalObject physicalObject = new Card();
            Assert.IsFalse(target.ContainsPhysicalObject(physicalObject.Id), "Pile does not contain the object.");
            target.Open = true;
            target.AddItem(physicalObject);
            Assert.IsTrue(target.ContainsPhysicalObject(physicalObject.Id), "Pile contains the object.");
        }

        /// <summary>
        /// A test for ContainsPhysicalObject
        /// </summary>
        [TestMethod()]
        public void ContainsPhysicalObjectTest()
        {
            Pile target = this.CreatePile();
            target.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            Card c = new Card(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceDown);
            Guid cid = c.Id;
            Assert.IsFalse(target.ContainsPhysicalObject(Guid.NewGuid()), "Pile does not contain an object Guid.");
            target.Open = true;
            target.AddItem(c);
            Assert.IsTrue(target.ContainsPhysicalObject(cid), "Pile contains an object Guid.");
        }

        /// <summary>
        /// A test for GetPhysicalObject
        /// </summary>
        [TestMethod()]
        public void GetPhysicalObjectTest()
        {
            Pile target = this.CreatePile();
            target.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            Card c = new Card(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceDown);
            Guid cid = c.Id;
            Assert.IsNull(target.GetPhysicalObject(cid), "Pile does not contain an object Guid.");
            target.Open = true;
            target.AddItem(c);
            Assert.IsNotNull(target.GetPhysicalObject(cid), "Pile does contain an object Guid.");
            Card c2 = (Card)target.GetPhysicalObject(cid);
            Assert.AreEqual(c.Id, c2.Id, "Retreived object ids are the same.");
            Assert.AreEqual(c, c2, "Retreived objects are the same.");
        }

        /// <summary>
        /// A test for RemoveItem
        /// </summary>
        [TestMethod()]
        public void RemoveItemTest()
        {
            Pile target = this.CreatePile();
            target.AddItem(new Card(Card.CardSuit.Diamonds, Card.CardFace.Five, Card.CardStatus.FaceDown));
            Card c = new Card(Card.CardSuit.Spades, Card.CardFace.Ace, Card.CardStatus.FaceDown);
            Guid cid = c.Id;
            target.Open = true;
            target.AddItem(c);
            target.AddItem(new Card(Card.CardSuit.Hearts, Card.CardFace.Jack, Card.CardStatus.FaceDown));
            Assert.IsNotNull(target.GetPhysicalObject(cid), "Object is in the pile.");
            target.RemoveItem(c);
            Assert.IsNull(target.GetPhysicalObject(cid), "Object is not in the pile.");
        }

        /// <summary>
        /// Creates the pile.
        /// </summary>
        /// <returns>An instance of the CardPile.</returns>
        internal virtual Pile CreatePile()
        {
            Pile target = new CardPile();
            return target;
        }
    }
}
