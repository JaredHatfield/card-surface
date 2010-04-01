// <copyright file="CommunicationObject.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the server uses for communication with the table.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;

    /// <summary>
    /// Object that is passed during transmission
    /// </summary>
    public class CommunicationObject
    {
        /// <summary>
        /// size of buffer.
        /// </summary>
        public const int BufferSize = 1024;

        /// <summary>
        /// socket that is receiving communication.
        /// </summary>
        private Socket workSocket = null;

        /// <summary>
        /// the buffer.
        /// </summary>
        private byte[] buffer = new byte[BufferSize];

        /// <summary>
        /// data in the communication.
        /// </summary>
        private byte[] data = null;

        /// <summary>
        /// Gets or sets the work socket.
        /// </summary>
        /// <value>The work socket.</value>
        public Socket WorkSocket
        {
            get { return this.workSocket; }
            set { this.workSocket = value; }
        }

        /// <summary>
        /// Gets or sets the buffer.
        /// </summary>
        /// <value>The buffer.</value>
        public byte[] Buffer
        {
            get { return this.buffer; }
            set { this.buffer = value; }
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data in the object.</value>
        public byte[] Data
        {
            get { return this.data; }
            set { this.data = value; }
        }
    }
}
