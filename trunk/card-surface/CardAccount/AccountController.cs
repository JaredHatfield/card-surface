// <copyright file="AccountController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The AccountController is responsible for managing the systems users.</summary>
namespace CardAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// The AccountController is responsible for managing the systems users.
    /// </summary>
    public class AccountController
    {
        /// <summary>
        /// The singleton instance of AccountController
        /// </summary>
        private static AccountController instance = new AccountController();

        /// <summary>
        /// The list of users that are on the system.
        /// </summary>
        private List<GameAccount> users = new List<GameAccount>();

        /// <summary>
        /// Prevents a default instance of the AccountController class from being created.
        /// </summary>
        private AccountController()
        {
            // https://code.google.com/p/student-educational-arrangement-tool/source/browse/trunk/SEAT/SEATLibrary/Room.cs
            // This is for saving to a file from FLAT file
        }

        /// <summary>
        /// Gets the instance of the AccountController.
        /// </summary>
        /// <value>The instance.</value>
        public static AccountController Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Authenticates the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>whether or not the combination is valid</returns>
        public bool Authenticate(string username, string password)
        {
            bool valid = false;

            foreach (GameAccount account in this.users)
            {
                if ((account.Username == username) && (account.Password == password))
                {
                    valid = true;
                }
            }

            return valid;
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>whether account was created.</returns>
        public bool CreateAccount(string username, string password)
        {
            bool success = false;

            foreach (GameAccount acnt in this.users)
            {
                if (acnt.Username == username)
                {
                    success = true;
                }
            }

            if (success)
            {
                try
                {
                    GameAccount account = new GameAccount(username, password);

                    this.users.Add(account);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught creating Game Account.", e);
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// Looks up user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>the account of the username</returns>
        public GameAccount LookUpUser(string username)
        {
            GameAccount account = null;

            foreach (GameAccount acnt in this.users)
            {
                if (acnt.Username == username)
                {
                    account = acnt;
                }
            }

            return account;
        }

        /// <summary>
        /// Updates the account.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="profileImage">The profile image.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="gamesPlayed">The games played.</param>
        /// <returns>whether the account was updated</returns>
        public bool UpdateAccount(string username, string password, string profileImage, int balance, int gamesPlayed)
        {
            bool updated = false;

            foreach (GameAccount account in this.users)
            {
                if (account.Username == username)
                {
                    updated = account.UpdateAccount(username, password, profileImage, balance, gamesPlayed);
                }
            }

            return updated;
        }
    }
}