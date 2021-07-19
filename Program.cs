using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AllCitizensBank
{
    class Program
    {
        static void Main(string[] args)
        {
            //todo load user.json
            List<User> ListOfUsers = User.LoadUsersFromFile();
            Console.WriteLine("Loading...");

            //todo load bankdata

            //todo load accounts
            //


            //welcome and main menu
            do
                MainMenu();
            while (true);
        }



        private static void MainMenu()
        {
            ShowBankLogo("Main Menu");
            ShowMainMenu();

            string optionTyped = null;
            optionTyped = Console.ReadLine();

            //login, sign up, or quit
            switch (optionTyped)
            {
                case "1":
                    LoginMenu();
                    break;
                case "2":
                    NewAccount();
                    break;
                case "3":
                    RecoverPassOrId();
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("Good bye");
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Please enter 1, 2, 3, or 4 next time. Press any key to return to main menu");
                    Console.ReadKey();
                    break;
            }
        }
        private static void LoginMenu()
        {
            var counter = 0;
            do
            {

                ShowBankLogo("Login");
                Console.Write("User ID: ");
                string _userId = Console.ReadLine();
                Console.Write("Password: ");
                string _password = Console.ReadLine();
                //check if the user id exist on users.json
                //if the user id doesnt exist dont check for the password
                //else check if the typed password match the stored password

                if (_userId == "Admin" && _password == "1234")
                {
                    counter = 3;
                    Console.WriteLine("should show all Accounts");
                    Console.ReadKey();
                    AccountSelector(_userId);
                }
                else
                {
                    Console.WriteLine("The password typed is incorrect.");
                    if (counter == 2)
                        Console.WriteLine("Press any key to return to main menu.");
                    else
                        Console.WriteLine("Press any key to try again.");
                    Console.ReadKey();

                }
                counter++;
            }
            while (counter < 3); 
        }
        private static void AccountSelector(string userID)
        {
            // user a linq to show the accounts owned by this user, if not show this user doesnt have accounts

            // give the option to select an account to work with
            
            Console.WriteLine("");
            Console.WriteLine("1: Deposit");
            Console.WriteLine("2: Withdrawal");
            Console.WriteLine("3: Check Balance");
            Console.WriteLine("4: Transfer");
            Console.WriteLine("5: Open New Account");
            Console.WriteLine("6: Transaction History");
            Console.WriteLine("7: Select Another Account");
            Console.WriteLine("8: Exit");
            Console.WriteLine("");
            Console.Write("Type your option: ");
        }
        public static void ShowBankLogo(string menuType)
        {
            Console.Clear();
            Console.WriteLine("\n----------------------------------");
            Console.WriteLine("\tALL CITIZENS BANK");
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"\n\t--- {menuType} ---\n");
        }
        public static void ShowMainMenu()
        {
            Console.WriteLine("Please select an option:\n");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Open an Account");
            Console.WriteLine("3: Forgot User ID or Password");
            Console.WriteLine("4: Exit");

        }
        public static void NewAccount()
        {
            //open a new account
            //ask for full name, user(check avaliability), password, and pin(for recover password and user)
            //ask for type of account to open and show options to decide
            //then auto-login

        }
        public static void RecoverPassOrId()
        {
            //to recover user or password
            //ask for full name and pin to give the option of make a new user and password
            //auto login

        }
    }
}
