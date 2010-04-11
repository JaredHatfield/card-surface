// <copyright file="Pile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A collection of IPhysicalObjects.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;

    /// <summary>
    /// A collection of IPhysicalObjects.
    /// </summary>
    [Serializable]
    public abstract class Pile : INotifyPropertyChanged
    {
        /// <summary>
        /// The collection of IPhysicalObjects.
        /// </summary>
        private ObservableCollection<IPhysicalObject> pileItems;

        /// <summary>
        /// True if the pile accept additional IPhysicalObjects into its collection.
        /// </summary>
        private bool open;

        /// <summary>
        /// A unique id for the pile.
        /// </summary>
        private Guid id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pile"/> class.
        /// </summary>
        protected internal Pile()
        {
            this.pileItems = new ObservableCollection<IPhysicalObject>();
            this.open = false;
            this.id = Guid.NewGuid();

            // We want to trigger the PropertyChanged event when the underlying ObservableCollection changes.
            this.pileItems.CollectionChanged += new NotifyCollectionChangedEventHandler(this.NotifyItemsChanged);
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Pile"/> can have IPhysicalObjects added to it.
        /// </summary>
        /// <value><c>true</c> if open; otherwise, <c>false</c>.</value>
        public bool Open
        {
            get
            {
                return this.open;
            }

            set
            {
                this.open = value;
                this.NotifyPropertyChanged("Open");
            }
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
        public IPhysicalObject TopItem
        {
            get
            {
                if (this.NumberOfItems > 0)
                {
                    return this.pileItems[this.NumberOfItems - 1];
                }
                else
                {
                    throw new CardGamePhysicalObjectNotFoundException();
                }
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        protected ObservableCollection<IPhysicalObject> Items
        {
            get { return this.pileItems; }
        }

        /// <summary>
        /// Adds an item to a pile.
        /// </summary>
        /// <param name="item">The item to add to a pile.</param>
        /// <returns>True if the item was added to the pile.</returns>
        public bool AddItem(IPhysicalObject item)
        {
            if (this.open)
            {
                this.pileItems.Add(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the specified PhysicalObject from the pile.
        /// </summary>
        /// <param name="item">The PhysicalObject. to remove.</param>
        /// <returns>True if the PhysicalObject was removed; otherwise false.</returns>
        public bool RemoveItem(IPhysicalObject item)
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
        /// Removes the specified PhysicalObject from the pile.
        /// </summary>
        /// <param name="id">The id of the PhysicalObject to remove.</param>
        /// <returns>True if the PhysicalObject was removed; otherwise false.</returns>
        public bool RemoveItem(Guid id)
        {
            IPhysicalObject po = this.GetPhysicalObject(id);
            return this.RemoveItem(po);
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
            for (int i = 0; i < this.pileItems.Count; i++)
            {
                if (this.pileItems[i].Id.Equals(physicalObject))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a PhysicalObject at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The PhysicalObject requested.</returns>
        public IPhysicalObject GetPhysicalObject(int index)
        {
            // TODO: Do we really need this function?  It was added to make things easier, but is it necessary to have public access?
            if (index < this.pileItems.Count)
            {
                return this.pileItems[index];
            }
            else
            {
                throw new CardGamePhysicalObjectNotFoundException();
            }
        }

        /// <summary>
        /// Gets the physical object specified by a unique id.
        /// </summary>
        /// <param name="physicalObjectId">The unique id.</param>
        /// <returns>The PhysicalObject specified or null if it is not in this pile.</returns>
        internal IPhysicalObject GetPhysicalObject(Guid physicalObjectId)
        {
            for (int i = 0; i < this.pileItems.Count; i++)
            {
                if (this.pileItems[i].Id.Equals(physicalObjectId))
                {
                    return this.pileItems[i];
                }
            }

            throw new CardGamePhysicalObjectNotFoundException();
        }

        /// <summary>
        /// Properties the changed event handeler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void NotifyItemsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.NotifyPropertyChanged("Items");
        }

        /// <summary>
        /// Signals that a property of this object has changed.
        /// </summary>
        /// <param name="info">The property that is being affected.</param>
        protected void NotifyPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
