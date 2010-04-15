// <copyright file="MainMenu.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The Main Menu of the application.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardServer;
    using GameBlackjack;
    using GameFreeplay;

    /// <summary>
    /// The Main Menu of the application.
    /// </summary>
    internal class MainMenu : BaseMenu
    {
        /// <summary>
        /// The copy of the server that is running.
        /// </summary>
        private ServerController serverController;

        /// <summary>
        /// Since we are in local mode, we are ok with having access to the controller.
        /// </summary>
        private GameController gameController;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenu"/> class.
        /// </summary>
        public MainMenu()
        {
            // Set the server up and teach it Blackjack
            this.serverController = new ServerController();
            this.gameController = GameController.Instance();

            // Keep prompting until the user decides to exit
            string input = string.Empty;
            while (!input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine("Enter a number selection or type exit when you are finished");
                Console.WriteLine("0) Create Game");
                for (int i = 0; i < this.gameController.Games.Count; i++)
                {
                    Console.WriteLine((i + 1) + ") " + this.gameController.Games[i].Name);
                }

                Console.Write(" > ");
                input = Console.ReadLine();
                this.ParseCommand(input);
            }
        }

        /// <summary>
        /// Parses the command.
        /// </summary>
        /// <param name="command">The command.</param>
        private void ParseCommand(string command)
        {
            if (command.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("Goodbye!");
                return;
            }
            else
            {
                // Get the input
                int selection;
                try
                {
                    selection = Int32.Parse(command);
                }
                catch
                {
                    Console.WriteLine("Numeric input required");
                    this.PrompForEnter();
                    return;
                }

                // Deal with the input
                if (selection == 0)
                {
                    this.NewGame();
                    return;
                }
                else if (selection <= this.gameController.Games.Count)
                {
                    selection--;
                    GameMenu gameMenu = new GameMenu(this.gameController.Games[selection]);
                    gameMenu.Start();
                    return;
                }
                else
                {
                    Console.WriteLine("Selection out of range");
                    this.PrompForEnter();
                    return;
                }
            }
        }

        /// <summary>
        /// Create a new game.
        /// </summary>
        private void NewGame()
        {
            Console.Clear();
            Console.WriteLine("Enter a selection from below or enter to go back");
            for (int i = 0; i < this.gameController.GameTypes.Count; i++)
            {
                Console.WriteLine((i + 1) + ") " + this.gameController.GameTypes[i]);
            }

            Console.Write(" >");

            // Get the input
            string input = Console.ReadLine();
            int selection;
            try
            {
                selection = Int32.Parse(input);
                selection--;
            }
            catch
            {
                Console.WriteLine("Non-numeric input");
                this.PrompForEnter();
                return;
            }

            // Process the input
            if (selection < this.gameController.GameTypes.Count)
            {
                this.gameController.CreateGame(this.gameController.GameTypes[selection]);
                return;
            }
            else
            {
                Console.WriteLine("Selection out of range");
                this.PrompForEnter();
                return;
            }
        }
    }
}
