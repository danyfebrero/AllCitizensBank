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


        public Users(string user, string password, string firstName, string lastName, short pin, Account newAccount)
        {
            User = user;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Pin = pin;
            AddAccount(newAccount);
        }

        public void SaveUserToFile(Users userToSave)
        {
            //if the file doesnt exist create the file
            //else add the file to the user list
            //and save the user list to the json file
        }

        public bool UserNameAvaliable(string newUser)
        {
            //check if the user exist in the list loaded from the jason file and give the option to cancel
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
        public bool PasswordIsSecure(string newPassword)
        {
            //while the password doesnt have the requirement ask for a new password and give the option to cancel
            return (newPassword.Length > 3);
        }
        public void ChangePin(short newPin)
        {
            Pin = newPin;
        }
        public bool PinIsSecure(short pin)
        {
            return pin.ToString().Length == 4;
        }
        public void ChangeFirstName(string newFirstName)
        {
            FirstName = newFirstName;
        }
        public void ChangeLastName(string newLastName)
        {
            LastName = newLastName;
        }
        public void AddAccount(Account newAccount)
        {
            Accounts.Add(newAccount);
        }
        public void DeleteAccount(Account account)
        {
            Accounts.Remove(account);
        }
    }
}
