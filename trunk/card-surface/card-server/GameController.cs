// <copyright file="GameController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that manages all of the game instances and the access to the game instances.</summary>
namespace CardServer
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardAccount;
    using CardGame;

    /// <summary>
    /// The controller that manages all of the game instances and the access to the game instances.
    /// </summary>
    public class GameController : IGameController
    {
        /// <summary>
        /// The list of games.
        /// </summary>
        private ObservableCollection<Game> games;

        /// <summary>
        /// The account controller.
        /// </summary>
        private AccountController accountController;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameController"/> class.
        /// </summary>
        internal GameController()
        {
            this.games = new ObservableCollection<Game>();
            this.accountController = AccountController.Instance;
        }

        /// <summary>
        /// Gets the active game count.
        /// </summary>
        /// <value>The active game count.</value>
        public int ActiveGameCount
        {
            get { return this.games.Count; }
        }

        /// <summary>
        /// Gets the current set of games.
        /// </summary>
        /// <value>The current set of games.</value>
        public ReadOnlyObservableCollection<Game> Games
        {
            get { return new ReadOnlyObservableCollection<Game>(this.games); }
        }

        /// <summary>
        /// Adds a game to the list of games.
        /// </summary>
        /// <param name="game">The game to add to the list of games.</param>
        public void AddGame(Game game)
        {
            this.games.Add(game);
        }

        /// <summary>
        /// Gets a game based off of its id.
        /// </summary>
        /// <param name="id">The unique id of the game.</param>
        /// <returns>The game requested.</returns>
        public Game GetGame(Guid id)
        {
            for (int i = 0; i < this.games.Count; i++)
            {
                if (this.games[i].Id.Equals(id))
                {
                    return this.games[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Tests to see if the password is valid for any open seat in any game.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>True if the password is valid; otherwise false.</returns>
        public bool PasswordPeek(string password)
        {
            for (int i = 0; i < this.games.Count; i++)
            {
                if (this.games[i].PasswordPeek(password))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
