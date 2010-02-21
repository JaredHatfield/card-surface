// <copyright file="Seat.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A Seat that can have a player sitting in it in a Game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A seat at the game.
    /// </summary>
    public class Seat
    {
        /// <summary>
        /// The length of the seat passwords that are used to join a table.
        /// </summary>
        private const int PasswordLength = 7;

        /// <summary>
        /// The player sitting at this seat.
        /// </summary>
        private Player player;

        /// <summary>
        /// The players location at the table.
        /// </summary>
        private SeatLocation location;

        /// <summary>
        /// The password required to join this seat.
        /// </summary>
        private string password;

        /// <summary>
        /// The username of the Account using this seat.
        /// </summary>
        private string username;

        /// <summary>
        /// The unique id for this seat.
        /// </summary>
        private Guid id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Seat"/> class.
        /// </summary>
        /// <param name="location">The location of the seat.</param>
        internal Seat(SeatLocation location)
        {
            this.player = null;
            this.location = location;
            this.password = Seat.GenerateSeatPassword();
            this.username = null;
            this.id = Guid.NewGuid();
        }

        /// <summary>
        /// The locations at the table.
        /// </summary>
        public enum SeatLocation
        {
            /// <summary>
            /// The north position on the table.
            /// </summary>
            North,

            /// <summary>
            /// The east position on the table.
            /// </summary>
            East,

            /// <summary>
            /// The west position on the table.
            /// </summary>
            West,

            /// <summary>
            /// The south position on the table.
            /// </summary>
            South,

            /// <summary>
            /// The southeast position on the table.
            /// </summary>
            SouthEast,

            /// <summary>
            /// The northeast position on the table.
            /// </summary>
            NorthEast,

            /// <summary>
            /// The northwest position on the table.
            /// </summary>
            NorthWest,

            /// <summary>
            /// The southwest position on the table.
            /// </summary>
            SouthWest
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <value>The player.</value>
        public Player Player
        {
            get { return this.player; }
        }

        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public SeatLocation Location
        {
            get { return this.location; }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return this.password; }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return this.username; }
        }

        /// <summary>
        /// Gets the unique id.
        /// </summary>
        /// <value>The unique id.</value>
        public Guid Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets a value indicating whether this seat is empty.
        /// </summary>
        /// <value><c>true</c> if this seat is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get
            {
                if (this.player == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Allows an account to sit down if it has the correct password and the chair is empty.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if the action was successful; otherwise false.</returns>
        public bool SitDown(string username, string password)
        {
            // TODO: Implement logic that allows a player to sit down at the seat.
            return false;
        }

        /// <summary>
        /// Generates a seat password.
        /// </summary>
        /// <returns>A string for a password.</returns>
        private static string GenerateSeatPassword()
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
