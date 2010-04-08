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
        /// The custom message.
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessException"/> class.
        /// </summary>
        public MessageProcessException()
            : base()
        {
            this.message = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageProcessException(string message)
            : base()
        {
            this.message = message;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        /// </returns>
        public override string Message
        {
            get
            {
                if (this.message == string.Empty)
                {
                    return "MessageProcess exception thrown";
                }
                else
                {
                    return "MessageProcess exception thrown: " + this.message;
                }
            }
        }
    }
}
