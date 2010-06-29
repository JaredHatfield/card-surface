// <copyright file="CommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The base class for the communication controller.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Runtime.Serialization.Formatters.Soap;
    using System.Text;
    using System.Threading;
    using System.Xml;
    using System.Xml.Serialization;
    using CardGame;
    using CommunicationException;
    using Messages;

    /// <summary>
    /// The base class for the communication controller.
    /// Create sockets using networkstreams to communicate to certain ports.
    /// Use listeners to handle incoming xml.  Write on command.
    /// </summary>
    public abstract class CommunicationController
    {
        /// <summary>
        /// The Port number of the listening socket.
        /// </summary>
        protected const int ServerListenerPortNumber = 30565;

        /// <summary>
        /// Header for the Game state message.
        /// </summary>
        protected const string HeaderGame = "GAME";

        /// <summary>
        /// Message header.
        /// </summary>
        protected const string HeaderMessage = "HEAD";

        /// <summary>
        /// Header for an asynchronous game stare message.
        /// </summary>
        protected const string HeaderPush = "PUSH";

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public abstract void Close(object sender, EventArgs args);

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        protected void AddParameter(ref Collection<ParameterStruct> parameters, string name, string value)
        {
            ParameterStruct parameter = new ParameterStruct();

            parameter.Name = name;
            parameter.Value = value;

            parameters.Add(parameter);
        }
    }
}
