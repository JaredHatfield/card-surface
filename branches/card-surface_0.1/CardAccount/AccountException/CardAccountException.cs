// <copyright file="CardAccountException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardAccountException.</summary>
namespace CardAccount.AccountException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An exception thrown by CardAccountException.
    /// </summary>
    public class CardAccountException : Exception
    {
        /// <summary>
        /// The custom message.
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardAccountException"/> class.
        /// </summary>
        public CardAccountException()
            : base()
        {
            this.message = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardAccountException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CardAccountException(string message)
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
                    return "CardAccount exception thrown";
                }
                else
                {
                    return "CardAccount exception thrown: " + this.message;
                }
            }
        }
    }
}
