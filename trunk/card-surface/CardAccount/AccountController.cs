﻿// <copyright file="AccountController.cs" company="University of Louisville Speed School of Engineering">
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
        /// The list of users that are on the system.
        /// </summary>
        private List<GameAccount> users;

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
                if (account.Username = username && account.Password = password)
                {
                    valid = true;
                }
            }

            return valid;
        }

        /// <summary>
        /// Looks up user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>the account of the username</returns>
        public GameAccount LookUpUser(string username)
        {
            GameAccount account;

            foreach (GameAccount account in this.users)
            {
                if (account.Username = username)
                {
                    this.account = account;
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
            foreach (GameAccount account in this.users)
            {
                if (account.Username = username)
                {
                    account.UpdateAccount(username, password, profileImage, balance, gamesPlayed);
                }
            }
        }
    }
}