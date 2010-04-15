// <copyright file="ChipPile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A pile of chips.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A pile of chips.
    /// </summary>
    [Serializable]
    public class ChipPile : Pile, IComparable, IEquatable<ChipPile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChipPile"/> class.
        /// </summary>
        protected internal ChipPile()
            : base()
        {
        }

        /// <summary>
        /// Gets the total chip amount in the pile.
        /// </summary>
        /// <value>The total chip amount in the pile.</value>
        public int Amount
        {
            get
            {
                int total = 0;
                for (int i = 0; i < this.NumberOfItems; i++)
                {
                    Chip chip = this.Items[i] as Chip;
                    total += chip.Amount;
                }

                return total;
            }
        }

        /// <summary>
        /// Gets the chips that are in the pile.
        /// </summary>
        /// <value>The chips in the pile.</value>
        public ReadOnlyObservableCollection<IPhysicalObject> Chips
        {
            get { return new ReadOnlyObservableCollection<IPhysicalObject>(this.Items); }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (obj is ChipPile)
            {
                ChipPile temp = (ChipPile)obj;
                return this.Amount - temp.Amount;
            }

            throw new ArgumentException("object is not a ChipPile");
        }

        /// <summary>
        /// Equalses the specified chip pile.
        /// </summary>
        /// <param name="chipPile">The chip pile.</param>
        /// <returns>True if the sum of the amounts of the chips in the two piles is the same</returns>
        public bool Equals(ChipPile chipPile)
        {
            if (this.Amount == chipPile.Amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">The <paramref name="obj"/> parameter is null.</exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return base.Equals(obj);
            }
            else if (obj is ChipPile)
            {
                return this.Equals(obj as ChipPile);
            }
            else
            {
                throw new InvalidCastException("The 'obj' argument is not a ChipPile object.");
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string chips = string.Empty;
            for (int i = 0; i < this.Items.Count; i++)
            {
                chips += i + "=" + (this.Items[i] as IChip).ToString() + " ";
            }

            if (this.Items.Count == 0)
            {
                return "Empty.";
            }

            return chips;
        }

        /// <summary>
        /// Updates the specified pile.
        /// This updates the Guid and open attributes.  It does not update the pile items.
        /// </summary>
        /// <param name="pile">The pile to reflect.</param>
        internal override void Update(Pile pile)
        {
            base.Update(pile);
        }
    }
}
