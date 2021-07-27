﻿using System;
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
                    if (ActiveUser != null)
                        AccountSelector();
                    break;
                case "2":
                    NewUser();
                    AddNewAccount();
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
            do
            {
                ShowBankLogo("Login");
                Console.Write("User ID: ");
                _userId = Console.ReadLine().ToLower();
                Console.Write("Password: ");
                _password = Console.ReadLine();

                // check if the user id exist on list
                if ( User.UserNameOnList( ListOfUsers, _userId )) //if the user exist on the list
                {
                    //select the user
                    var userIndex = ListOfUsers.FindIndex( u => u.UserId == _userId );
                    //check the pasword
                    if ( ListOfUsers[userIndex].Password ==  _password )
                    {
                        counter = 3;
                        ActiveUser = _userId;
                        continue;
                    }
                    else
                    {
                        if (counter < 2)
                        {
                            Console.WriteLine("Password incorrect. Press any key to return to try again");
                            Console.ReadKey();
                        }
                        else
                        {
                            ActiveUser = null;
                            Console.WriteLine("Password incorrect. Press any key to return to main menu");
                            Console.ReadKey();
                        }
                    }

                }
                else
                {
                    if (counter < 2)
                    {
                        Console.WriteLine("User ID incorrect. Press any key to return to try again");
                        Console.ReadKey();
                    }
                    else
                    {
                        ActiveUser = null;
                        Console.WriteLine("User ID incorrect. Press any key to return to main menu");
                        Console.ReadKey();
                    }
                }
                counter++;
            }
            while (counter < 3);
        }
        private static void AccountSelector()
        {
            // todo user a linq to show the accounts owned by this user, if not show this user doesnt have accounts
            Console.Clear();
            ShowBankLogo($"Accounts");
            if (ListOfUsers[ActiveUserIndex].Accounts == null)
            {
                Console.WriteLine("You haven't opened an account yet.");
                string makeNewAccount;
                bool endLoop = false;
                do
                {
                    Console.Write("Do you want to open a new account now?: ");
                    makeNewAccount = Console.ReadLine().ToLower();
                    switch (makeNewAccount[0])
                    {
                        case 'y':
                            AddNewAccount();
                            endLoop = true;
                            break;
                        case 'n':
                            Console.WriteLine("This user doesn't have accounts, goin back to main menu.");
                            ActiveUser = null;
                            Console.ReadKey();

                            endLoop = true;
                            //press any key to go back to main menu
                            break;
                        case 'q':
                            System.Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Please enter yes, no, or quit.");
                            break;
                    }
                }
                while ( endLoop == false );
                
            }
            else
            {
                string selectedAccountIndex;
                int i = 0;
                foreach (var account in ListOfUsers[ActiveUserIndex].Accounts)
                {
                    
                    Console.WriteLine("______________________________");
                    Console.WriteLine($"Account {account}: {i}");
                    Console.WriteLine($"{account.AccountType}: {account.AccountNumber}\t\t{account.Balance}");
                    Console.WriteLine($"As of: {DateTime.Now}\t\tAvailable Balance");
                    i++;

                }
                // give the option to select an account to work with
                do
                {
                    Console.Write("Select an Account or type quit to exit: ");
                    selectedAccountIndex = Console.ReadLine().ToLower();
                    if (selectedAccountIndex[0] == 'q')
                        System.Environment.Exit(0);
                    else
                    {

                    }
                }
                while ((!int.TryParse(selectedAccountIndex, out int temp)) && ( temp < 0 || temp > ListOfUsers[ActiveUserIndex].Accounts.Count - 1));

                
            }



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

            Console.Write("User ID (must have 4 characters with no blankspaces): ");
            userId = Console.ReadLine().ToLower();

            Console.Write("Password (must have more than 4 characters): ");
            pass = Console.ReadLine();
            Console.Write("Repeat Password: ");
            repeatPass = Console.ReadLine();
            
            Console.Write("Pin(must have 4 digits): ");
            pin = Console.ReadLine();
            Console.Write("Pin: ");
            repeatPin = Console.ReadLine();

            while (name.Any(char.IsDigit) || String.IsNullOrEmpty(name))
            {
                
                Console.WriteLine("Invalid First Name.");
                Console.Write("First Name: ");
                name = Console.ReadLine();
                
            }
            while (lastName.Any(char.IsDigit) || String.IsNullOrEmpty(name))
            {
               
                Console.WriteLine("Invalid Last Name.");
                Console.Write("Last Name: ");
                lastName = Console.ReadLine();

            }
            while (userId.Length < 4 || User.UserNameOnList(ListOfUsers,userId) || userId.Contains(" ") || String.IsNullOrEmpty(name))
            {
                
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
            while (pin.Length != 4 || pin != repeatPin || !pin.All(char.IsDigit))
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
            string userResponse;
            var endLoop = false;
            do
            {
                Console.WriteLine("What type of account do you want to open:");
                Console.WriteLine("1: Cheking");
                Console.WriteLine("2: Saving");
                Console.WriteLine("3: Go Back");
                Console.WriteLine("Please select an option:\n");
                userResponse = Console.ReadLine();
                switch (userResponse)
                {
                    case "1": //checking account
                        NewAccount("Checking");

                        break;
                    case "2": //saving acccount
                        NewAccount("Savings");
                        break;
                    case "3": // no account
                        AccountSelector();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Please enter 1, 2, or 3 next time. Press any key to return");
                        Console.ReadKey();
                        break;
                }
                if (userResponse == "1" || userResponse == "2" || userResponse == "3")
                    endLoop = true;
            }
            while (endLoop == false);
        }

        public static void RecoverPassOrId()
        {
            //todo to recover user or password
            string option;
            string firstName, lastName, pin, userId, newPassword, repeatedPassword;
            int intent = 0;
            var endLoop = false;
            do
            {
                Console.Clear();
                ShowBankLogo("Recovery Menu");
                Console.WriteLine("");
                Console.WriteLine("1: Recover your password");
                Console.WriteLine("2: Recover your User ID");
                Console.WriteLine("3: Back to main menu");
                Console.WriteLine("");
                Console.Write("Type your option: ");
                option = Console.ReadLine();
                Console.Clear();
                if (option == "3")
                    break;
                if (option != "1" && option != "2")
                    {
                        Console.WriteLine("Invalid option, please try again");
                        Console.ReadKey();
                        continue;
                    }
                    
                Console.WriteLine("Please introduce your first name: ");
                firstName = Console.ReadLine().ToLower();
                Console.WriteLine("Please introduce your last name: ");
                lastName = Console.ReadLine().ToLower();
                // check if there is some users with that name and last name
                var filteredUsers = ListOfUsers.Where(u => u.FirstName == firstName).Where(u => u.LastName == lastName);
                if (filteredUsers.Count() == 0)
                {
                    Console.WriteLine($"There is no user vinculated to {firstName} {lastName}");
                    Console.WriteLine($"Press any key to go back to main menu");
                    Console.ReadKey();
                    break;
                }
                // else continue 

                switch (option)
                {
                    case "1":
                        int passRecoveryCounter = 0;
                        do
                        {
                            Console.WriteLine("Please introduce your User ID: ");
                            userId = Console.ReadLine().ToLower();
                            if (User.UserNameOnList(ListOfUsers, userId)) //the user exist
                            {
                                var indexOfUser = ListOfUsers.FindIndex(u => u.UserId == userId);
                                if (userId == ListOfUsers[indexOfUser].UserId
                                    && firstName == ListOfUsers[indexOfUser].FirstName
                                    && lastName == ListOfUsers[indexOfUser].LastName)
                                {
                                    do
                                    {
                                        Console.WriteLine("Type your new password: ");
                                        newPassword = Console.ReadLine();
                                        Console.WriteLine("Retype your new password: ");
                                        repeatedPassword = Console.ReadLine();

                                        if (repeatedPassword == newPassword)
                                        {
                                            ListOfUsers[indexOfUser].Password = newPassword;
                                            User.SaveUsersToFile(ListOfUsers);
                                            passRecoveryCounter = 3;
                                        }
                                        else
                                        {
                                            intent++;
                                            Console.WriteLine("The passwords do not match");
                                            Console.ReadKey();
                                            if (intent == 3)
                                                passRecoveryCounter = 3;
                                        }
                                    }
                                    while (newPassword != repeatedPassword || intent < 3);
                                }
                                else
                                {
                                    Console.WriteLine("The first and/or the last name are incorrect");
                                    Console.ReadKey();
                                }

                            }
                            else
                            {
                                Console.WriteLine($"The {userId} user ID does't exist. Press any key to try again.");
                                Console.ReadKey();
                            }
                            passRecoveryCounter++;
                        }
                        while ( passRecoveryCounter < 3);
                        endLoop = true;
                        break;
                    case "2": 
                        var userRecoveryCounter = 0;
                        do
                        {
                            Console.WriteLine("Please introduce your Pin: ");
                            pin = Console.ReadLine().ToLower();
                            
                            // if one user have same name, last name, and pin
                            // give the user id to the user
                            var result = ListOfUsers.Where(u => u.FirstName == firstName).Where(u => u.LastName == lastName).Where(u => u.Pin == pin);
                            if (result.Count() == 0)
                            {
                                Console.WriteLine($"Invalid pin, please try again. Attempt {userRecoveryCounter}");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                foreach (var item in result)
                                {
                                    Console.WriteLine($"Your User ID is: {item.UserId}");
                                    Console.ReadKey();
                                    userRecoveryCounter = 3;
                                }
                            }
                                
                            userRecoveryCounter++;
                        }
                        while((pin.Length != 4 && !pin.All(char.IsDigit)) || userRecoveryCounter < 3);
                        endLoop = true;
                        break;
                    case "3":
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Please enter 1, 2, or 3 next time. Press any key to return to the menu");
                        Console.ReadKey();
                        break;
                }
                if (option == "1" || option == "2")
                    endLoop = true;
            }
            while (endLoop == false);
            

            
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
