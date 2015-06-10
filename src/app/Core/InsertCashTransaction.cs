using System;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(int id, User user, DateTime date, int amount)
            : base(id, user, date)
        {
            if (amount < 1)
                throw new ArgumentOutOfRangeException("amount", "Amount must be a positive integer.");

            Amount = amount;
        }

        public override void Execute()
        {
            int newBalance = User.Balance + Amount;
            if (User.Balance > 0 && newBalance < 0)
                throw new BalanceOverflowException(User, newBalance);

            User.Balance += Amount;
        }

        public override string ToString()
        {
            return string.Format("{0} Type=InsertCash", base.ToString());
        }
    }
}