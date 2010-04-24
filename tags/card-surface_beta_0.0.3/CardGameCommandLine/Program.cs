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
            JoinMenu m = new JoinMenu();
            Environment.Exit(0);
        }
    }
}
