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
    using CardServer;

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
            this.serverController = new ServerController();
            this.tableCommunicationController = new TableCommunicationController();
            this.tableCommunicationController.OnUpdateGameList += new TableCommunicationController.UpdateGameListHandler(this.DoNothing);
            this.tableCommunicationController.SendRequestGameListMessage();

            string input = string.Empty;
            while (!input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Clear();
                Console.WriteLine("Commands: games");
                Console.Write(" > ");
                input = Console.ReadLine();
                if (input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    // We are all done.
                }
                else if (input.Equals("games", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Get the list of games from the server.  Danger, danger!
                    Collection<string> games = this.tableCommunicationController.SendRequestGameListMessage();

                    // Print out that list that we hopefully retreived.
                    Console.WriteLine("Games:");
                    for (int i = 0; i < games.Count; i++)
                    {
                        Console.WriteLine(games[i]);
                    }

                    this.PrompForEnter();
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                    this.PrompForEnter();
                }
            }
        }

        /// <summary>
        /// Does the nothing.
        /// </summary>
        /// <param name="games">The games.</param>
        private void DoNothing(Collection<string> games)
        {
        }
    }
}
