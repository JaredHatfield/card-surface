// <copyright file="Program.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>Main application.</summary>
namespace CardServer
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using GameBlackjack;

    /// <summary>
    /// Main application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main application.
        /// </summary>
        /// <param name="args">The args for the application.</param>
        private static void Main(string[] args)
        {
            // TODO: Implement CardServer.
            ServerController serverController = new ServerController();
            serverController.GameController.SubscribeGame(typeof(Blackjack), "Blackjack");
            ReadOnlyCollection<string> s = serverController.GameController.GameTypes;
            Console.WriteLine("Available Game Types on this Server:");
            for (int i = 0; i < s.Count; i++)
            {
                Console.WriteLine(s[i]);
            }
        }
    }
}
