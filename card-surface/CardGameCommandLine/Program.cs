// <copyright file="Program.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A command line application for using the CardGame classes.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using CardGame;
    using CardServer;

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
            Game game = new Game();
            IGameController gameController = new GameController();
            gameController.AddGame(game);

            Console.ReadLine();
        }
    }
}
