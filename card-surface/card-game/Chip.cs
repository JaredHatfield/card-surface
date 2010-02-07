// <copyright file="Chip.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A single chip.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;

    /// <summary>
    /// A single chip.
    /// </summary>
    public class Chip : PhysicalObject
    {
        /// <summary>
        /// 
        /// </summary>
        private int amount;

        /// <summary>
        /// 
        /// </summary>
        private Color chipColor;

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
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
