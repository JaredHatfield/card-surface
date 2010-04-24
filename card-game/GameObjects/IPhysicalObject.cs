// <copyright file="IPhysicalObject.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Interface for a physical object that is manipulated as part of the game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Interface for a physical object that is manipulated as part of the game.
    /// </summary>
    public interface IPhysicalObject : IComparable, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="PhysicalObject"/> is moveable.
        /// </summary>
        /// <value><c>true</c> if moveable; otherwise, <c>false</c>.</value>
        bool Moveable
        {
            get;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The unique id.</value>
        Guid Id
        {
            get;
        }
    }
}
