// <copyright file="Pile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A collection of PhysicalObjects.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
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
        private List<PhysicalObject> items;

        /// <summary>
        /// True if the pile accept additional PhysicalObjects into its collection.
        /// </summary>
        private bool open;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pile"/> class.
        /// </summary>
        internal Pile()
        {
            this.items = new List<PhysicalObject>();
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
    }
}
