// <copyright file="CardGamePhysicalObjectNotFoundException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardGame when a PhysicalObject is not found.</summary>
namespace CardGame.GameException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by CardGame when a PhysicalObject is not found.
    /// </summary>
    public class CardGamePhysicalObjectNotFoundException : CardGameException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardGamePhysicalObjectNotFoundException"/> class.
        /// </summary>
        public CardGamePhysicalObjectNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                return "PhysicalObject could not be found exception";
            }
        }
    }
}
