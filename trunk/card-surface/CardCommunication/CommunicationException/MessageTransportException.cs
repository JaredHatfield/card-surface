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
        /// The custom message.
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportException"/> class.
        /// </summary>
        public MessageTransportException()
            : base()
        {
            this.message = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageTransportException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageTransportException(string message)
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
                    return "MessageTransport exception thrown";
                }
                else
                {
                    return "MessageTransport exception thrown: " + this.message;
                }
            }
        }
    }
}
