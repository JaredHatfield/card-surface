// <copyright file="Pile.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary></summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public abstract class Pile
    {
        /// <summary>
        /// 
        /// </summary>
        private List<PhysicalObject> items;

        /// <summary>
        /// True if the pile accept additional PhysicalObjects into its collection.
        /// </summary>
        private Boolean open;


        /// <summary>
        /// Gets a value indicating whether this <see cref="Pile"/> may accept new PhysicalObjects.
        /// </summary>
        /// <value><c>true</c> if open; otherwise, <c>false</c>.</value>
        public Boolean Open
        {
            get { return this.open; }
        }
    }
}
