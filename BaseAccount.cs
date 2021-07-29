using System;
using System.Collections.Generic;

namespace AllCitizensBank
{
    public class Account
    {
        public int AccountNumber { get; }
        public string AccountType { get; }
        public List<Transaction> allTransactions = new();
        public decimal Balance { get
            {
                decimal balance = 0;
                foreach (var transaction in allTransactions)
                {
                    balance += transaction.Amount; 
                }
                return balance;
            }
        }
        

        public Account(int acccountNumber, string accountType)
        {
            AccountNumber = acccountNumber;
            AccountType = accountType;
        }

        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            //add deposit to the current balance
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            
            var deposit = new Transaction(amount, date, note);
            allTransactions.Add(deposit);
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            // check if the withdraw is less than the current balance
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }
            if (Balance - amount < 0)
            {
                throw new InvalidOperationException("Insufficient founds for this withdrawal");
            }
            var withdrawal = new Transaction(-amount, date, note);
            allTransactions.Add(withdrawal);

        }

        public bool IsDecimal(string value)
        {
            return decimal.TryParse(value, out decimal n);
        }

    }
}
