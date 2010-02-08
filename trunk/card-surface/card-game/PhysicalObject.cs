// <copyright file="PhysicalObject.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A physical object that is manipulated as part of the game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A physical object that is manipulated as part of the game.
    /// </summary>
    public class PhysicalObject
    {
        /// <summary>
        /// Determines if the object can be physically moved.
        /// </summary>
        private bool moveable;

        /// <summary>
        /// A unique identifier for the object.
        /// </summary>
        private Guid id;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalObject"/> class.
        /// </summary>
        internal PhysicalObject()
        {
            this.moveable = false;
            this.id = Guid.NewGuid();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PhysicalObject"/> is moveable.
        /// </summary>
        /// <value><c>true</c> if moveable; otherwise, <c>false</c>.</value>
        public bool Moveable
        {
            get { return this.moveable; }
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The unique id.</value>
        public Guid Id
        {
            get { return this.id; }
        }
    }
}
