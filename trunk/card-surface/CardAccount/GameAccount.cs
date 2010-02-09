using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace CardAccount
{
    class GameAccount
    {
        private string username;

        private string password;

        private string profileImage;

        private int balance;

        private int gamesPlayed;


        public string Username
        {
            get { return this.username; }
        }

        public string Password
        {
            get { return this.password; }
        }

    }
}
