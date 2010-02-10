// <copyright file="GameAccount.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary></summary>
namespace CardAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    class GameAccount
    {
        /// <summary>
        /// 
        /// </summary>
        private string username;

        /// <summary>
        /// 
        /// </summary>
        private string password;

        /// <summary>
        /// 
        /// </summary>
        private string profileImage;

        /// <summary>
        /// 
        /// </summary>
        private int balance;

        /// <summary>
        /// 
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


    }
}
