// <copyright file="SocketBindingException.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An exception thrown by SocketBindingException.</summary>
namespace CardCommunication.CommunicationException
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Socket Binding Exception
    /// </summary>
    public class SocketBindingException : CardCommunicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SocketBindingException"/> class.
        /// </summary>
        public SocketBindingException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketBindingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SocketBindingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketBindingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        internal SocketBindingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
