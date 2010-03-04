// <copyright file="AccountController.cs" company="University of Louisville Speed School of Engineering">
// GNU General Public License v3
// </copyright>
// <summary>The AccountController is responsible for managing the systems users.</summary>
namespace CardAccount
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml;

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
        private string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

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
            bool success = true;
            
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
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught creating Game Account.", e);
                    success = false;
                }
            }

            return success;

            //// CreateFlatFile(file);
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
        /// Creates the flat file.
        /// </summary>
        /// <param name="file">The filepath to create the file.</param>
        /// <returns>whether the file was created.</returns>
        private bool CreateFlatFile(string file)
        {
            bool success = false;

            /*FileStream filestream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, 1000, true);
            GZipStream gzipstream = new GZipStream(filestream, CompressionMode.Compress);
            XmlWriter w = new XmlTextWriter(gzipstream, null);
            w.WriteStartDocument();
            w.WriteStartElement("FILE"); // START FILE
            w.WriteStartElement("Accounts"); // START ACCOUNTS
            for (int i = 0; i < this.users.Count; i++)
            {
                w.WriteStartElement("Account"); // START STUDENT
                w.WriteAttributeString("Username", this.users[i].Username);
                w.WriteAttributeString("Password", this.users[i].FirstName);
                w.WriteAttributeString("Last", this.users[i].LastName);
                w.WriteAttributeString("Section", this.users[i].Section);
                w.WriteAttributeString("Sid", this.users[i].Sid);
                w.WriteAttributeString("LeftHanded", this.users[i].LeftHanded.ToString());
                w.WriteAttributeString("VisionImpairment", this.users[i].VisionImpairment.ToString());
                w.WriteEndElement(); // END STUDENT
            }

            w.WriteEndElement(); // END STUDENTS
            w.WriteStartElement("Rooms"); // START ROOMS
            for (int i = 0; i < this.rooms.Count; i++)
            {
                w.WriteStartElement("Room"); // START ROOM
                w.WriteAttributeString("Name", this.rooms[i].RoomName);
                w.WriteAttributeString("Location", this.rooms[i].Location);
                w.WriteAttributeString("Description", this.rooms[i].Description);
                w.WriteAttributeString("Width", this.rooms[i].Width.ToString());
                w.WriteAttributeString("Height", this.rooms[i].Height.ToString());
                w.WriteStartElement("Chairs"); // START CHAIRS
                for (int j = 0; j < this.rooms[i].Height; j++)
                {
                    for (int k = 0; k < this.rooms[i].Width; k++)
                    {
                        Chair c = this.rooms[i].Chairs[j, k];
                        w.WriteStartElement("Chair"); // START CHAIR
                        w.WriteAttributeString("PosX", j.ToString());
                        w.WriteAttributeString("PosY", k.ToString());
                        w.WriteAttributeString("LeftHanded", c.LeftHanded.ToString());
                        w.WriteAttributeString("FbPosition", c.FbPosition.ToString());
                        w.WriteAttributeString("LrPosition", c.LrPosition.ToString());
                        w.WriteAttributeString("NonChair", c.NonChair.ToString());
                        w.WriteAttributeString("MustBeEmpty", c.MustBeEmpty.ToString());
                        w.WriteAttributeString("Name", c.SeatName);
                        if (c.TheStudent == null)
                        {
                            w.WriteAttributeString("SUID", new Guid().ToString());
                        }
                        else
                        {
                            w.WriteAttributeString("SUID", c.TheStudent.Uid.ToString());
                        }

                        w.WriteEndElement(); // END CHAIR
                    }
                }

                w.WriteEndElement(); // END CHAIRS
                w.WriteStartElement("RoomStudents"); // START ROOMSTUDENTS
                for (int j = 0; j < this.rooms[i].RoomStudents.Count; j++)
                {
                    w.WriteStartElement("RoomStudent"); // START ROOMSTUDENT
                    w.WriteAttributeString("SUID", this.rooms[i].RoomStudents[j].Uid.ToString());
                    w.WriteEndElement(); // END ROOMSTUDENT
                }

                w.WriteEndElement(); // END ROOMSTUDENTS
                w.WriteEndElement(); // END ROOM
            }

            w.WriteEndElement(); // END ROOMS
            w.WriteEndElement(); // END SEAT
            w.WriteEndDocument();
            w.Close();
            gzipstream.Close();
            filestream.Close();

            /* Mark the file as no longer dirty
            SeatManager.dirty = false;
            SeatManager.FileBecameDirty.Invoke(null, null);*/

            return success;
        }

        /// <summary>
        /// Updates the flat file.
        /// </summary>
        /// <param name="file">The filepath to update the file.</param>
        /// <returns>whether the file was updated.</returns>
        private bool UpdateFlatFile(string file)
        {
            bool success = false;

            // Read in the XML document and load all of the data into memory
            FileStream filestream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            GZipStream gzipstream = new GZipStream(filestream, CompressionMode.Decompress, true);
            XmlReader r = new XmlTextReader(gzipstream);
            while (r.Read())
            {
                if (r.NodeType == XmlNodeType.Element && r.Name == "Username")
                {
                    /*// Read the room's attributes and make a new instance of a room
                    this.width = Int32.Parse(r.GetAttribute("Width"));
                    this.height = Int32.Parse(r.GetAttribute("Height"));
                    this.roomName = r.GetAttribute("Name");
                    this.location = r.GetAttribute("Location");
                    this.description = r.GetAttribute("Description");
                    this.chairs = new Chair[this.height, this.width];

                    // Get all of the information contained in a room
                    while (!(r.NodeType == XmlNodeType.EndElement && r.Name == "Username"))
                    {
                        r.Read();

                        // Read in all of the chairs
                        if (r.NodeType == XmlNodeType.Element && r.Name == "Chairs")
                        {
                            while (!(r.NodeType == XmlNodeType.EndElement && r.Name == "Chairs"))
                            {
                                r.Read();

                                // Read in the information about a chair
                                if (r.NodeType == XmlNodeType.Element && r.Name == "Chair")
                                {
                                    // Get the position of the chair in the room
                                    int x = Int32.Parse(r.GetAttribute("PosX"));
                                    int y = Int32.Parse(r.GetAttribute("PosY"));

                                    // For the student, we assume the chair is blank.
                                    Student s = null;

                                    // Replace the chair from the default constructor with the information contained in the stored version of the chair
                                    this.Chairs[x, y] = new Chair(
                                        Boolean.Parse(r.GetAttribute("LeftHanded")),
                                        Int32.Parse(r.GetAttribute("FbPosition")),
                                        Int32.Parse(r.GetAttribute("LrPosition")),
                                        Boolean.Parse(r.GetAttribute("NonChair")),
                                        Boolean.Parse(r.GetAttribute("MustBeEmpty")),
                                        r.GetAttribute("Name"),
                                        s);
                                }
                            }
                        }
                    }*/
                }
            }

            r.Close();
            gzipstream.Close();
            filestream.Close();

            return success;
        }
    }
}