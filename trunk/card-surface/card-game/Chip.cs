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
    public class Chip : PhysicalObject
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
    }
}
