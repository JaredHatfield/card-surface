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
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public int Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }

        /// <summary>
        /// Gets or sets the color of the chip.
        /// </summary>
        /// <value>The color of the chip.</value>
        public Color ChipColor
        {
            get { return this.chipColor; }
            set { this.chipColor = value; }
        }
    }
}
