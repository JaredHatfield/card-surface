// <copyright file="CardGameException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardGame.</summary>
namespace CardGame.GameException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by CardGame.
    /// </summary>
    public class CardGameException : Exception
    {
        /// <summary>
        /// The custom message.
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardGameException"/> class.
        /// </summary>
        public CardGameException()
            : base()
        {
            this.message = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardGameException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CardGameException(string message)
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
                if (this.message == string.Empty)
                {
                    return "CardGame exception thrown";
                }
                else
                {
                    return "CardGame exception thrown: " + this.message;
                }
            }
        }
    }
}
