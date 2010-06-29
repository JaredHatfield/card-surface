// <copyright file="GameCommandLine.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The version of a remote game used from the command line.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardCommunication;

    /// <summary>
    /// The version of a remote game used from the command line.
    /// </summary>
    internal class GameCommandLine : GameNetworkClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameCommandLine"/> class.
        /// </summary>
        /// <param name="clientCommunicationController">The table communication controller.</param>
        /// <param name="game">The game to join.</param>
        internal GameCommandLine(ClientCommunicationController clientCommunicationController, ActiveGameStruct game)
            : base(clientCommunicationController, game)
        {
        }
    }
}
