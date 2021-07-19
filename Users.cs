using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace AllCitizensBank
{
    public class User
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public short Pin { get; set; }
        public List<Account> Accounts = new();


        public User(string user, string password, string firstName, string lastName, short pin)
        {
            UserId = user;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Pin = pin;
        }

        public bool UserNameAvaliable(List<User> users, string newUserId)
        {
            //check if the user exist in the list loaded from the json file and give the option to cancel
            var counter = 0;
            foreach (var user in users)
            {
                if (user.UserId == newUserId)
                {
                    counter++;
                }
            }
            return counter == 0 ;
        }
        public void ChangeUserName(string newUserName)
        {
            UserId = newUserName;
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

        public static List<User> LoadUsersFromFile()
        {

            var file = new FileInfo(UserFileName());
            List<User> ListOfUsers = new();
            try
            {
                //checks if users.json exist and creates it if not
                if (!file.Exists)
                {
                    User AdminUser = new("Admin","1234","Administrator",null,1111);
                    ListOfUsers.Add(AdminUser);
                    SerializeUserToFile(ListOfUsers);
                }
                ListOfUsers = DeserializeUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
            }
            return ListOfUsers;
        }

        public static void SerializeUserToFile(List<User> listOfUsers)
        {
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(UserFileName()))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, listOfUsers);
            }
        }
        public static List<User> DeserializeUsers()
        {
            var users = new List<User>();
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(UserFileName()))
            using (var jasonReader = new JsonTextReader(reader))
            {
                users = serializer.Deserialize<List<User>>(jasonReader);
            }
            return users;
        }
        public static string UserFileName()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "users.json");
            return fileName;
        }
    }
}
