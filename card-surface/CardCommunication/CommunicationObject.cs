// <copyright file="CommunicationObject.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the server uses for communication with the table.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
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
        /// whether the message is a game state
        /// </summary>
        private bool gameState = false;

        /// <summary>
        /// the Type of message.
        /// </summary>
        private string messageType;

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
        /// whether this is the first segment of the message.
        /// </summary>
        private bool firstKB = true;

        /// <summary>
        /// The remote IPEndpoint that went the message.
        /// </summary>
        private IPAddress remoteIPAddress;

        /// <summary>
        /// Gets or sets a value indicating whether [game state].
        /// </summary>
        /// <value><c>true</c> if [game state]; otherwise, <c>false</c>.</value>
        public bool GameState
        {
            get { return this.gameState; }
            set { this.gameState = value; }
        }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        public string MessageType
        {
            get { return this.messageType; }
            set { this.messageType = value; }
        }

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

        /// <summary>
        /// Gets or sets a value indicating whether [first KB].
        /// </summary>
        /// <value><c>true</c> if [first KB]; otherwise, <c>false</c>.</value>
        public bool FirstKB
        {
            get { return this.firstKB; }
            set { this.firstKB = value; }
        }

        /// <summary>
        /// Gets or sets the remote IP end point.
        /// </summary>
        /// <value>The remote IP end point.</value>
        public IPAddress RemoteIPAddress
        {
            get { return this.remoteIPAddress; }
            set { this.remoteIPAddress = value; }
        }
    }
}
