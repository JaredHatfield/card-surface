// <copyright file="CommandLineGraphics.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A collection of static methods used to display the Game elements.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;

    /// <summary>
    /// A collection of static methods used to display the Game elements.
    /// </summary>
    internal class CommandLineGraphics
    {
        /// <summary>
        /// Displays the specified game.
        /// </summary>
        /// <param name="game">The Game to display.</param>
        public static void Display(Game game)
        {
            // Display the gaming area
            Console.WriteLine(game.Name);
            for (int i = 0; i < game.GamingArea.Chips.Count; i++)
            {
                Console.WriteLine("Chip" + i + ": " + game.GamingArea.Chips[i].ToString());
            }

            for (int i = 0; i < game.GamingArea.Cards.Count; i++)
            {
                Console.WriteLine("Card" + i + ": " + game.GamingArea.Cards[i].ToString());
            }

            CommandLineGraphics.DisplayDivider();

            // Display all of the players
            for (int i = 0; i < game.Seats.Count; i++)
            {
                if (game.Seats[i].IsSittable)
                {
                    CommandLineGraphics.Display(game.Seats[i]);
                }
            }
        }

        /// <summary>
        /// Displays the specified Seat.
        /// </summary>
        /// <param name="seat">The Seat to display.</param>
        private static void Display(Seat seat)
        {
            if (seat.IsEmpty)
            {
                Console.WriteLine(seat.Location + " is empty.");
            }
            else
            {
                Console.WriteLine(seat.Location + " Player: " + seat.Username);
                CommandLineGraphics.Display(seat.Player);
            }

            CommandLineGraphics.DisplayDivider();
        }

        /// <summary>
        /// Displays the specified player.
        /// </summary>
        /// <param name="player">The Player to display.</param>
        private static void Display(Player player)
        {
            Console.WriteLine("Balance is " + player.Balance.ToString());
            Console.Write("Actions: ");
            for (int i = 0; i < player.Actions.Count; i++)
            {
                Console.Write(player.Actions[i]);
                if (i + 1 < player.Actions.Count)
                {
                    Console.Write(", ");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Hand: " + player.Hand.ToString());
            Console.WriteLine("Bank: " + player.BankPile.ToString());
            for (int i = 0; i < player.PlayerArea.Chips.Count; i++)
            {
                Console.WriteLine("Chip" + i + ": " + player.PlayerArea.Chips[i].ToString());
            }

            for (int i = 0; i < player.PlayerArea.Cards.Count; i++)
            {
                Console.WriteLine("Card" + i + ": " + player.PlayerArea.Cards[i].ToString());
            }
        }

        /// <summary>
        /// Displays the divider.
        /// </summary>
        private static void DisplayDivider()
        {
            for (int i = 0; i < 80; i++)
            {
                Console.Write("-");
            }

            Console.WriteLine();
        }
    }
}
