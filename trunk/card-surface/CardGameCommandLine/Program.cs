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
            // Add a dummy players to the game
            Program.PlayerJoinGame("player1", 100);
            Player player = Program.game.GetPlayer("player1");
            Program.DisplayPlayers();

            ServerController serverController = new ServerController();
            serverController.GameController.SubscribeGame(typeof(Blackjack), "Blackjack");
            Guid guid = serverController.GameController.CreateGame("Blackjack");
            Blackjack blackjack = serverController.GameController.GetGame(guid) as Blackjack;

            do
            {
                Console.Write("> ");
                string cmd = Console.ReadLine();
                if (cmd.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
                else if (cmd.Equals("bet 1", StringComparison.CurrentCultureIgnoreCase))
                {
                    Player p;
                    try
                    {
                        p = game.GetActivePlayer();
                    }
                    catch
                    {
                        p = player;
                    }

                    Program.PlaceBet(p, 1);
                }
                else if (cmd.Equals("bet 5", StringComparison.CurrentCultureIgnoreCase))
                {
                    Player p;
                    try
                    {
                        p = game.GetActivePlayer();
                    }
                    catch
                    {
                        p = player;
                    }

                    Program.PlaceBet(p, 5);
                }
                else if (cmd.Equals("bet 10", StringComparison.CurrentCultureIgnoreCase))
                {
                    Player p;
                    try
                    {
                        p = game.GetActivePlayer();
                    }
                    catch
                    {
                        p = player;
                    }

                    Program.PlaceBet(p, 10);
                }
                else if (cmd.Equals("bet 25", StringComparison.CurrentCultureIgnoreCase))
                {
                    Player p;
                    try
                    {
                        p = game.GetActivePlayer();
                    }
                    catch
                    {
                        p = player;
                    }

                    Program.PlaceBet(p, 25);
                }
                else if (cmd.Equals("showseats", StringComparison.CurrentCultureIgnoreCase))
                {
                    Program.DisplaySeats();
                }
                else
                {
                    try
                    {
                        // SOMEONE BROKE THIS!
                        //serverController.ServerCommunicationController.SendMessage(game, Message.MessageType.GameState);
                        Program.game.ExecuteAction(cmd, game.GetActivePlayerSeat().Username);
                    }
                    catch (CardGamePlayerNotFoundException ex)
                    {
                        if (cmd.Equals("Deal"))
                        {
                           Program.game.ExecuteAction(cmd, null);
                        }
                        else
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    catch (CardGameActionNotFoundException ex)
                    {
                        Console.Write(ex);
                    }
                }

                Program.DisplayPlayers();
            }
            while (true);

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
                    Console.Write("[" + p.Actions.Count + "] ");
                    for (int j = 0; j < p.Actions.Count; j++)
                    {
                        Console.Write(p.Actions[j]);
                        if (j + 1 < p.Actions.Count)
                        {
                            Console.Write(", ");
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("Bank: " + p.BankPile.ToString());
                    Console.WriteLine("Bet: " + p.PlayerArea.Chips[0].ToString());

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
        /// Places the bet.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="chipValue">The chip value.</param>
        private static void PlaceBet(Player player, int chipValue)
        {
            // Find the physical object
            IChip chip = null;
            for (int i = 0; i < player.BankPile.Chips.Count; i++)
            {
                if ((player.BankPile.Chips[i] as IChip).Amount.Equals(chipValue))
                {
                    chip = player.BankPile.Chips[i] as IChip;
                    break;
                }
            }

            if (chip != null)
            {
                Program.game.MoveAction(chip.Id, player.PlayerArea.Chips[0].Id);
            }
            else
            {
                Console.WriteLine("Invalid bet amount.");
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
        /// <param name="amount">The amount.</param>
        private static void PlayerJoinGame(string username, int amount)
        {
            for (int i = 0; i < Program.game.Seats.Count; i++)
            {
                if (Program.game.Seats[i].IsEmpty)
                {
                    Seat seat = Program.game.Seats[i];
                    if (game.SitDown(username, seat.Password, amount))
                    {
                        Console.WriteLine(username + " has joined the game at seat " + seat.Location.ToString() + ".");
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
