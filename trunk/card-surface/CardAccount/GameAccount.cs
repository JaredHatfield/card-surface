// <copyright file="GameAccount.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An account for a user of the system.</summary>
namespace CardAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// An account for a user of the system.
    /// </summary>
    public class GameAccount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameAccount"/> class.
        /// </summary>
        public GameAccount()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameAccount"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="profileImage">The profile image.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="gamesPlayed">The games played.</param>
        public GameAccount(string username, string password, string profileImage,
                           int balance, int gamesPlayed)
        {
            this.username = username;
            this.password = password;
            this.profileImage = profileImage;
            this.balance = balance;
            this.gamesPlayed = gamesPlayed;
        }

        /// <summary>
        /// The account's username.
        /// </summary>
        private string username;

        /// <summary>
        /// The account's password.
        /// </summary>
        private string password;

        /// <summary>
        /// The account's profile picture.
        /// </summary>
        private string profileImage;

        /// <summary>
        /// The account's balance.
        /// </summary>
        private int balance;

        /// <summary>
        /// The number of games played on the account.
        /// </summary>
        private int gamesPlayed;

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return this.username; }
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
        /// Gets the profile image.
        /// </summary>
        /// <value>The profile image.</value>
        public string ProfileImage
        {
            get { return this.profileImage; }
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        /// <value>The balance.</value>
        public int Balance
        {
            get { return this.balance; }
        }

        /// <summary>
        /// Gets the games played.
        /// </summary>
        /// <value>The games played.</value>
        public int GamesPlayed
        {
            get { return this.gamesPlayed; }
        }

        /// <summary>
        /// Balances the change.
        /// </summary>
        /// <param name="value">The value.</param>
        public void BalanceChange(double value)
        {
            balance += value;
        }

        /// <summary>
        /// Adds the game.
        /// </summary>
        public void AddGame()
        {
            gamesPlayed++;
        }
    }
}
