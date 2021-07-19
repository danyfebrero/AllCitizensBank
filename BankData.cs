using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AllCitizensBank
{
    public class BankData
    {
        private static int BankAccountSeed { get; set; }
        public List<string> AccountTypes = new() {"Checking", "Savings"}; //load the types from the stream

        public int GetNewAccountNumber()
        {
            return BankAccountSeed;
        }
        public void MakeNewSeed()
        {
            BankAccountSeed++;
        }
        public static List<User> LoadBankDataFromFile()
        {

            var file = new FileInfo(UserFileName());
            List<User> ListOfUsers = new();
            try
            {
                //checks if users.json exist and creates it if not
                if (!file.Exists)
                {
                    User AdminUser = new("Admin", "1234", "Administrator", null, 1111);
                    ListOfUsers.Add(AdminUser);
                    SerializeBankDataToFile(ListOfUsers);
                }
                ListOfUsers = DeserializeBankData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
            }
            return ListOfUsers;
        }

        public static void SerializeBankDataToFile(List<User> listOfUsers)
        {
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(UserFileName()))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, listOfUsers);
            }
        }
        public static List<User> DeserializeBankData()
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
            var fileName = Path.Combine(directory.FullName, "bankdata.json");
            return fileName;
        }
    }
}
