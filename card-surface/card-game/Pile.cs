// <copyright file="Pile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A collection of PhysicalObjects.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A collection of PhysicalObjects.
    /// </summary>
    public abstract class Pile
    {
        /// <summary>
        /// The collection of PhysicalObjects.
        /// </summary>
        private ObservableCollection<PhysicalObject> pileItems;

        /// <summary>
        /// True if the pile accept additional PhysicalObjects into its collection.
        /// </summary>
        private bool open;

        /// <summary>
        /// A unique id for the pile.
        /// </summary>
        private Guid id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pile"/> class.
        /// </summary>
        internal Pile()
        {
            this.pileItems = new ObservableCollection<PhysicalObject>();
            this.open = false;
            this.id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Pile"/> can have PhysicalObjects added to it.
        /// </summary>
        /// <value><c>true</c> if open; otherwise, <c>false</c>.</value>
        public bool Open
        {
            get { return this.open; }
            set { this.open = value; }
        }

        /// <summary>
        /// Gets the unique id.
        /// </summary>
        /// <value>The unique id.</value>
        public Guid Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the number of items.
        /// </summary>
        /// <value>The number of items.</value>
        public int NumberOfItems
        {
            get { return this.pileItems.Count; }
        }

        /// <summary>
        /// Gets the top item.
        /// </summary>
        /// <value>The top item.</value>
        public PhysicalObject TopItem
        {
            get
            {
                if (this.NumberOfItems > 0)
                {
                    return this.pileItems[this.NumberOfItems - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        protected ObservableCollection<PhysicalObject> Items
        {
            get { return this.pileItems; }
        }
        
        /// <summary>
        /// Adds an item to a pile.
        /// </summary>
        /// <param name="item">The item to add to a pile.</param>
        /// <returns>True if the item was added to the pile.</returns>
        public bool AddItem(PhysicalObject item)
        {
            if (this.open)
            {
                this.pileItems.Add(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the specified physical object
        /// </summary>
        /// <param name="item">The physical object to remove.</param>
        /// <returns>True if the physical object was removed; otherwise false.</returns>
        public bool RemoveItem(PhysicalObject item)
        {
            // TODO: Should a non-open pile be able to have items removed from it?
            // If this is changed, a corresponding check needs to be added to the MoveAction method in Game.
            if (this.open || true)
            {
                if (!this.pileItems.Contains(item))
                {
                    return false;
                }
                else
                {
                    this.pileItems.Remove(item);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the pile contains the specified physical object.
        /// </summary>
        /// <param name="physicalObject">The physical object.</param>
        /// <returns>
        ///     <c>true</c> if the pile contains the specified physical object; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsPhysicalObject(Guid physicalObject)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (this.Items[i].Id.Equals(physicalObject))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the physical object specified by a unique id.
        /// </summary>
        /// <param name="physicalObjectId">The unique id.</param>
        /// <returns>The PhysicalObject specified or null if it is not in this pile.</returns>
        internal PhysicalObject GetPhysicalObject(Guid physicalObjectId)
        {
            for (int i = 0; i < this.pileItems.Count; i++)
            {
                if (this.pileItems[i].Id.Equals(physicalObjectId))
                {
                    return this.pileItems[i];
                }
            }

            return null;
        }
    }
}
