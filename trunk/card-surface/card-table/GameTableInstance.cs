﻿// <copyright file="GameTableInstance.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The game instance on the table.</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Windows.Threading;
    using CardCommunication;
    using CardGame;

    /// <summary>
    /// The game instance on the table.
    /// </summary>
    internal class GameTableInstance
    {
        /// <summary>
        /// The singleton instance of GameTableInstance
        /// </summary>
        private static GameTableInstance instance = new GameTableInstance();

        /// <summary>
        /// The TableCommunicationController that coordinates the communication to the server.
        /// </summary>
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// The game that is currently being played on the table.
        /// </summary>
        private Game game;

        /// <summary>
        /// The CardTableWindow that contains the currently playable game.
        /// </summary>
        private CardTableWindow gameWindow;

        /// <summary>
        /// Prevents a default instance of the <see cref="GameTableInstance"/> class from being created.
        /// </summary>
        private GameTableInstance()
        {
            this.game = null;
            this.tableCommunicationController = new TableCommunicationController();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static GameTableInstance Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Gets the communication controller.
        /// </summary>
        /// <value>The communication controller.</value>
        public TableCommunicationController CommunicationController
        {
            get { return this.tableCommunicationController; }
        }

        /// <summary>
        /// Gets the game.
        /// </summary>
        /// <value>The current game.</value>
        public Game CurrentGame
        {
            get { return this.game; }
        }

        /// <summary>
        /// Gets the game window.
        /// </summary>
        /// <value>The game window.</value>
        public CardTableWindow GameWindow
        {
            get { return this.gameWindow; }
        }

        /// <summary>
        /// Creates the new game.
        /// </summary>
        /// <param name="game">The game to create.</param>
        public void CreateNewGame(Game game)
        {
            if (this.game == null)
            {
                this.game = game;

                /* Create the CardTableWindow after we install the game because the window requires binding to game properties. */
                this.gameWindow = new CardTableWindow();

                /* Transition ownership of handling GameUpdates to GameTableInstance. */
                Debug.WriteLine("GameTableInstance: Accepted responsible for processing incoming GameState updates.");
                this.tableCommunicationController.OnUpdateGameState += new TableCommunicationController.UpdateGameStateHandler(this.OnUpdateGameState);
            }
            else
            {
                Debug.WriteLine("GameTableInstance: A game has already been created on this table instance!");
            }
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="game">The new game.</param>
        public void OnUpdateGameState(Game game)
        {
            Debug.WriteLine("GameTableInstance: Updating the current game...");
            this.game = game;
        }
    }
}
