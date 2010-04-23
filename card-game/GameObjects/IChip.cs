// <copyright file="IChip.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interface for a single chip.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Interface for a single chip.
    /// </summary>
    public interface IChip : IPhysicalObject, IEquatable<Chip>
    {
        /// <summary>
        /// Gets the amount of the chip.
        /// </summary>
        /// <value>The amount.</value>
        int Amount
        {
            get;
        }

        /// <summary>
        /// Gets the color of the chip.
        /// </summary>
        /// <value>The color of the chip.</value>
        Color ChipColor
        {
            get;
        }
    }
}
