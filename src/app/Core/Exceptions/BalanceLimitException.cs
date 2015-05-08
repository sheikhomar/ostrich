using System;

namespace ostrich.Core.Exceptions
{
    public abstract class BalanceLimitException : Exception
    {
        protected BalanceLimitException(User user, int newBalance)
        {
            if (user == null) 
                throw new ArgumentNullException("user");

            User = user;
            NewBalance = newBalance;
        }

        public User User { get; set; }

        public int NewBalance { get; set; }
    }
}