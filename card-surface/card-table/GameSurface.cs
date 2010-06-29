// <copyright file="GameSurface.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The version of a remote game used by the surface.</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardCommunication;

    /// <summary>
    /// The version of a remote game used from the command line.
    /// </summary>
    public class GameSurface : GameNetworkClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameSurface"/> class.
        /// </summary>
        /// <param name="clientCommunicationController">The table communication controller.</param>
        /// <param name="game">The game to join.</param>
        public GameSurface(ClientCommunicationController clientCommunicationController, ActiveGameStruct game)
            : base(clientCommunicationController, game)
        {
        }
    }
}
