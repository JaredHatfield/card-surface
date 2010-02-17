﻿// <copyright file="GameController.cs" company="University of Louisville Speed School of Engineering">
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

            // TODO: This needs to be changed to an instance of the Singleton class!
            this.accountController = new AccountController();
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
    }
}