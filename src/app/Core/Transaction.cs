using System;

namespace ostrich.Core
{
    public abstract class Transaction
    {
        protected Transaction(int id, User user, DateTime date, int amount)
        {
            if (user == null) throw new ArgumentNullException("user");

            TransactionID = id;
            User = user;
            Date = date;
            Amount = amount;
        }

        public int TransactionID { get; private set; }

        public User User { get; private set; }

        public DateTime Date { get; private set; }

        public int Amount { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0:yyyy-MM-dd HH:mm:ss}] ID={1} Amount={2}", Date, TransactionID, Amount);
        }

        public abstract void Execute();
    }
}