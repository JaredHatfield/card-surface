// <copyright file="JoinMenu.cs" company="University of Louisville Speed School of Engineering">
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
    using CardGame;
    using CardGame.GameFactory;
    using CardServer;
    using GameBlackjack;
    using GameFreeplay;

    /// <summary>
    /// The Join Menu of the application.
    /// </summary>
    internal class JoinMenu : BaseMenu
    {
        /// <summary>
        /// The communication controller that can access the server.
        /// </summary>
        private TableCommunicationController tableCommunicationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinMenu"/> class.
        /// </summary>
        public JoinMenu()
        {
            Console.WriteLine("Enter the IP address of the server you want to connect to:");
            bool loop = true;
            string input = string.Empty;
            while (!input.Equals("exit", StringComparison.CurrentCultureIgnoreCase) && loop)
            {
                Console.Write(" > ");
                input = Console.ReadLine();
                if (input.Equals("local", StringComparison.CurrentCultureIgnoreCase) || input.Equals("localhost", StringComparison.CurrentCultureIgnoreCase) || input.Length == 0)
                {
                    // Start communicating with the server
                    this.tableCommunicationController = new TableCommunicationController();

                    // Set up the factory!
                    PhysicalObjectFactory.SubscribeCardFactory(CardFactory.Instance());
                    PhysicalObjectFactory.SubscribeChipFactory(ChipFactory.Instance());
                    PhysicalObjectFactory factory = PhysicalObjectFactory.Instance();
                    loop = false;
                    this.GameList();
                }
                else
                {
                    try
                    {
                        // Start communicating with the server
                        this.tableCommunicationController = new TableCommunicationController(input);

                        // Set up the factory!
                        PhysicalObjectFactory.SubscribeCardFactory(CardFactory.Instance());
                        PhysicalObjectFactory.SubscribeChipFactory(ChipFactory.Instance());
                        PhysicalObjectFactory factory = PhysicalObjectFactory.Instance();
                        loop = false;
                        this.GameList();
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
        /// Print out the list of possible game types and prompt the user to join one of the games.
        /// </summary>
        private void GameList()
        {
            Console.Clear();
            Console.WriteLine("Choose from the list of available game types.");
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

                // Read input from the keyboard
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

                if (input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went terribly wrong!");
                Console.WriteLine(e.ToString());
                Console.ReadLine();
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
            Console.Clear();
            Console.WriteLine("Choose from the list of available " + gameName + " games.");
            ActiveGameStruct selected = new ActiveGameStruct();

            try
            {
                Collection<ActiveGameStruct> games = this.tableCommunicationController.SendRequestExistingGames(gameName);

                for (int i = 0; i < games.Count; i++)
                {
                    Console.WriteLine(i + ") " + games[i].DisplayString);
                }

                // Read input from the keyboard
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

                if (input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went terribly wrong!");
                Console.WriteLine(e.ToString());
                Console.ReadLine();
                return;
            }

            // TODO: REMOVE THIS TRY STATEMENT!!!
            // THIS REALLY, REALLY, REALLY SHOULD NOT BE HERE!
            try
            {
                GameCommandLine game = new GameCommandLine(this.tableCommunicationController, selected);
                GameMenu gameMenu = new GameNetworkMenu(game, this.tableCommunicationController);
                gameMenu.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("We tried, we failed...");
                Console.WriteLine(e.ToString());
            }

            this.PrompForEnter();
        }
    }
}
