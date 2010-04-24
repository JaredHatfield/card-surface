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
        /// Gets the current set of games.
        /// </summary>
        /// <value>The current set of games.</value>
        ReadOnlyObservableCollection<Game> Games
        {
            get;
        }

        /// <summary>
        /// Gets the list of available game types.
        /// </summary>
        /// <value>The list of game types.</value>
        ReadOnlyCollection<string> GameTypes
        {
            get;
        }

        /// <summary>
        /// Gets a game.
        /// </summary>
        /// <param name="id">The unique id of the game.</param>
        /// <returns>The game requested.</returns>
        Game GetGame(Guid id);

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <param name="seatPassword">The seat password.</param>
        /// <returns>The instance of the game containing the specified seat password.</returns>
        Game GetGame(string seatPassword);

        /// <summary>
        /// Adds a game so that it can be created.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        /// <param name="gameName">Name of the game.</param>
        void SubscribeGame(Type gameType, string gameName);

        /// <summary>
        /// Creates a new instance of the Game.
        /// </summary>
        /// <param name="gameName">Name of the game.</param>
        /// <returns>The id of the game that was created.</returns>
        Guid CreateGame(string gameName);

        /// <summary>
        /// Sets the action status.
        /// </summary>
        /// <param name="gameGuid">The game GUID.</param>
        /// <param name="actionSuccess">if set to <c>true</c> [action success].</param>
        void SetActionStatus(Guid gameGuid, bool actionSuccess);

        /// <summary>
        /// Tests to see if the password is valid for any open seat in any game.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>True if the password is valid; otherwise false.</returns>
        bool PasswordPeek(string password);
    }
}
