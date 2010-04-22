// <copyright file="TableManager.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The Singleton TableManager.</summary>
namespace CardTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using CardCommunication;
    using CardGame.GameFactory;
    using CardTable.GameFactory;

    /// <summary>
    /// The Singleton TableManager.
    /// </summary>
    public class TableManager
    {
        /// <summary>
        /// The Singleton instance of TableManager.
        /// </summary>
        private static TableManager instance;

        /// <summary>
        /// The IP address of the server that this table is connected to
        /// </summary>
        private string serverIpAddress;

        /// <summary>
        /// The TableCommunicationController.
        /// </summary>
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// The current game being played on this table.
        /// </summary>
        private GameSurface currentGame;

        /// <summary>
        /// The CardTableWindow where the currentGame for this table is being played
        /// </summary>
        private CardTableWindow currentGameWindow;

        /// <summary>
        /// The window responsible for allowing the user to select a game
        /// </summary>
        private GameSelection gameSelectionWindow;

        /// <summary>
        /// Prevents a default instance of the <see cref="TableManager"/> class from being created.
        /// </summary>
        private TableManager()
        {
            this.tableCommunicationController = new TableCommunicationController();
            this.InitializeFactories();
            this.currentGame = null;
            this.serverIpAddress = Dns.GetHostName();

            this.gameSelectionWindow = new GameSelection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableManager"/> class.
        /// </summary>
        /// <param name="ip">The ip address to connect to.</param>
        private TableManager(string ip)
        {
            /* TODO: We should verify that the IP address passed to this constructor is valid! */
            this.tableCommunicationController = new TableCommunicationController(ip);
            this.InitializeFactories();
            this.currentGame = null;
            this.serverIpAddress = ip;
            this.gameSelectionWindow = new GameSelection();
        }

        /// <summary>
        /// Gets the game surface.
        /// </summary>
        /// <value>The game surface.</value>
        public GameSurface CurrentGame
        {
            get { return this.currentGame; }
        }

        /// <summary>
        /// Gets the current game window.
        /// </summary>
        /// <value>The current game window.</value>
        public CardTableWindow CurrentGameWindow
        {
            get { return this.currentGameWindow; }
        }

        /// <summary>
        /// Gets the game selection window.
        /// </summary>
        /// <value>The game selection window.</value>
        public GameSelection GameSelectionWindow
        {
            get { return this.gameSelectionWindow; }
        }

        /// <summary>
        /// Gets the server address.
        /// </summary>
        /// <value>The server address.</value>
        public string ServerAddress
        {
            get { return this.serverIpAddress; }
        }

        /// <summary>
        /// Gets the table communication controller.
        /// </summary>
        /// <value>The table communication controller.</value>
        internal TableCommunicationController TableCommunicationController
        {
            get { return this.tableCommunicationController; }
        }

        /// <summary>
        /// Creates the game.
        /// </summary>
        /// <param name="game">The new game.</param>
        public void CreateGame(GameSurface game)
        {
            if (this.currentGame == null)
            {
                this.currentGame = game;
                this.currentGameWindow = new CardTableWindow(this.currentGame);
            }
        }

        /// <summary>
        /// Instances this instance.
        /// </summary>
        /// <returns>The instance of TableManager.</returns>
        internal static TableManager Instance()
        {
            if (TableManager.instance == null)
            {
                throw new Exception("TableManager not Initialized!");
            }

            return TableManager.instance;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>The instance of TableManager.</returns>
        internal static TableManager Initialize()
        {
            if (TableManager.instance == null)
            {
                TableManager.instance = new TableManager();
                return TableManager.instance;
            }

            throw new Exception("TableManager: TableManager already Initialized!");
        }

        /// <summary>
        /// Initializes the specified ip.
        /// </summary>
        /// <param name="ip">The ip address to connect to.</param>
        /// <returns>The instance of TableManager.</returns>
        internal static TableManager Initialize(string ip)
        {
            if (TableManager.instance == null)
            {
                TableManager.instance = new TableManager(ip);
                return TableManager.instance;
            }

            throw new Exception("TableManager: TableManager already Initialized!");
        }

        /// <summary>
        /// Initializes the object factories for this table.
        /// </summary>
        private void InitializeFactories()
        {
            // TODO: We need to create new chip and card factories that make surface elements!!!
            PhysicalObjectFactory.SubscribeCardFactory(SurfaceCardFactory.Instance());
            PhysicalObjectFactory.SubscribeChipFactory(SurfaceChipFactory.Instance());
            PhysicalObjectFactory factory = PhysicalObjectFactory.Instance();
        }
    }
}
