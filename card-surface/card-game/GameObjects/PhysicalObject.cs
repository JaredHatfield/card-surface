﻿// <copyright file="PhysicalObject.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A physical object that is manipulated as part of the game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A physical object that is manipulated as part of the game.
    /// </summary>
    [Serializable]
    public abstract class PhysicalObject : IPhysicalObject
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
        protected internal PhysicalObject()
        {
            this.moveable = false;
            this.id = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalObject"/> class.
        /// </summary>
        /// <param name="id">The unique id.</param>
        protected internal PhysicalObject(Guid id)
        {
            this.moveable = false;
            this.id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalObject"/> class.
        /// </summary>
        /// <param name="moveable">if set to <c>true</c> [moveable].</param>
        /// <param name="id">The unique id.</param>
        protected internal PhysicalObject(bool moveable, Guid id)
        {
            this.moveable = moveable;
            this.id = id;
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception>
        public virtual int CompareTo(object obj)
        {
            if (obj is PhysicalObject)
            {
                PhysicalObject temp = (PhysicalObject)obj;
                return this.id.CompareTo(temp.id);
            }

            throw new ArgumentException("object is not a Chip");
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
