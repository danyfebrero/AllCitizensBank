using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace AllCitizensBank
{
    class Program
    {
        // load user.json
        public static List<User> ListOfUsers = User.LoadUsersFromFile();
        public static string ActiveUser;
        public static int ActiveUserIndex { get
            {
                return ListOfUsers.FindIndex(u => u.UserId == ActiveUser);
            }
        }

        static void Main(string[] args)
        {
            
            Console.WriteLine("Loading...");

            // load bankdata
            BankData.LoadBankDataFromFile();


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

                    //Console.WriteLine(ActiveUser.UserId);
                    //Console.ReadKey();
                    //Environment.Exit(0);
                    break;
                case "2":
                    NewUser();
                    AddNewAccount();

                    //Console.WriteLine(ActiveUser.UserId);
                    //Console.ReadKey();
                    //Environment.Exit(0);
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
            string _userId, _password;
            string selectedUser ;
            do
            {
                ShowBankLogo("Login");
                Console.Write("User ID: ");
                _userId = Console.ReadLine();
                Console.Write("Password: ");
                _password = Console.ReadLine();

                // check if the user id exist on list
                if (User.UserNameAvaliable(ListOfUsers, _userId))
                {
                    //select the user
                    selectedUser = ListOfUsers.Single(u => u.UserId == _userId).UserId;
                    //check the pasword
                    if (_password == ListOfUsers[ActiveUserIndex].Password)
                    {
                        counter = 3;
                        ActiveUser = selectedUser;
                        continue;
                    }

                }
                if (counter < 2)
                {
                    Console.WriteLine("User ID or password incorrect. Press any key to return to try again");
                    Console.ReadKey();
                }
                else
                {
                    selectedUser = null;
                    Console.WriteLine("User ID or password incorrect. Press any key to return to main menu");
                    Console.ReadKey();
                }
                counter++;
            }
            while (counter < 3);
        }
        private static void AccountSelector(string userID)
        {
            // todo user a linq to show the accounts owned by this user, if not show this user doesnt have accounts
            if()
            {

            }
            // give the option to select an account to work with
            
            
        }
        private static void AccountMenu()
        {
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
            Console.WriteLine("2: Sign Up");
            Console.WriteLine("3: Forgot User ID or Password");
            Console.WriteLine("4: Exit");
            Console.Write("\nOption: ");

        }
        public static void NewUser()
        {
            string name, lastName, userId, pass, repeatPass, pin, repeatPin;

            //open a new account
            ShowBankLogo("Sign Up");

            //ask for full name, user(check avaliability), password, and pin(for recover password and user)
            Console.Write("First Name: ");
            name = Console.ReadLine();
           

            Console.Write("Last Name: ");
            lastName = Console.ReadLine();

            Console.Write("User ID (more than 4 characters with no blankspaces): ");
            userId = Console.ReadLine().ToLower();

            Console.Write("Password (more than 4 characters): ");
            pass = Console.ReadLine();
            Console.Write("Repeat Password: ");
            repeatPass = Console.ReadLine();
            
            Console.Write("Pin(must have 4 digits): ");
            pin = Console.ReadLine();
            Console.Write("Pin: ");
            repeatPin = Console.ReadLine();

            while (name.Any(char.IsDigit))
            {
                //mostrar mensaje de nombre invaldo y volver a preguntar
                Console.WriteLine("Invalid First Name.");
                Console.Write("First Name: ");
                name = Console.ReadLine();
                
            }
            while (lastName.Any(char.IsDigit))
            {
                //mostrar mensaje de apellido invaldo y volver a preguntar
                Console.WriteLine("Invalid Last Name.");
                Console.Write("Last Name: ");
                lastName = Console.ReadLine();

            }
            while (userId.Length < 4 || User.UserNameAvaliable(ListOfUsers,userId)|| userId.Contains(" "))
            {
                // mostrar mnaje de usuario invaldo y volver a preguntar
                Console.WriteLine("Invalid or Unavailable User Id.");
                Console.Write("User ID (more than 4 characters with no blankspaces): ");
                userId = Console.ReadLine();
            }
            while (pass.Length < 4 || pass != repeatPass)
            {
                Console.WriteLine("Invalid Password.");
                Console.Write("Password (more than 4 characters): ");
                pass = Console.ReadLine();
                Console.Write("Repeat Password: ");
                repeatPass = Console.ReadLine();
            }
            while (pin.Length != 4 || pin != repeatPin)
            {
                Console.Write("Pin(must have 4 digits): ");
                pin = Console.ReadLine();
                Console.Write("Pin: ");
                repeatPin = Console.ReadLine();
            }

            var newUser = new User(userId, pass, name, lastName, pin ,null);

            ListOfUsers.Add(newUser);
            User.SaveUsersToFile(ListOfUsers);
            ActiveUser = newUser.UserId;
        }

        public static void AddNewAccount()
        {
            var LoopEnd = false;
            do
            {
                Console.WriteLine("What type of account do you want to open:");
                Console.WriteLine("1: Cheking");
                Console.WriteLine("2: Saving");
                Console.WriteLine("3: Back to Main Menu");
                Console.WriteLine("Please select an option:\n");
                string userResponse = Console.ReadLine();
                if (userResponse == "1" || userResponse == "2" || userResponse == "3")
                    LoopEnd = true;
                switch (userResponse)
                {
                    case "1": //checking account
                        NewAccount("Checking");
                        break;
                    case "2": //saving acccount
                        NewAccount("Savings");
                        break;
                    case "3": // no account
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Please enter 1, 2, or 3 next time. Press any key to return");
                        Console.ReadKey();
                        break;
                }
            }
            while (LoopEnd == false);
        }

        public static void RecoverPassOrId()
        {
            //todo to recover user or password
            Console.WriteLine("");
            Console.WriteLine("1: Recover your password");
            Console.WriteLine("2: Recover your User ID");
            Console.WriteLine("3: Back to main menu");
            Console.WriteLine("");
            Console.Write("Type your option: ");

            //ask for full name and pin to give the option of make a new user and password
            //auto login

        }
        public static void NewAccount(string accountType)
        {
            string answer;
            do
            {
                Console.WriteLine("Do you want to make an initial deposit?");
                answer = Console.ReadLine();
                decimal amount = 0;

                if (answer[0].ToString().ToLower() == "y")
                {
                    var repeat = true;
                    do
                    {
                        Console.WriteLine("Enter deposit amount: ");
                        if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
                        {
                            repeat = false;
                        }
                        else
                            Console.WriteLine("Invalid amount");
                    }
                    while (repeat == true);
                }

                var newAccountnumber = BankData.BankAccountSeed;
                BankData.MakeNewSeed();
                var newAccount = new Account(newAccountnumber, accountType, amount);
                BankData.SaveBankDataToFile();
                //var index = ListOfUsers.FindIndex(u => u.UserId == ActiveUser);
                ListOfUsers[ActiveUserIndex].Accounts.Add(newAccount);
                User.SaveUsersToFile(ListOfUsers);
                Console.WriteLine($"{ListOfUsers[ActiveUserIndex].FirstName} {ListOfUsers[ActiveUserIndex].LastName} added a new {newAccount.AccountType} account with a initial deposit of {newAccount.Balance}");
            }
            while ((answer[0].ToString().ToLower() != "y") || (answer[0].ToString().ToLower() != "n"));
        }
    }
}
