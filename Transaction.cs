using System;
namespace AllCitizensBank
{
    public class Transaction
    {
        public decimal Amount;
        public DateTime Date;
        public string Note;

        public Transaction(decimal amount, DateTime date, string note)
        {
            this.Amount = amount;
            this.Date = date;
            this.Note = note;
        }
    }
}
