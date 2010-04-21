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
        /// The object image.
        /// </summary>
        private Image objectImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceChip"/> class.
        /// </summary>
        /// <param name="id">The chip's id.</param>
        /// <param name="amount">The chip's amount.</param>
        /// <param name="chipColor">The chip's color.</param>
        protected internal SurfaceChip(Guid id, int amount, Color chipColor)
            : base(id, amount, chipColor)
        {
            // TODO: This should load the image
            // this.objectImage = Image.FromFile("pack://application:,,,/Resources/chipGreen.png");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceChip"/> class.
        /// </summary>
        /// <param name="amount">The chip's amount.</param>
        /// <param name="chipColor">The color of the chip.</param>
        protected internal SurfaceChip(int amount, Color chipColor)
            : base(amount, chipColor)
        {
            // TODO: This should load the image
            // this.objectImage = Image.FromFile("pack://application:,,,/Resources/chipGreen.png");
        }

        /// <summary>
        /// Gets the object image.
        /// </summary>
        /// <value>The object image.</value>
        internal Image ObjectImage
        {
            get
            {
                return this.objectImage;
            }
        }
    }
}
