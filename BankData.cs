﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AllCitizensBank
{
    public class BankData
    {
        public static int BankAccountSeed { get; set; }

        public void MakeNewSeed()
        {
            BankAccountSeed++;
        }
        public static void LoadBankDataFromFile()
        {

            var file = new FileInfo(BankDataFileName());
            int bankAccountSeed = 1000000000; ;
            try
            {
                
                //checks if users.json exist and creates it if not
                if (!file.Exists)
                {
                    SerializeBankDataToFile(bankAccountSeed);
                }
                bankAccountSeed = DeserializeBankData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
            }
            BankAccountSeed = bankAccountSeed;
        }

        public static void SerializeBankDataToFile(int bankAccountSeed)
        {
            var serializer = new JsonSerializer();
            using (var writer = new StreamWriter(BankDataFileName()))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                serializer.Serialize(jsonWriter, bankAccountSeed);
            }
        }
        public static int DeserializeBankData()
        {
            int bankAccountSeed;
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(BankDataFileName()))
            using (var jasonReader = new JsonTextReader(reader))
            {
                bankAccountSeed = serializer.Deserialize<int>(jasonReader);
            }
            return bankAccountSeed;
        }
        public static string BankDataFileName()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "bankdata.json");
            return fileName;
        }
    }
}
