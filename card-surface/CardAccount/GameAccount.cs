// <copyright file="GameAccount.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>An account for a user of the system.</summary>
namespace CardAccount
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// An account for a user of the system.
    /// </summary>
    public class GameAccount
    {
        /// <summary>
        /// Key used for encryption/decryption
        /// </summary>
        private static byte[] encryptionKey = { 34, 54, 67, 78, 89, 25, 74, 23, 46, 88, 24, 12, 35, 64, 35, 26, 82, 24, 36, 83, 57, 92, 43, 62 };

        /// <summary>
        /// Initialization vector for encryption/decryption
        /// </summary>
        private static byte[] initVector = { 74, 23, 46, 88, 24, 12, 35, 64, 35, 26, 82, 24, 36, 83, 57, 92, 43, 62, 34, 54, 67, 78, 89, 25 };

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
        /// The number of games won on the account.
        /// </summary>
        private int gamesWon;

        /// <summary>
        /// The number of games lost on the account.
        /// </summary>
        private int gamesLost;

        /*/// <summary>
        /// MAY ADD LATER
        /// </summary>
        private int gameStreak;*/

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
        public GameAccount(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.profileImage = String.Empty;
            this.balance = 0;
            this.gamesPlayed = 0;
            this.gamesWon = 0;
            this.gamesLost = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameAccount"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="profileImage">The profile image.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="gamesPlayed">The games played.</param>
        /// <param name="gamesWon">The games won.</param>
        /// <param name="gamesLost">The games lost.</param>
        public GameAccount(string username, string password, string profileImage, int balance, int gamesPlayed, int gamesWon, int gamesLost)
        {
            this.username = username;
            this.password = password;
            this.profileImage = profileImage;
            this.balance = balance;
            this.gamesPlayed = gamesPlayed;
            this.gamesWon = gamesWon;
            this.gamesLost = gamesLost;
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
        /// Gets or sets the Encrypted password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
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
        /// Gets the games won.
        /// </summary>
        /// <value>The games won.</value>
        public int GamesWon
        {
            get { return this.gamesWon; }
        }

        /// <summary>
        /// Gets the games lost.
        /// </summary>
        /// <value>The games lost.</value>
        public int GamesLost
        {
            get { return this.gamesLost; }
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="text">The text that will be encrypted.</param>
        public static void Encrypt(ref string text)
        {
            //// Object only used for Encryption
            GameAccount ga = new GameAccount();

            text = ga.Encrypt(text);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The text that will be decrypted.</param>
        public static void Decrypt(ref string text)
        {
            //// Object only used for Decryption
            GameAccount ga = new GameAccount();

            text = ga.Decrypt(text);
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

            this.username = username;
            this.password = password;
            this.profileImage = profileImage;
            this.balance = balance;
            this.gamesPlayed = gamesPlayed;
            updated = true;

            return updated;
        }

        /// <summary>
        /// Balances the change.
        /// </summary>
        /// <param name="value">The value.</param>
        public void BalanceChange(int value)
        {
            this.balance += value;
        }

        /// <summary>
        /// Increments Games won and games played.
        /// </summary>
        public void GameWon()
        {
            this.gamesPlayed++;
            this.gamesWon++;
        }

        /// <summary>
        /// Increments Games won and games played.
        /// </summary>
        public void GameLost()
        {
            this.gamesPlayed++;
            this.gamesLost++;
        }

        /// <summary>
        /// Encrypts the specified text.
        /// </summary>
        /// <param name="text">The text to be encrypted.</param>
        /// <returns>Encrypted text</returns>
        private string Encrypt(string text)
        {
            TripleDES tripleDES = new TripleDESCryptoServiceProvider();
            ICryptoTransform encryptor = tripleDES.CreateEncryptor(encryptionKey, initVector);
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] encryptedText = encoding.GetBytes(text);

            encryptedText = encryptor.TransformFinalBlock(encryptedText, 0, encryptedText.Length);

            return encoding.GetString(encryptedText);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The text to be decrypted.</param>
        /// <returns>Decrypted text</returns>
        private string Decrypt(string text)
        {
            TripleDES tripleDES = new TripleDESCryptoServiceProvider();
            ICryptoTransform decryptor = tripleDES.CreateDecryptor(encryptionKey, initVector);
            byte[] decryptedText = new byte[24];

            decryptor.TransformBlock(Convert.FromBase64String(text), 0, text.Length, decryptedText, 0);

            return Convert.ToBase64String(decryptedText);
        }
    }
}
