// <copyright file="Chip.cs" company="University of Louisville Speed School of Engineering">
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

    /// <summary>
    /// A single chip.
    /// </summary>
    public class Chip : PhysicalObject, IComparable
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
        internal Chip()
        {
            this.amount = 10;
            this.chipColor = Color.Blue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chip"/> class.
        /// </summary>
        /// <param name="amount">The chip's amount.</param>
        /// <param name="chipColor">The color of the chip.</param>
        internal Chip(int amount, Color chipColor)
        {
            this.amount = amount;
            this.chipColor = chipColor;
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
                int cmp1 = this.chipColor.ToArgb().CompareTo(temp.chipColor.ToArgb());
                if (cmp1 != 0)
                {
                    return cmp1;
                }
                else
                {
                    return this.amount - temp.amount;
                }
            }

            throw new ArgumentException("object is not a Chip");
        }
    }
}
