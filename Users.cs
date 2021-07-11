using System;
using System.Collections.Generic;

namespace AllCitizensBank
{
    public class Users
    {
        private string User { get; set; }
        private string Password { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private short Pin { get; set; }
        private List<Account> Accounts = new();


        public Users()
        {
        }

        public bool UserNameAvaliable(string newUser)
        {
            //check if the user already exist
            return true;
        }
        public void ChangeUserName(string newUserName)
        {
            User = newUserName;
        }
        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
        }
        public void CheckUserName(string newUserName)
        {
            //while the user name is not avaliable ask for a new user name and give the option to cancel
        }
        public void CheckPassword(string newPassword)
        {
            //while the password doesnt have the requirement ask for a new password and give the option to cancel
        }
        public void ChangePin(short newPin)
        {
            Pin = newPin;
        }
    }
}
