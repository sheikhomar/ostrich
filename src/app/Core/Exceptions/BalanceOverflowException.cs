using System;

namespace ostrich.Core.Exceptions
{
    public class BalanceOverflowException : Exception
    {
        public BalanceOverflowException(User user, int newBalance)
        {
            if (user == null) 
                throw new ArgumentNullException("user");

            User = user;
            NewBalance = newBalance;
        }

        public User User { get; set; }

        public int NewBalance { get; set; }

        public override string Message
        {
            get { return "User\'s maximum balance has been reached."; }
        }
    }
}