using System;
using System.Collections.Generic;
using System.Text;

namespace GoDexData.Model
{
   public  class User
    {
        private int userID;
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string role;

        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        public User(string sUserName, string sPassword)
        {
            userName = sUserName;
            password = sPassword;
        }
    }
}
