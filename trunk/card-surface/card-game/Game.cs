// <copyright file="Game.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A generic card game that is extended to implement a specific game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A generic card game that is extended to implement a specific game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The length of the seat passwords that are used to join a table.
        /// </summary>
        private const int PasswordLength = 7;

        /// <summary>
        /// A unique identifier for a game.
        /// </summary>
        private Guid id;

        /// <summary>
        /// The center shared area of the game for cards and chips.
        /// </summary>
        private PlayingArea gamingArea;

        /// <summary>
        /// The collection of players that are playing in the game.
        /// </summary>
        private ObservableCollection<Player> players;

        /// <summary>
        /// The list of seat passwords.
        /// </summary>
        private Dictionary<Player.SeatLocation, string> seatPasswords;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        internal Game()
        {
            this.id = Guid.NewGuid();
            this.gamingArea = new PlayingArea();
            this.players = new ObservableCollection<Player>();
            this.seatPasswords = new Dictionary<Player.SeatLocation, string>(8);

            // Set an initial value for all of the seat locations
            this.seatPasswords[Player.SeatLocation.East] = this.GenerateSeatPassword();
            this.seatPasswords[Player.SeatLocation.North] = this.GenerateSeatPassword();
            this.seatPasswords[Player.SeatLocation.NorthEast] = this.GenerateSeatPassword();
            this.seatPasswords[Player.SeatLocation.NorthWest] = this.GenerateSeatPassword();
            this.seatPasswords[Player.SeatLocation.South] = this.GenerateSeatPassword();
            this.seatPasswords[Player.SeatLocation.SouthEast] = this.GenerateSeatPassword();
            this.seatPasswords[Player.SeatLocation.SouthWest] = this.GenerateSeatPassword();
            this.seatPasswords[Player.SeatLocation.West] = this.GenerateSeatPassword();
        }

        /// <summary>
        /// Gets the unique id for a game.
        /// </summary>
        /// <value>The unique id.</value>
        public Guid Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the gaming area.
        /// </summary>
        /// <value>The gaming area.</value>
        public PlayingArea GamingArea
        {
            get { return this.gamingArea; }
        }

        /// <summary>
        /// Gets the players that are playing the game.
        /// </summary>
        /// <value>The players that are playing the game.</value>
        public ReadOnlyObservableCollection<Player> Players
        {
            get { return new ReadOnlyObservableCollection<Player>(this.players); }
        }

        /// <summary>
        /// Gets the number of players in the game.
        /// </summary>
        /// <value>The number of players in the game.</value>
        public int NumberOfPlayers
        {
            get { return this.players.Count; }
        }

        /// <summary>
        /// Gets the seat passwords.
        /// </summary>
        /// <value>The seat passwords.</value>
        internal Dictionary<Player.SeatLocation, string> SeatPasswords
        {
            get { return this.seatPasswords; }
        }

        /// <summary>
        /// Gets the player at a specific seat.
        /// </summary>
        /// <param name="location">The location of the player.</param>
        /// <returns>The player at that location.</returns>
        public Player GetPlayer(Player.SeatLocation location)
        {
            for (int i = 0; i < this.players.Count; i++)
            {
                if (this.players[i].Location == location)
                {
                    return this.players[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the seat at the specified location is empty.
        /// </summary>
        /// <param name="location">The location to check.</param>
        /// <returns>
        ///     <c>true</c> if the seat at the specified location is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSeatEmpty(Player.SeatLocation location)
        {
            for (int i = 0; i < this.players.Count; i++)
            {
                if (this.players[i].Location == location)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Generates a seat password.
        /// </summary>
        /// <returns>A string for a password.</returns>
        private string GenerateSeatPassword()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < PasswordLength; i++)
            {
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65))));
            }

            return builder.ToString().ToLower();
        }
    }
}
