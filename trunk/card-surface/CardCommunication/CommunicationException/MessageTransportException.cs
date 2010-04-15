// <copyright file="MessageTransportException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by MessageTransport.</summary>
namespace CardCommunication.CommunicationException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Message Transport Exception
    /// </summary>
    public class MessageTransportException : CardCommunicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportException"/> class.
        /// </summary>
        public MessageTransportException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageTransportException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        internal MessageTransportException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
