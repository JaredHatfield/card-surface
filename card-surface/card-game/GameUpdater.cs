// <copyright file="GameUpdater.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Updates an instance of a game based on a GameMessage.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Updates an instance of a game based on a GameMessage.
    /// </summary>
    public class GameUpdater
    {
        /// <summary>
        /// The game that will be updated.
        /// </summary>
        private Game game;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameUpdater"/> class.
        /// </summary>
        /// <param name="game">The game that will be updated.</param>
        public GameUpdater(Game game)
        {
            this.game = game;
        }

        /// <summary>
        /// Updates the specified game message.
        /// </summary>
        /// <param name="gameMessage">The game message.</param>
        public void Update(Game gameMessage)
        {
            // TODO: Implement Me!
        }
    }
}
