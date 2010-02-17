// <copyright file="IGameController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The interface to the GameController used by other libraries to access instances of the Game class.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The interface to the GameController used by other libraries to access instances of the Game class.
    /// </summary>
    public interface IGameController
    {
        /// <summary>
        /// Gets the active game count.
        /// </summary>
        /// <value>The active game count.</value>
        int ActiveGameCount
        {
            get;
        }

        /// <summary>
        /// Adds a game.
        /// </summary>
        /// <param name="game">The game to add.</param>
        void AddGame(Game game);

        /// <summary>
        /// Gets a game.
        /// </summary>
        /// <param name="id">The unique id of the game.</param>
        /// <returns>The game requested.</returns>
        Game GetGame(Guid id);
    }
}
