// <copyright file="ClientObject.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An object that contains a Table guid, Game guid, and IPEndPoint of a Client.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;

    /// <summary>
    /// Object that contains information about a client.
    /// </summary>
    public class ClientObject
    {
        /// <summary>
        /// Guid of the Game the Client is connected to .
        /// </summary>
        private Guid game;

        /// <summary>
        /// IPEndPoint of the Client.
        /// </summary>
        private IPEndPoint clientIPEndPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientObject"/> class.
        /// </summary>
        /// <param name="ip">The ip end point.</param>
        public ClientObject(IPEndPoint ip)
        {
            this.clientIPEndPoint = ip;
        }

        /// <summary>
        /// Gets or sets the game guid.
        /// </summary>
        /// <value>The game guid.</value>
        public Guid Game
        {
            get { return this.game; }
            set { this.game = value; }
        }

        /// <summary>
        /// Gets or sets the client IP end point.
        /// </summary>
        /// <value>The client IP end point.</value>
        public IPEndPoint ClientIPEndPoint
        {
            get { return this.clientIPEndPoint; }
            set { this.clientIPEndPoint = value; }
        }
    }
}
