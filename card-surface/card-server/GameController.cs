// <copyright file="GameController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The controller that manages all of the game instances and the access to the game instances.</summary>
namespace CardServer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardAccount;
    using CardGame;
    using CardGame.GameException;

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
        /// The array of games that can be started.
        /// </summary>
        private Dictionary<string, Type> availableGames;

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
            this.availableGames = new Dictionary<string, Type>();
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
        /// Gets the list of available game types.
        /// </summary>
        /// <value>The list of game types.</value>
        public ReadOnlyCollection<string> GameTypes
        {
            get
            {
                // TODO: Is this really the best method?
                Collection<string> games = new Collection<string>();
                foreach (var pair in this.availableGames)
                {
                    games.Add(pair.Key);
                }

                return new ReadOnlyCollection<string>(games);
            }
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

            throw new CardGameGameNotFoundException();
        }

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <param name="seatPassword">The seat password.</param>
        /// <returns>
        /// The instance of the game containing the specified seat password.
        /// </returns>
        public Game GetGame(string seatPassword)
        {
            for (int i = 0; i < this.games.Count; i++)
            {
                if (this.games[i].PasswordPeek(seatPassword))
                {
                    return this.games[i];
                }
            }

            throw new CardGameGameNotFoundException();
        }

        /// <summary>
        /// Adds a game so that it can be created.
        /// </summary>
        /// <param name="gameType">Type of the game.</param>
        /// <param name="gameName">Name of the game.</param>
        public void SubscribeGame(Type gameType, string gameName)
        {
            // TODO: Maybe we should check to see if the type passed inherited Game.
            this.availableGames.Add(gameName, gameType);
        }

        /// <summary>
        /// Creates a new instance of the Game.
        /// </summary>
        /// <param name="gameName">Name of the game.</param>
        /// <returns>The id of the game that was created.</returns>
        public Guid CreateGame(string gameName)
        {
            // TODO: There is the potential for lots of Exceptions to be thrown in the following code.
            Type type = this.availableGames[gameName];
            Game game = Activator.CreateInstance(type) as Game;
            this.games.Add(game);
            return game.Id;
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
