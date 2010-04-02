// <copyright file="GameMenu.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The Game Menu that allows the player to actually play the game.</summary>
namespace CardGameCommandLine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CardGame;
    using CardGame.GameException;

    /// <summary>
    /// The Game Menu that allows the player to actually play the game.
    /// </summary>
    internal class GameMenu : BaseMenu
    {
        /// <summary>
        /// The instance of the game that is being played.
        /// </summary>
        private Game game;

        /// <summary>
        /// The selected Seat for this Game.
        /// </summary>
        private Seat seat;

        /// <summary>
        /// The username of the person in the Seat.
        /// This can also be accessed from the Seat.
        /// </summary>
        private string username;

        /// <summary>
        /// The Player that is located in the seat.
        /// This can also be accessed from the Seat.
        /// </summary>
        private Player player;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameMenu"/> class.
        /// </summary>
        /// <param name="game">The Game instance.</param>
        public GameMenu(Game game)
        {
            this.game = game;
            Console.Clear();

            // First we let the player choose an empty Seat
            Console.WriteLine("Select from available seats:");
            for (int i = 0; i < game.Seats.Count; i++)
            {
                if (game.Seats[i].IsEmpty)
                {
                    Console.WriteLine(game.Seats[i].Location);
                }
            }

            Seat seat = null;
            while (seat == null)
            {
                Seat.SeatLocation location;
                try
                {
                    Console.Write(" >");
                    location = Seat.ParseSeatLocation(Console.ReadLine());
                    seat = game.GetSeat(location);
                    if (!seat.IsEmpty)
                    {
                        seat = null;
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid selection");
                }
            }

            // Display the password for that Seat
            Console.Clear();
            Console.WriteLine("Seat Password for " + seat.Location + ": " + seat.Password);

            // Wait until someone appears in that seat
            while (seat.IsEmpty)
            {
                Console.WriteLine("Press enter after you have joined the game on your mobile device...");
                Console.ReadLine();
            }

            // Now we can start playing the game
            this.seat = seat;
            this.username = seat.Username;
            this.player = seat.Player;
            this.GameLoop();
        }

        /// <summary>
        /// The main loop of the game.
        /// </summary>
        private void GameLoop()
        {
            string input = string.Empty;
            while (!input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Clear();
                CommandLineGraphics.Display(this.game);

                Console.Write(" > ");
                input = Console.ReadLine();
                if (input.Length == 0)
                {
                    // We are just reloading the screen, nothing exciting here...
                }
                else if (input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                {
                    // Nothing to see here, move along...
                }
                else if (input.StartsWith("move", StringComparison.CurrentCultureIgnoreCase))
                {
                    // The player is trying to move a piece so we need to use that subroutine
                    try
                    {
                        this.Move(input);
                        Console.WriteLine("The move was executed successfully!");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something went terribly wrong!");
                        Console.WriteLine(e.ToString());
                    }

                    this.PrompForEnter();
                }
                else
                {
                    // We assume the player is trying to execute an action
                    try
                    {
                        this.game.ExecuteAction(input, this.username);
                        Console.WriteLine("The action was executed successfully!");
                    }
                    catch (CardGameActionNotFoundException)
                    {
                        Console.WriteLine("You have entered an invalid action.");
                    }
                    catch (CardGameActionAccessDeniedException)
                    {
                        Console.WriteLine("You are not allowed to execute that action at this time.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something went terribly wrong!");
                        Console.WriteLine(e.ToString());
                    }

                    this.PrompForEnter();
                }
            }
        }

        /// <summary>
        /// Perform the move as specified by the player.
        /// We are assuming they are only moving PhysicalObject to piles that are in their area.
        /// This should be enhanced to allow moves of anything to anywhere.
        /// We are not going to catch exceptions in this routine because we catch them when we call this.
        /// </summary>
        /// <param name="input">The input string.</param>
        private void Move(string input)
        {
            // We need to parse the string and execute the appropriate action...

            // First, we break the string into its components saparated by spaces
            string[] words = input.Split(' ');
            if (words.Length == 4)
            {
                // Lets get everything we need all in one place
                Pile sourcePile = this.GetPile(words[1]);
                IPhysicalObject physicalObject;
                try
                {
                    int number = Int32.Parse(words[2]);
                    physicalObject = sourcePile.GetPhysicalObject(number);
                }
                catch
                {
                    throw new CardGamePhysicalObjectNotFoundException();
                }

                Pile destinationPile = this.GetPile(words[3]);

                // Now actually attempt the move!
                this.game.MoveAction(physicalObject.Id, destinationPile.Id);
            }
            else
            {
                throw new Exception("The Move command did not receive the correct number of parameters.");
            }
        }

        /// <summary>
        /// Gets the pile specified by a string.
        /// This method will throw a CardGamePileNotFoundException if the pile could not be found.
        /// This only gets the pile for the player that is playing on this instance of the game.
        /// This could be expanded to let the player access any of the piles that are in the game.
        /// </summary>
        /// <param name="name">The name of the pile.</param>
        /// <returns>The instance of the pile that was located.</returns>
        private Pile GetPile(string name)
        {
            if (name.Equals("hand", StringComparison.CurrentCultureIgnoreCase))
            {
                return this.player.Hand;
            }
            else if (name.Equals("bank", StringComparison.CurrentCultureIgnoreCase))
            {
                return this.player.BankPile;
            }
            else if (name.StartsWith("chip", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    int number = Int32.Parse(name.Substring(4));
                    if (number < this.player.PlayerArea.Chips.Count)
                    {
                        return this.player.PlayerArea.Chips[number];
                    }
                }
                catch
                {
                    // We just give up, an exception will be thrown at the end of this method.
                }
            }
            else if (name.StartsWith("card", StringComparison.CurrentCultureIgnoreCase))
            {
                try
                {
                    int number = Int32.Parse(name.Substring(4));
                    if (number < this.player.PlayerArea.Cards.Count)
                    {
                        return this.player.PlayerArea.Cards[number];
                    }
                }
                catch
                {
                    // We just give up, an exception will be thrown at the end of this method.
                }
            }

            throw new CardGamePileNotFoundException();
        }
    }
}
