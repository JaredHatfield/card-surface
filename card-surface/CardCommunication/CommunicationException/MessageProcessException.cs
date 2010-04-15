// <copyright file="MessageProcessException.cs" company="University of Louisville Speed School of Engineering">
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
    /// Message Process Exception
    /// </summary>
    public class MessageProcessException : CardCommunicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessException"/> class.
        /// </summary>
        public MessageProcessException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageProcessException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        internal MessageProcessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
