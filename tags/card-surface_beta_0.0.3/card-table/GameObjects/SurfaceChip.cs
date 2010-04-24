// <copyright file="SurfaceChip.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Implements the SurfaceChip.</summary>
namespace CardTable.GameObjects
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using CardGame;
    using Microsoft.Surface;
    using Microsoft.Surface.Presentation;
    using Microsoft.Surface.Presentation.Controls;

    /// <summary>
    /// The surface chip.
    /// </summary>
    internal class SurfaceChip : Chip
    {
        /// <summary>
        /// The object image source.
        /// </summary>
        private string objectImageSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceChip"/> class.
        /// </summary>
        /// <param name="id">The chip's id.</param>
        /// <param name="amount">The chip's amount.</param>
        /// <param name="chipColor">The chip's color.</param>
        protected internal SurfaceChip(Guid id, int amount, Color chipColor)
            : base(id, amount, chipColor)
        {
            this.objectImageSource = this.FindChipImageSource(chipColor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceChip"/> class.
        /// </summary>
        /// <param name="amount">The chip's amount.</param>
        /// <param name="chipColor">The color of the chip.</param>
        protected internal SurfaceChip(int amount, Color chipColor)
            : base(amount, chipColor)
        {
            this.objectImageSource = this.FindChipImageSource(chipColor);
        }

        /// <summary>
        /// Gets the object image source.
        /// </summary>
        /// <value>The object image source.</value>
        public string ObjectImageSource
        {
            get
            {
                return this.objectImageSource;
            }
        }

        /// <summary>
        /// Finds the chip image source.
        /// </summary>
        /// <param name="color">The color of the chip.</param>
        /// <returns>The path to this image resource as a string</returns>
        private string FindChipImageSource(Color color)
        {
            if (color == Color.Blue)
            {
                return "Resources/chipBlue.png";
            }
            else if (color == Color.Red)
            {
                return "Resources/chipRed.png";
            }
            else if (color == Color.White)
            {
                return "Resources/chipWhite.png";
            }
            else if (color == Color.Green)
            {
                return "Resources/chipGreen.png";
            }
            else if (color == Color.Black)
            {
                return "Resources/chipBlack.png";
            }

            throw new Exception("Chip Type not found!");
        }
    }
}
