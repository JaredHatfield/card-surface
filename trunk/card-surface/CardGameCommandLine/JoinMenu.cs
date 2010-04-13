﻿// <copyright file="JoinMenu.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The Join Menu of the application.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardCommunication;
    using CardServer;
    using GameBlackjack;
    using GameFreeplay;

    /// <summary>
    /// The Join Menu of the application.
    /// </summary>
    internal class JoinMenu : BaseMenu
    {
        /// <summary>
        /// This should not be necessary, but lets just go with it.
        /// </summary>
        private ServerController serverController;

        /// <summary>
        /// The communication controller that can access the server.
        /// </summary>
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinMenu"/> class.
        /// </summary>
        public JoinMenu()
        {
            Console.WriteLine("Where would you like to connect to?");
            Console.WriteLine("Enter 'auto' to have a server created for you or enter the IP address of the server to connect to it.");
            bool loop = true;
            string input = string.Empty;
            while (!input.Equals("exit", StringComparison.CurrentCultureIgnoreCase) && loop)
            {
                Console.Write(" > ");
                input = Console.ReadLine();
                if (input.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.serverController = new ServerController();
                    this.tableCommunicationController = new TableCommunicationController();
                    this.tableCommunicationController.OnUpdateGameList += new TableCommunicationController.UpdateGameListHandler(this.DoNothing);
                    this.tableCommunicationController.SendRequestGameListMessage();
                    loop = false;
                    this.MainLoop();
                }
                else
                {
                    try
                    {
                        this.tableCommunicationController = new TableCommunicationController(input);
                        loop = false;
                        this.MainLoop();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something very bad happened!");
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// The main loop for joining a game.s
        /// </summary>
        private void MainLoop()
        {
            string input = string.Empty;
            while (!input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine("Commands: list");
                Console.Write(" > ");
                input = Console.ReadLine();
                if (input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    // We are all done.
                }
                else if (input.Equals("list", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.GameList();
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                    this.PrompForEnter();
                }

                // TODO: We need to add some more commands.
            }
        }

        /// <summary>
        /// Does the nothing.
        /// </summary>
        /// <param name="games">The games.</param>
        private void DoNothing(Collection<string> games)
        {
        }

        /// <summary>
        /// Print out the list of possible game types and prompt the user to join one of the games.
        /// </summary>
        private void GameList()
        {
            string selected = string.Empty;

            // Get the list of games from the server.  Danger, danger!
            try
            {
                Collection<string> games = this.tableCommunicationController.SendRequestGameListMessage();

                // Print out that list that we hopefully retreived.
                Console.WriteLine("Games:");
                for (int i = 0; i < games.Count; i++)
                {
                    Console.WriteLine(i + ") " + games[i]);
                }

                string input = string.Empty;
                while (!input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.Write(" > ");
                    input = Console.ReadLine();
                    try
                    {
                        int selection = Int32.Parse(input);
                        if (selection >= games.Count)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        selected = games[selection];
                        break;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid selection");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went terribly wrong!");
                Console.WriteLine(e.ToString());
                return;
            }

            this.GameTypeMenu(selected);
        }

        /// <summary>g
        /// Get the list of active games and prompt to join one of those or add a new game and join that.
        /// </summary>
        /// <param name="gameName">Name of the game.</param>
        private void GameTypeMenu(string gameName)
        {
            // TODO: I really wanted a synchronous call, but it doesn't exist yet! :(
            /*
            try
            {
                Collection<string> games = this.tableCommunicationController.SendRequestExistingGames(gameName);

                for (int i = 0; i < games; i++)
                {
                    Console.WriteLine(i + ") " + games[i]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went terribly wrong!");
                Console.WriteLine(e.ToString());
                return;
            }
             */

            Console.WriteLine("The list of active games will appear here for " + gameName);
            this.PrompForEnter();
        }
    }
}
