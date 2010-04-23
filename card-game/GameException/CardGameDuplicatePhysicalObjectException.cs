// <copyright file="CardGameDuplicatePhysicalObjectException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardGame when a duplicate PhysicalObject is attempted to be created.</summary>
namespace CardGame.GameException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by CardGame when a duplicate PhysicalObject is attempted to be created.
    /// </summary>
    public class CardGameDuplicatePhysicalObjectException : CardGameException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardGameDuplicatePhysicalObjectException"/> class.
        /// </summary>
        public CardGameDuplicatePhysicalObjectException()
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
                return "A duplicate PhysicalObject Guid was attempted to be created.";
            }
        }
    }
}
