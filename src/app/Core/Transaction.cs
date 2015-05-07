using System;
using System.IO;

namespace ostrich.Core
{
    public abstract class Transaction
    {
        protected Transaction(int id, User user, DateTime date)
        {
            if (user == null) 
                throw new ArgumentNullException("user");

            TransactionID = id;
            User = user;
            Date = date;
        }

        public int TransactionID { get; private set; }

        public User User { get; private set; }

        public DateTime Date { get; private set; }

        public int Amount { get; protected set; }

        public abstract void Execute();

        public override string ToString()
        {
            return string.Format("[{0:yyyy-MM-dd HH:mm:ss}] ID={1} UserID={2} Amount={3}", Date, TransactionID, User.UserID, Amount);
        }

        public void Log(StreamWriter writer)
        {
            writer.WriteLine(ToString());
        }
    }
}