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
        /// Initializes a new instance of the <see cref="Pile"/> class.
        /// </summary>
        internal Pile()
        {
            this.pileItems = new ObservableCollection<PhysicalObject>();
            this.open = false;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Pile"/> may accept new PhysicalObjects.
        /// </summary>
        /// <value><c>true</c> if open; otherwise, <c>false</c>.</value>
        public bool Open
        {
            get { return this.open; }
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
        public void AddItem(PhysicalObject item)
        {
            this.pileItems.Add(item);
        }
    }
}
