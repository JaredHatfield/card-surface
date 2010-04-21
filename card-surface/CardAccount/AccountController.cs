// <copyright file="AccountController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The AccountController is responsible for managing the systems users.</summary>
namespace CardAccount
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using AccountException;

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
        /// filepath of Flat file
        /// </summary>
        private string file = Directory.GetCurrentDirectory();

        /// <summary>
        /// Prevents a default instance of the AccountController class from being created.
        /// </summary>
        private AccountController()
        {
            this.file += "\\Accounts";
            this.ReadFlatFile(this.file);
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

            //// Encrypt password before comparison
            GameAccount.Encrypt(ref password);

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
            bool success = true;

            if (username.Length >= 4 || password.Length >= 4)
            {
                GameAccount.Encrypt(ref password);

                foreach (GameAccount acnt in this.users)
                {
                    if (acnt.Username == username)
                    {
                        success = false;
                    }
                }

                if (success)
                {
                    try
                    {
                        GameAccount account = new GameAccount(username, password);

                        this.users.Add(account);

                        success = this.CreateFlatFile(this.file);
                    }
                    catch (Exception)
                    {
                        throw new CardAccountCreationException();
                    }
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
        /// <returns>whether the account was updated.</returns>
        public bool UpdateAccount(string username, string password, string profileImage, int balance, int gamesPlayed)
        {
            bool updated = false;

            GameAccount.Encrypt(ref password);

            foreach (GameAccount account in this.users)
            {
                if (account.Username == username)
                {
                    updated = account.UpdateAccount(username, password, profileImage, balance, gamesPlayed);
                }
            }

            return updated;
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Close(object sender, EventArgs args)
        {
            // TODO: Write the account information to disk because the application is about to close
        }

        /// <summary>
        /// Creates the flat file.
        /// </summary>
        /// <param name="file">The filepath to create the file.</param>
        /// <returns>whether the file was created.</returns>
        private bool CreateFlatFile(string file)
        {
            bool success = true;

            if (File.Exists(file))
            {
                //// overwrite Backup file if exists
                File.Copy(file, file + "Backup", true);
            }

            try
            {
                FileStream filestream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, 1000, true);
                GZipStream gzipstream = new GZipStream(filestream, CompressionMode.Compress);
                XmlWriter w = new XmlTextWriter(gzipstream, null);
                w.WriteStartDocument();
                w.WriteStartElement("FILE"); // START FILE
                w.WriteStartElement("Accounts"); // START ACCOUNTS
                for (int i = 0; i < this.users.Count; i++)
                {
                    w.WriteStartElement("Account"); // START ACCOUNT
                    w.WriteAttributeString("Username", this.users[i].Username);
                    w.WriteAttributeString("Password", this.users[i].Password);
                    w.WriteAttributeString("Image", this.users[i].ProfileImage);
                    w.WriteAttributeString("Balance", this.users[i].Balance.ToString());
                    w.WriteAttributeString("GamesPlayed", this.users[i].GamesPlayed.ToString());
                    w.WriteAttributeString("GamesWon", this.users[i].GamesWon.ToString());
                    w.WriteAttributeString("GamesLost", this.users[i].GamesLost.ToString());
                    w.WriteEndElement(); // END ACCOUNT
                }

                w.WriteEndElement(); // END ACCOUNTS
                w.WriteEndElement(); // END FILE
                w.WriteEndDocument();
                w.Close();
                gzipstream.Close();
                filestream.Close();
            }
            catch (Exception)
            {
                throw new CardAccountFileAccessException();
            }

            return success;
        }

        /// <summary>
        /// Reads the flat file.
        /// </summary>
        /// <param name="file">Reads the Flat file if it exists.</param>
        /// <returns>whether the file was read successfully.</returns>
        private bool ReadFlatFile(string file)
        {
            bool success = false;
            FileStream filestream;

            if (File.Exists(file))
            {
                try
                {
                    //// Read in the XML document and load all of the data into memory
                    filestream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                    GZipStream gzipstream = new GZipStream(filestream, CompressionMode.Decompress, true);
                    XmlReader r = new XmlTextReader(gzipstream);
                    GameAccount newAccount;

                    while (r.Read())
                    {
                        if (r.NodeType == XmlNodeType.Element && r.Name == "Account")
                        {
                            string username, password, profileImage;
                            int balance, gamesPlayed, gamesWon, gamesLost;

                            //// Read the account's attributes and make a new instance of an account
                            username = r.GetAttribute("Username");
                            password = r.GetAttribute("Password");
                            profileImage = r.GetAttribute("Image");
                            balance = Int32.Parse(r.GetAttribute("Balance"));
                            gamesPlayed = Int32.Parse(r.GetAttribute("GamesPlayed"));
                            gamesWon = Int32.Parse(r.GetAttribute("GamesWon"));
                            gamesLost = Int32.Parse(r.GetAttribute("GamesLost"));
                            newAccount = new GameAccount(username, password, profileImage, balance, gamesPlayed, gamesWon, gamesLost);

                            this.users.Add(newAccount);
                        }
                    }

                    r.Close();
                    gzipstream.Close();
                    filestream.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error occurred writing to Flat file." + e.ToString());
                    success = false;

                    if (this.file == file)
                    {
                        Debug.WriteLine("Attempt to read from backup file.");
                        return this.ReadFlatFile(file + "Backup");
                    }
                    else
                    {
                        throw new CardAccountFileAccessException();
                    }
                }
            }
            else
            {
                success = this.CreateFlatFile(file);
            }

            return success;
        }
    }
}