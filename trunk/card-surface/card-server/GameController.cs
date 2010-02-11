﻿// <copyright file="GameController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that manages all of the game instances and the access to the game instances.</summary>
namespace CardServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// The controller that manages all of the game instances and the access to the game instances.
    /// </summary>
    public class GameController : IGameController
    {
        /// <summary>
        /// The list of games.
        /// </summary>
        private List<Game> games;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class.
        /// </summary>
        internal GameController()
        {
            this.games = new List<Game>();
        }

        /// <summary>
        /// Gets the active game count.
        /// </summary>
        /// <value>The active game count.</value>
        public int ActiveGameCount
        {
            get { return this.games.Count; }
        }
    }
}
