// <copyright file="CardCommunicationException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by CardCommunication.</summary>
namespace CardCommunication.CommunicationException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Card Communication Exception
    /// </summary>
    public class CardCommunicationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardCommunicationException"/> class.
        /// </summary>
        public CardCommunicationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardCommunicationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CardCommunicationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardCommunicationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        internal CardCommunicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
