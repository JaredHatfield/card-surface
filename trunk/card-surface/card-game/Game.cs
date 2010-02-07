// <copyright file="Game.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A generic card game that is extended to implement a specific game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A generic card game that is extended to implement a specific game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// 
        /// </summary>
        private PlayingArea gamingArea;

        /// <summary>
        /// Gets the gaming area.
        /// </summary>
        /// <value>The gaming area.</value>
        public PlayingArea GamingArea
        {
            get { return this.gamingArea; }
        }
    }
}
