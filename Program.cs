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
                    if (ActiveUser != null)
                        AccountSelector();
                    break;
                case "2":
                    NewUser();
                    AddNewAccount();
                    AccountSelector();
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
            
            Console.Clear();
            ShowBankLogo($"Accounts");
            Console.WriteLine("----------------------------------");
            if (ListOfUsers[ActiveUserIndex].Accounts.Count == 0)
            {
                
                string makeNewAccount;
                bool endLoop = false;
                do
                {
                    Console.WriteLine("----------------------------------\n");
                    Console.WriteLine($"\tNo account to show\n");
                    Console.WriteLine("1: Open a new account.");
                    Console.WriteLine("2: Back to main menu.");
                    Console.WriteLine("3: Exit.");
                    Console.Write("Type your option: ");
                    makeNewAccount = Console.ReadLine().ToLower();
                    switch (makeNewAccount)
                    {
                        case "1":
                            AddNewAccount();
                            endLoop = true;
                            break;
                        case "2":
                            ActiveUser = null;
                            endLoop = true;
                            break;
                        case "3":
                            Console.Clear();
                            System.Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Please enter 1, 2, or 3.");
                            break;
                    }
                }
                while ( endLoop == false );
            }
            else
            {

                //todo chages the account number
                string selectedAccountIndex;
                int i;
                bool correctAnswer = false;
                do
                {
                    i = 0;
                    foreach (var account in ListOfUsers[ActiveUserIndex].Accounts)
                    {
                        i++;
                        //i = ListOfUsers[ActiveUserIndex].Accounts.FindIndex(a => a.AccountNumber == account.AccountNumber)+1;
                        Console.WriteLine("__________________________________");
                        Console.WriteLine($"Account: {i}\n");
                        Console.WriteLine($"{account.AccountType} Account");
                        Console.WriteLine($"Available Balance: {account.Balance}");
                        Console.WriteLine($"As of: {DateTime.Now.ToString("dd/MM/yyyy")}");
                        
                        
                    }
                    Console.Write("\nEnter Account number, New (to add a accounts), Back (to go back) or Quit (to exit): ");
                    selectedAccountIndex = Console.ReadLine().ToLower();

                    if (selectedAccountIndex[0] == 'b')
                    {
                        Console.Clear();

                        ActiveUser = null;
                        break;
                    }
                    else if (selectedAccountIndex[0] == 'n')
                    {
                        AddNewAccount();
                        Console.Clear();
                    }
                    else if (selectedAccountIndex[0] == 'q')
                    {
                        Console.Clear();
                        System.Environment.Exit(0);
                    }
                    else if (int.TryParse(selectedAccountIndex, out int tempAccountIndex))
                    {
                        tempAccountIndex = tempAccountIndex - 1;
                        if (tempAccountIndex > -1 && tempAccountIndex < ListOfUsers[ActiveUserIndex].Accounts.Count)
                        {
                            Console.Clear();
                            AccountMenu(tempAccountIndex);
                        }
                        else
                        {
                            Console.WriteLine($"{selectedAccountIndex} is an invalid account.");
                            Console.ReadKey();
                        }

                    }
                    else
                    {
                        Console.WriteLine($"{selectedAccountIndex} is an invalid option.");
                        Console.ReadKey();
                        
                    }
                    Console.Clear();
                    
                }
                while (correctAnswer == false);
            }
        }
        private static void AccountMenu(int ActiveAccountIndex)
        {
            string selectedOption= null;
            bool endLoop = false;
            do
            {
                ShowBankLogo(ListOfUsers[ActiveUserIndex].Accounts[ActiveAccountIndex].AccountType);
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Avaliable Balance\t\t{ListOfUsers[ActiveUserIndex].Accounts[ActiveAccountIndex].Balance}");
                Console.WriteLine("\n1: Deposit");
                Console.WriteLine("2: Withdrawal");
                Console.WriteLine("3: Transaction History");
                Console.WriteLine("4: Account Information");
                Console.WriteLine("5: Back to Accounts menu");
                Console.WriteLine("6: Exit");
                Console.Write("\nType your option: ");
                selectedOption = Console.ReadLine();
                if (int.TryParse(selectedOption, out int temp) == true && (temp > 0 && temp < 7))
                {
                    
                    switch(temp)
                    {
                        case 1:
                            //ask for the amount for deposit
                            Console.Write("Deposit amount: ");
                            decimal amountToDeposit = 0;
                            decimal.TryParse(Console.ReadLine(), out amountToDeposit);
                            if (amountToDeposit > 0 && amountToDeposit < decimal.MaxValue)
                            {
                                //make deposit
                                
                                bool endWhile = false;
                                string note = null;
                                do
                                {
                                    Console.WriteLine("Do you want to add a note?");
                                    Console.Write("Yes / No : ");
                                    var answer = Console.ReadLine().ToLower();
                                    switch (answer[0])
                                    {
                                        case 'y':
                                            Console.Write("Note: ");
                                            note = Console.ReadLine();
                                            endWhile = true;
                                            break;
                                        case 'n':
                                            endWhile = true;
                                            break;
                                        default:
                                            Console.WriteLine("Please try again.");
                                            break;
                                    }
                                }
                                while (endWhile == false);
                                ListOfUsers[ActiveUserIndex].Accounts[ActiveAccountIndex].MakeDeposit(amountToDeposit, DateTime.Now, note);
                                User.SaveUsersToFile(ListOfUsers);
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount. Please try again");
                            }
                            break;
                        case 2:
                            Console.Write("Withdrawal amount: ");
                            decimal amountToWithdraw = 0;
                            decimal.TryParse(Console.ReadLine(), out amountToWithdraw);
                            if (amountToWithdraw > 0 && amountToWithdraw <= ListOfUsers[ActiveUserIndex].Accounts[ActiveAccountIndex].Balance)
                            {
                                //make deposit
                                bool endWhile = false;
                                string note = null;
                                do
                                {
                                    Console.WriteLine("Do you want to add a note?");
                                    Console.Write("Yes / No : ");
                                    var answer = Console.ReadLine().ToLower();
                                    switch (answer[0])
                                    {
                                        case 'y':
                                            Console.Write("Note: ");
                                            note = Console.ReadLine();
                                            endWhile = true;
                                            break;
                                        case 'n':
                                            endWhile = true;
                                            break;
                                        default:
                                            Console.WriteLine("Please try again.");
                                            break;
                                    }
                                }
                                while (endWhile == false);
                                ListOfUsers[ActiveUserIndex].Accounts[ActiveAccountIndex].MakeWithdrawal(amountToWithdraw, DateTime.Now, note);
                                User.SaveUsersToFile(ListOfUsers);
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount. Please try again");
                            }
                            break;
                        case 3:
                            //show the transactions

                            Console.Clear();
                            Console.WriteLine("Transactions: \n");
                            foreach (var item in ListOfUsers[ActiveUserIndex].Accounts[ActiveAccountIndex].allTransactions)
                            {
                                Console.WriteLine($"{item.Note}\n{item.Date}\t\t{item.Amount}");
                                Console.WriteLine("__________________________________");
                            }
                            Console.WriteLine("Press any key to go back to the account: ");
                            Console.ReadKey();
                            break;
                        case 4:
                            //todo show account number 

                            Console.WriteLine($"\nAccount number: {ListOfUsers[ActiveUserIndex].Accounts[ActiveAccountIndex].AccountNumber}");
                            Console.ReadKey();                           
                            break;
                        case 5:
                            endLoop = true;
                            Console.Clear();
                            break;
                        case 6:
                            Console.Clear();
                            System.Environment.Exit(0);
                            break;
                        default:
                            break;
                    }

                }
                else
                {
                    Console.WriteLine("Invalid option, try again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            while (endLoop == false);

            
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
            
            Console.Write("Pin (must have 4 digits): ");
            pin = Console.ReadLine();
            Console.Write("Repeat Pin: ");
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
                Console.Write("Pin (must have 4 digits): ");
                pin = Console.ReadLine();
                Console.Write("Repeat Pin: ");
                repeatPin = Console.ReadLine();
            }

            var newUser = new User(userId, pass, name, lastName, pin ,new());

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
            bool endLoop = false;
            do
            {
                Console.WriteLine("Do you want to make an initial deposit?");
                Console.Write("Yes/No: ");
                answer = Console.ReadLine();
                decimal amount = 0;
                if ((answer[0].ToString().ToLower() == "y") || (answer[0].ToString().ToLower() == "n"))
                    endLoop = true;
                    if (answer[0].ToString().ToLower() == "y")
                {
                    var repeat = true;
                    do
                    {
                        Console.Write("Enter deposit amount: ");
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
                Account newAccount = new(newAccountnumber, accountType);
                
                //var index = ListOfUsers.FindIndex(u => u.UserId == ActiveUser);
                ListOfUsers[ActiveUserIndex].Accounts.Add(newAccount);
                int indexNewAccount = ListOfUsers[ActiveUserIndex].Accounts.FindIndex(a => a.AccountNumber == newAccountnumber);
                
                if (amount > 0)
                {
                    ListOfUsers[ActiveUserIndex].Accounts[indexNewAccount].MakeDeposit(amount, DateTime.Now, "Initial Deposit");
                }
                User.SaveUsersToFile(ListOfUsers);
                BankData.SaveBankDataToFile();
                Console.WriteLine($"Added a new {newAccount.AccountType} account with a initial deposit of {newAccount.Balance}");
                Console.WriteLine($"Press any key to continue.");
                Console.ReadKey();


            }
            while (endLoop == false); ;
        }
    }
}
