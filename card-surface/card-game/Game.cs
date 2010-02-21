﻿// <copyright file="Game.cs" company="University of Louisville Speed School of Engineering">
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
        private ReadOnlyObservableCollection<Seat> seats;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        internal Game()
        {
            this.id = Guid.NewGuid();
            this.gamingArea = new PlayingArea();
            ObservableCollection<Seat> s = new ObservableCollection<Seat>();
            s.Add(new Seat(Seat.SeatLocation.East));
            s.Add(new Seat(Seat.SeatLocation.North));
            s.Add(new Seat(Seat.SeatLocation.NorthEast));
            s.Add(new Seat(Seat.SeatLocation.NorthWest));
            s.Add(new Seat(Seat.SeatLocation.South));
            s.Add(new Seat(Seat.SeatLocation.SouthEast));
            s.Add(new Seat(Seat.SeatLocation.SouthWest));
            s.Add(new Seat(Seat.SeatLocation.West));
            this.seats = new ReadOnlyObservableCollection<Seat>(s);
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
        /// Gets the seats.
        /// </summary>
        /// <value>The seats.</value>
        public ReadOnlyObservableCollection<Seat> Seats
        {
            get { return this.seats; }
        }

        /// <summary>
        /// Gets the number of players in the game.
        /// </summary>
        /// <value>The number of players in the game.</value>
        public int NumberOfPlayers
        {
            get
            {
                int number = 0;
                for (int i = 0; i < this.seats.Count; i++)
                {
                    if (!this.seats[i].IsEmpty)
                    {
                        number++;
                    }
                }

                return number;
            }
        }

        /// <summary>
        /// Gets the player at a specific seat.
        /// </summary>
        /// <param name="location">The location of the player.</param>
        /// <returns>The player at that location.</returns>
        public Player GetPlayer(Seat.SeatLocation location)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].Location == location)
                {
                    return this.seats[i].Player;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a player based off of a username
        /// </summary>
        /// <param name="username">The username to look up.</param>
        /// <returns>The instance of the player.</returns>
        public Player GetPlayer(string username)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty && this.seats[i].Username.Equals(username))
                {
                    return this.seats[i].Player;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified username is playing this game.
        /// </summary>
        /// <param name="username">The username to look up.</param>
        /// <returns>
        ///     <c>true</c> if the specified username is playing this game; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserPlaying(string username)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (!this.seats[i].IsEmpty && this.seats[i].Username.Equals(username))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the seat at the specified location is empty.
        /// </summary>
        /// <param name="location">The location to check.</param>
        /// <returns>
        ///     <c>true</c> if the seat at the specified location is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsSeatEmpty(Seat.SeatLocation location)
        {
            for (int i = 0; i < this.seats.Count; i++)
            {
                if (this.seats[i].Location == location)
                {
                    return this.seats[i].IsEmpty;
                }
            }

            return true;
        }
    }
}
