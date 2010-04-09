// <copyright file="CardGameFactoryException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardGame when a problem occurs in the factory.</summary>
namespace CardGame.GameException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by CardGame when a problem occurs in the factory.
    /// </summary>
    public class CardGameFactoryException : CardGameException
    {
        /// <summary>
        /// Additional details about the exception.
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardGameFactoryException"/> class.
        /// </summary>
        public CardGameFactoryException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardGameFactoryException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CardGameFactoryException(string message)
            : base()
        {
            this.message = message;
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
                if (this.message.Length == 0)
                {
                    return "An error occured with the PhysicalObjectFactory.";
                }
                else
                {
                    return "Factory Exception: " + this.message;
                }
            }
        }
    }
}
