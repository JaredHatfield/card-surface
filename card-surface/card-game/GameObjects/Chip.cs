﻿// <copyright file="Chip.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A single chip.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using CardGame.GameFactory;

    /// <summary>
    /// A single chip.
    /// </summary>
    [Serializable]
    public class Chip : PhysicalObject, IChip
    {
        /// <summary>
        /// The monetary value of the chip.
        /// </summary>
        private int amount;

        /// <summary>
        /// The color of the chip.
        /// </summary>
        private Color chipColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Chip"/> class.
        /// </summary>
        /// <param name="id">The chip's id.</param>
        /// <param name="amount">The chip's amount.</param>
        /// <param name="chipColor">The chip's color.</param>
        protected internal Chip(Guid id, int amount, Color chipColor)
            : base(id)
        {
            this.amount = amount;
            this.chipColor = chipColor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chip"/> class.
        /// </summary>
        /// <param name="amount">The chip's amount.</param>
        /// <param name="chipColor">The color of the chip.</param>
        protected internal Chip(int amount, Color chipColor)
            : base()
        {
            this.amount = amount;
            this.chipColor = chipColor;
        }

        /// <summary>
        /// Prevents a default instance of the Chip class from being created.
        /// </summary>
        private Chip()
            : base()
        {
            // DO NOT USE THIS CONSTRUCTOR!
        }

        /// <summary>
        /// Finalizes an instance of the Chip class.
        /// </summary>
        ~Chip()
        {
            PhysicalObjectFactory.Instance().Destroy(this.Id);
        }

        /// <summary>
        /// Gets the amount of the chip.
        /// </summary>
        /// <value>The amount.</value>
        public int Amount
        {
            get { return this.amount; }
        }

        /// <summary>
        /// Gets the color of the chip.
        /// </summary>
        /// <value>The color of the chip.</value>
        public Color ChipColor
        {
            get { return this.chipColor; }
        }

        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception>
        public override int CompareTo(object obj)
        {
            if (obj is Chip)
            {
                Chip temp = (Chip)obj;
                return this.Id.CompareTo(temp.Id);
            }

            throw new ArgumentException("object is not a Chip");
        }

        /// <summary>
        /// Equalses the specified chip.
        /// </summary>
        /// <param name="chip">The chip to equate with.</param>
        /// <returns>True if the color and the value of the chips are the same.</returns>
        public bool Equals(Chip chip)
        {
            if (this.CompareTo(chip) == 0)
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
            else if (obj is Chip)
            {
                return this.Equals(obj as Chip);
            }
            else
            {
                return false;
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
            return "[" + this.amount + "]";
        }
    }
}
