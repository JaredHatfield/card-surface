// <copyright file="Game.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A generic card game that is extended to implement a specific game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A generic card game that is extended to implement a specific game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The center shared area of the game for cards and chips.
        /// </summary>
        private PlayingArea gamingArea;

        /// <summary>
        /// The collection of players that are playing in the game.
        /// </summary>
        private ObservableCollection<Player> players;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        internal Game()
        {
            this.gamingArea = new PlayingArea();
            this.players = new ObservableCollection<Player>();
            Player p = new Player(Player.SeatLocation.North);
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
    }
}
