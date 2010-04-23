// <copyright file="PhysicalObjectFactory.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Class that is used to create PhysicalObjects.</summary>
namespace CardGame.GameFactory
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;

    /// <summary>
    /// Class that is used to create PhysicalObjects.
    /// </summary>
    public class PhysicalObjectFactory
    {
        /// <summary>
        /// The singleton instance of the PhysicalObjectFactory.
        /// </summary>
        private static PhysicalObjectFactory instance;

        /// <summary>
        /// The CardFactroy that creates new Cards.
        /// </summary>
        private static CardFactory cardFactory;

        /// <summary>
        /// The ChipFactroy that creates new Chips.
        /// </summary>
        private static ChipFactory chipFactory;

        /// <summary>
        /// A flag that is set to determine if duplication should be checked or not.
        /// </summary>
        private static bool preventDuplication = true;

        /// <summary>
        /// The list of created Guids.
        /// </summary>
        private Collection<Guid> createdObjects;

        /// <summary>
        /// Prevents a default instance of the PhysicalObjectFactory class from being created.
        /// </summary>
        private PhysicalObjectFactory()
        {
            if (PhysicalObjectFactory.chipFactory == null)
            {
                throw new CardGameFactoryException("ChipFactory needs to be set before instance can be created.");
            }
            else if (PhysicalObjectFactory.cardFactory == null)
            {
                throw new CardGameFactoryException("CardFactory needs to be set before instance can be created.");
            }
            else
            {
                // It looks like we are good to go!
                this.createdObjects = new Collection<Guid>();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [prevent duplication].
        /// </summary>
        /// <value><c>true</c> if [prevent duplication]; otherwise, <c>false</c>.</value>
        public static bool PreventDuplication
        {
            get { return PhysicalObjectFactory.preventDuplication; }
            set { PhysicalObjectFactory.preventDuplication = value; }
        }

        /// <summary>
        /// Returns an instance of the PhysicalObjectFactory.
        /// </summary>
        /// <returns>The singleton instance of PhysicalObjectFactory.</returns>
        public static PhysicalObjectFactory Instance()
        {
            if (PhysicalObjectFactory.instance == null)
            {
                PhysicalObjectFactory.instance = new PhysicalObjectFactory();
            }

            return PhysicalObjectFactory.instance;
        }

        /// <summary>
        /// Subscribes the ChipFactory to the application.
        /// This method can only be called once or it will thrown an CardGameFactoryException!
        /// </summary>
        /// <param name="chipFactory">The chip factory.</param>
        public static void SubscribeChipFactory(ChipFactory chipFactory)
        {
            if (PhysicalObjectFactory.chipFactory == null)
            {
                PhysicalObjectFactory.chipFactory = chipFactory;
            }
            else
            {
                throw new CardGameFactoryException("Attempted to subscribe second ChipFactory");
            }
        }

        /// <summary>
        /// Subscribes the CardFactory to the application.
        /// This method can only be called once or it will thrown an CardGameFactoryException!
        /// </summary>
        /// <param name="cardFactory">The chip factory.</param>
        public static void SubscribeCardFactory(CardFactory cardFactory)
        {
            if (PhysicalObjectFactory.cardFactory == null)
            {
                PhysicalObjectFactory.cardFactory = cardFactory;
            }
            else
            {
                throw new CardGameFactoryException("Attempted to subscribe second CardFactory");
            }
        }

        /// <summary>
        /// Creates a new ICard.
        /// </summary>
        /// <param name="id">The card's id.</param>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        /// <returns>An ICard with a a specified Guid.</returns>
        public ICard MakeCard(
            Guid id,
            Card.CardSuit suit,
            Card.CardFace face,
            Card.CardStatus status)
        {
            if (PhysicalObjectFactory.preventDuplication && this.createdObjects.Contains(id))
            {
                throw new CardGameDuplicatePhysicalObjectException();
            }
            else
            {
                ICard card = PhysicalObjectFactory.cardFactory.MakeCard(id, suit, face, status);
                this.createdObjects.Add(card.Id);
                return card;
            }
        }

        /// <summary>
        /// Creates a new ICard.
        /// </summary>
        /// <param name="suit">The card's suit.</param>
        /// <param name="face">The card's face.</param>
        /// <param name="status">The card's status.</param>
        /// <returns>An ICard with a new Guid.</returns>
        public ICard MakeCard(
            Card.CardSuit suit,
            Card.CardFace face,
            Card.CardStatus status)
        {
            ICard card = PhysicalObjectFactory.cardFactory.MakeCard(suit, face, status);
            this.createdObjects.Add(card.Id);
            return card;
        }

        /// <summary>
        /// Creates a new IChip.
        /// </summary>
        /// <param name="id">The chip's id.</param>
        /// <param name="amount">The chip's amount.</param>
        /// <returns>An IChip with a specified Guid.</returns>
        public IChip MakeChip(Guid id, int amount)
        {
            if (PhysicalObjectFactory.preventDuplication && this.createdObjects.Contains(id))
            {
                throw new CardGameDuplicatePhysicalObjectException();
            }
            else
            {
                IChip chip = PhysicalObjectFactory.chipFactory.MakeChip(id, amount);
                this.createdObjects.Add(chip.Id);
                return chip;
            }
        }

        /// <summary>
        /// Creates a new IChip.
        /// </summary>
        /// <param name="amount">The chip's amount.</param>
        /// <returns>An IChip with a new Guid.</returns>
        public IChip MakeChip(int amount)
        {
            IChip chip = PhysicalObjectFactory.chipFactory.MakeChip(amount);
            this.createdObjects.Add(chip.Id);
            return chip;
        }

        /// <summary>
        /// The specified object id has been destroyed so remove it from the list of objects.
        /// </summary>
        /// <param name="id">The id to remove.</param>
        public void Destroy(Guid id)
        {
            this.createdObjects.Remove(id);
        }
    }
}
