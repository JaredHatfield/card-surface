// <copyright file="Program.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A command line application for using the CardGame classes.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using CardCommunication;
    using CardCommunication.Messages;
    using CardGame;
    using CardGame.GameException;
    using CardGame.GameFactory;
    using CardServer;
    using GameBlackjack;

    /// <summary>
    /// A command line application for using the CardGame classes.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args for the application.</param>
        public static void Main(string[] args)
        {
            bool loop = true;
            string input = string.Empty;
            while (!input.Equals("empty", StringComparison.CurrentCultureIgnoreCase) && loop)
            {
                Console.WriteLine("Local or Remote?");
                Console.Write(" > ");
                input = Console.ReadLine();
                if (input.Equals("local", StringComparison.CurrentCultureIgnoreCase))
                {
                    // We are running the server locally and have direct access to the game.
                    MainMenu m = new MainMenu();
                    loop = false;
                }
                else if (input.Equals("remote", StringComparison.CurrentCultureIgnoreCase))
                {
                    // We are accessing the game through the network, even though it is still a local server.
                    JoinMenu m = new JoinMenu();
                    loop = false;
                }
                else
                {
                    Console.WriteLine("Invalid selection.");
                }
            }

            Environment.Exit(0);
        }
    }
}
