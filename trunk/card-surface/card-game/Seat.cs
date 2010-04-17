// <copyright file="Seat.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>A Seat that can have a player sitting in it in a Game.</summary>
namespace CardGame
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using CardGame.GameException;

    /// <summary>
    /// A seat at the game.
    /// </summary>`
    [Serializable]
    public class Seat : INotifyPropertyChanged
    {
        /// <summary>
        /// The list of all of the passwords, used to prevent duplicate passwords from being generated.
        /// </summary>
        private static ObservableCollection<string> passwordBank = new ObservableCollection<string>();

        /// <summary>
        /// The player sitting at this seat.
        /// </summary>
        private Player player;

        /// <summary>
        /// A flag that indicates someone can sit in thie chair.
        /// </summary>
        private bool sittable;

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
            this.sittable = true;
            this.player = null;
            this.location = location;
            this.AssignSeatPassword();
            this.username = null;
            this.id = Guid.NewGuid();
        }

        /// <summary>
        /// The delegate for an event that is triggered when a player leaves a game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void PlayerLeaveSeatEventHandler(object sender, EventArgs e);

        /// <summary>
        /// The delegate for an event that is triggered when a player joins a game.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void PlayerJoinSeatEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Occurs when a player leaves the game.
        /// </summary>
        public event PlayerLeaveSeatEventHandler PlayerLeaveGame;

        /// <summary>
        /// Occurs when a player joins the game.
        /// </summary>
        public event PlayerJoinSeatEventHandler PlayerJoinGame;

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        /// Gets a value indicating whether this instance is sittable.
        /// </summary>
        /// <value><c>true</c> if this instance is sittable; otherwise, <c>false</c>.</value>
        public bool IsSittable
        {
            get { return this.sittable; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Seat"/> is sittable.
        /// </summary>
        /// <value><c>true</c> if sittable; otherwise, <c>false</c>.</value>
        internal bool Sittable
        {
            get { return this.sittable; }
            set { this.sittable = value; }
        }

        /// <summary>
        /// Gets a SeatLocation based on the string name of a SeatLocation.
        /// </summary>
        /// <param name="name">The string representation of a SeatLocation.</param>
        /// <returns>The SeatLocation requested.</returns>
        public static SeatLocation ParseSeatLocation(string name)
        {
            switch (name.ToLower())
            {
                case "east":
                    return SeatLocation.East;
                case "north":
                    return SeatLocation.North;
                case "northeast":
                    return SeatLocation.NorthEast;
                case "northwest":
                    return SeatLocation.NorthWest;
                case "south":
                    return SeatLocation.South;
                case "southeast":
                    return SeatLocation.SouthEast;
                case "southwest":
                    return SeatLocation.SouthWest;
                case "west":
                    return SeatLocation.West;
            }

            throw new CardGameSeatNotFoundException();
        }

        /// <summary>
        /// Allows an account to sit down if it has the correct password and the chair is empty.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if the action was successful; otherwise false.</returns>
        public bool SitDown(string username, string password)
        {
            // 1) Make sure the seat is empty
            if (!this.IsEmpty)
            {
                // TODO: This should throw an exception!
                return false;
            }

            // 2) Make sure this seat is sittable
            if (!this.sittable)
            {
                throw new CardGameSeatNotSittableException();
            }

            // 3) Make sure the passwords match
            if (!this.password.Equals(password))
            {
                return false;
            }

            // 4) Change the password
            this.AssignSeatPassword();

            // 5) Assign the username to this seat
            this.username = username;
            this.NotifyPropertyChanged("Username");

            // 6) Make a player and add them to the seat
            this.player = new Player();
            this.NotifyPropertyChanged("Player");
            this.OnJoinGame();

            // 7) Indicate that everything was successful
            return true;
        }

        /// <summary>
        /// Test to see if the password is correct without making any changes.
        /// </summary>
        /// <param name="password">The password to test.</param>
        /// <returns>True if the seat is empty and the password is correct; otherwise false.</returns>
        public bool PasswordPeek(string password)
        {
            if (this.IsEmpty && this.password.Equals(password))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the specified seat.
        /// This does not update the PhysicalObjects.
        /// </summary>
        /// <param name="seat">The seat to reflect.</param>
        internal void Update(Seat seat)
        {
            this.id = seat.id;
            this.username = seat.username;
            this.password = seat.password;
            this.sittable = seat.sittable;
            if (this.player == null && seat.player != null)
            {
                // A Player joined so lets add them
                this.player = new Player();
                this.OnJoinGame();
            }

            // Now update the player
            if (this.player != null)
            {
                this.player.Update(seat.player);
            }
        }

        /// <summary>
        /// The player has left the game so we remove any reference to them.
        /// </summary>
        internal void PlayerLeft()
        {
            this.player = null;
            this.OnLeaveGame();
        }

        /// <summary>
        /// Have the player leave this seat.
        /// </summary>
        internal void Leave()
        {
            this.player = null;
            this.username = string.Empty;
            this.AssignSeatPassword();
            this.OnLeaveGame();
        }

        /// <summary>
        /// Raises the <see cref="E:PlayerLeaveGame"/> event.
        /// </summary>
        protected void OnLeaveGame()
        {
            if (this.PlayerLeaveGame != null)
            {
                this.PlayerLeaveGame(this, null);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:PlayerJoinGame"/> event.
        /// </summary>
        protected void OnJoinGame()
        {
            if (this.PlayerJoinGame != null)
            {
                this.PlayerJoinGame(this, null);
            }
        }

        /// <summary>
        /// Generates a seat password.
        /// </summary>
        /// <returns>A string for a password.</returns>
        private static string GeneratePassword()
        {
            int length = CardGame.Properties.Settings.Default.SeatPasswordLength;
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65))));
            }

            return builder.ToString().ToLower();
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="info">The name of the property that changed.</param>
        private void NotifyPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        /// <summary>
        /// Assigns a new seat password to this object.
        /// </summary>
        private void AssignSeatPassword()
        {
            // Remove the existing password from the bank.
            if (this.password != null)
            {
                Seat.passwordBank.Remove(this.password);
            }

            // Generate a password.
            string newpassword = Seat.GeneratePassword();

            // Keep generating passwords until we have a unique password.
            while (Seat.passwordBank.Contains(newpassword))
            {
                newpassword = Seat.GeneratePassword();
            }

            // Assign the password and add it to the bank.
            this.password = newpassword;
            this.NotifyPropertyChanged("Password");
            Seat.passwordBank.Add(this.password);
        }
    }
}
