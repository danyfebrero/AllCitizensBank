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
        public string Pin { get; set; }
        public List<Account> Accounts = new();


        public User(string user, string password, string firstName, string lastName, string pin, List<Account> account)
        {
            UserId = user;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Pin = pin;
            Accounts = account;
        }

        public static bool UserNameAvaliable(List<User> users, string newUserId)
        {
            //check if the user exist in the list loaded from the json file and give the option to cancel
            return users.Any(user => user.UserId.ToLower() == newUserId.ToLower());
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
                    var AdminUser = new User("admin","1234","Administrator",null,"1111",null);
                    ListOfUsers.Add(AdminUser);
                    SaveUsersToFile(ListOfUsers);
                }
                ListOfUsers = DeserializeUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
            }
            return ListOfUsers;
        }

        public static void SaveUsersToFile(List<User> listOfUsers)
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
