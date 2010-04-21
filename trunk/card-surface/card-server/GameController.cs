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
    using CardGame.GameFactory;
    using GameBlackjack;
    using GameFreeplay;

    /// <summary>
    /// The controller that manages all of the game instances and the access to the game instances.
    /// </summary>
    public class GameController : IGameController
    {
        /// <summary>
        /// The singleton instance of GameController.
        /// </summary>
        private static GameController instance;

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
        /// Prevents a default instance of the GameController class from being created.
        /// </summary>
        private GameController()
        {
            // We need to set up the PhysicalObjectFactory before we can do anything
            PhysicalObjectFactory.SubscribeCardFactory(CardFactory.Instance());
            PhysicalObjectFactory.SubscribeChipFactory(ChipFactory.Instance());
            PhysicalObjectFactory factory = PhysicalObjectFactory.Instance();

            // Set up everything else
            this.games = new ObservableCollection<Game>();
            this.availableGames = new Dictionary<string, Type>();
            this.accountController = AccountController.Instance;

            // Add the various game types
            this.SubscribeGame(typeof(Blackjack), "Blackjack");
            this.SubscribeGame(typeof(Freeplay), "Freeplay");
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
            game.PlayerLeaveGame += new Game.PlayerLeaveGameEventHandler(this.PlayerLeave);
            game.PlayerJoinGame += new Game.PlayerJoinGameEventHandler(this.PlayerJoin);
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

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Close(object sender, EventArgs args)
        {
            // TODO: Move all of the money back into the players class.
        }

        /// <summary>
        /// Get a singleton instance of the GameController.
        /// </summary>
        /// <returns>An instance of the GameController.</returns>
        internal static GameController Instance()
        {
            if (GameController.instance == null)
            {
                GameController.instance = new GameController();
            }

            return GameController.instance;
        }

        /// <summary>
        /// Players the left the game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CardGame.PlayerLeaveGameEventArgs"/> instance containing the event data.</param>
        private void PlayerLeave(object sender, PlayerLeaveGameEventArgs e)
        {
            GameAccount account = this.accountController.LookUpUser(e.Username);
            account.BalanceChange(e.Money);
        }

        /// <summary>
        /// Players the join.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CardGame.PlayerJoinGameEventArgs"/> instance containing the event data.</param>
        private void PlayerJoin(object sender, PlayerJoinGameEventArgs e)
        {
        }
    }
}
