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
    using CardGame;
    using CardServer;
    using GameBlackjack;

    /// <summary>
    /// A command line application for using the CardGame classes.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The game of Blackjack that is being played.
        /// </summary>
        private static Game game = new Blackjack();

        /// <summary>
        /// Mains the specified args.
        /// </summary>
        /// <param name="args">The args for the application.</param>
        public static void Main(string[] args)
        {
            /* By creating a ServerController, CardGameCommandLine can be set as the StartUp project to enable a Game to interact with CardWeb. */
            ServerController serverController = new ServerController();

            // Add some dummy players to the game
            Program.PlayerJoinGame("player1");
            Program.PlayerJoinGame("player2");
            Program.DisplayPlayers();

            Program.DisplaySeats();

            /* Add a reference to this game to the server's GameController. */
            serverController.GameController.AddGame(Program.game);

            do
            {
                string cmd = Console.ReadLine();
                if (cmd.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
                else
                {
                    Program.game.ExecuteAction(cmd, string.Empty);
                }
            }
            while(true);

            Environment.Exit(0);
        }

        /// <summary>
        /// Displays the player.
        /// </summary>
        private static void DisplayPlayers()
        {
            for (int i = 0; i < Program.game.Seats.Count; i++)
            {
                if (!Program.game.Seats[i].IsEmpty)
                {
                    Player p = Program.game.Seats[i].Player;
                    Console.WriteLine(Program.game.Seats[i].Username);
                    Console.Write("Actions: ");
                    for (int j = 0; j < p.Actions.Count; j++)
                    {
                        Console.Write(p.Actions[j]);
                        if (j + 1 < p.Actions.Count)
                        {
                            Console.Write(", ");
                        }
                    }

                    Console.WriteLine();

                    Console.WriteLine("Hand: " + BlackjackRules.GetPileVale(p.Hand));
                    if (p.Hand.Cards.Count > 0)
                    {
                        Program.DisplayCardPile(p.Hand);
                    }
                    else
                    {
                        Console.WriteLine("Empty");
                    }

                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Displays the playing area.
        /// </summary>
        /// <param name="playingArea">The playing area.</param>
        private static void DisplayPlayingArea(PlayingArea playingArea)
        {
            // TODO: Implement DisplayPlayingArea
        }

        /// <summary>
        /// Displays the card pile.
        /// </summary>
        /// <param name="pile">The card pile.</param>
        private static void DisplayCardPile(CardPile pile)
        {
            for (int i = 0; i < pile.Cards.Count; i++)
            {
                Console.WriteLine(pile.Cards[i].ToString());
            }
        }

        /// <summary>
        /// Join a player to a game.
        /// </summary>
        /// <param name="username">The username of the player.</param>
        private static void PlayerJoinGame(string username)
        {
            for (int i = 0; i < Program.game.Seats.Count; i++)
            {
                if (Program.game.Seats[i].IsEmpty)
                {
                    if (game.SitDown(username, Program.game.Seats[i].Password))
                    {
                        Console.WriteLine(username + "has joined the game.");
                    }
                    else
                    {
                        Console.WriteLine(username + " could not join the game.");
                    }

                    return;
                }
            }

            Console.WriteLine("There were no open seats for " + username + " to join.");
        }

        /// <summary>
        /// Prints the player list.
        /// </summary>
        private static void PrintPlayerList()
        {
            for (int i = 0; i < Program.game.Seats.Count; i++)
            {
                if (!Program.game.Seats[i].IsEmpty)
                {
                    Console.WriteLine(Program.game.Seats[i].Location + " - " + Program.game.Seats[i].Username);
                }
            }
        }

        /// <summary>
        /// Displays the empty seats and their seat codes.
        /// </summary>
        private static void DisplaySeats()
        {
            Console.WriteLine("Available Seats:\n");
            for (int i = 0; i < Program.game.Seats.Count; i++)
            {
                if (Program.game.Seats[i].IsEmpty)
                {
                    Console.WriteLine(Program.game.Seats[i].Location + " - " + Program.game.Seats[i].Password);
                }
            }
        }
    }
}
