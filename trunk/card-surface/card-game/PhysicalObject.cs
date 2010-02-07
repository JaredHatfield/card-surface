// <copyright file="PhysicalObject.cs" company="University of Louisville Speed School of Engineering">
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
    public class PhysicalObject
    {
        /// <summary>
        /// 
        /// </summary>
        private Boolean moveable;

        /// <summary>
        /// 
        /// </summary>
        private Guid id;

        /// <summary>
        /// Gets a value indicating whether this <see cref="PhysicalObject"/> is moveable.
        /// </summary>
        /// <value><c>true</c> if moveable; otherwise, <c>false</c>.</value>
        public Boolean Moveable
        {
            get { return this.moveable; }
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id
        {
            get { return this.id; }
        }
    }
}
