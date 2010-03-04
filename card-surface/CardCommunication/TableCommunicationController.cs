﻿// <copyright file="TableCommunicationController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that the table uses for communication with the server.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The controller that the table uses for communication with the server.
    /// </summary>
    public class TableCommunicationController : CommunicationController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableCommunicationController"/> class.
        /// </summary>
        public TableCommunicationController()
            : base()
        {
        }
    }
}