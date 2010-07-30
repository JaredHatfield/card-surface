// <copyright file="ActiveGameStruct.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A struct for an active game.</summary>
namespace CardCommunication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A struct for an active game.
    /// </summary>
    public struct ActiveGameStruct
    {
        /// <summary>
        /// The type of game.
        /// </summary>
        public string GameType;

        /// <summary>
        /// The string display of the Game.
        /// </summary>
        public string DisplayString;

        /// <summary>
        /// The guid of the game.
        /// </summary>
        public Guid Id;

        /// <summary>
        /// The string representation of the players.
        /// </summary>
        public string Players;
    }
}
