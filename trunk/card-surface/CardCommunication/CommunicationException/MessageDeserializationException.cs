// <copyright file="MessageDeserializationException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by MessageDeserializationException.</summary>
namespace CardCommunication.CommunicationException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Message Deserialization Exception
    /// </summary>
    public class MessageDeserializationException : CardCommunicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDeserializationException"/> class.
        /// </summary>
        public MessageDeserializationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDeserializationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageDeserializationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDeserializationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        internal MessageDeserializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}