using System;

namespace AllCitizensBank
{
    class Program
    {
        static void Main(string[] args)
        {
            //welcome
            Console.Clear();
            Console.WriteLine("Welcome to All Citizens Bank");
            
            int optionTyped = 0;

            promptOptions();

            while (!(int.TryParse(Console.ReadLine(), out optionTyped)) || optionTyped < 1 || optionTyped > 4)
            {
                Console.WriteLine("");
                Console.WriteLine("Invalid option, please try again.");
                promptOptions();
            }

            //login, sign up, or quit
            if (optionTyped == 1)
            {

                //upload the user stream
                //check if the user id exist on the stream
                //if the user id doesnt exist dont check for the password
                //else check if the typed password match the stored password
                //then show the accounts owned by this user
                //give the option to select an account to work with
            }
            if (optionTyped == 2)
            {
                //open a new account
                //ask for full name, 
            }
            if (optionTyped == 3)
            {
                //recover user or password
            }
            if (optionTyped == 4)
            {
                Console.WriteLine("Good bye");
                System.Environment.Exit(1);
            }


            //show accounts under user, go back to login or quit



            //actions for the selected account
            //  deposit
            //  withdraw
            //  balance
            //  transfer
            //  open new account
            //  transactiion history
            //  select another account
            //  quit


        }

        private static void promptOptions()
        {
            Console.WriteLine("");
            Console.WriteLine("1: Login");
            Console.WriteLine("2: Open an Account");
            Console.WriteLine("3: Forgot User ID or Password");
            Console.WriteLine("4: Exit");
            Console.WriteLine("");
            Console.Write("Type your option: ");
        }
        private static void AccountPromptOptions()
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
    }
}
