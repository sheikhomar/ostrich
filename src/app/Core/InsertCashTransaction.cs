using System;

namespace ostrich.Core
{
    public class InsertCashTransaction : Transaction
    {
        public InsertCashTransaction(int id, User user, DateTime date, int amount)
            : base(id, user, date)
        {
            Amount = amount;
        }

        public override void Execute()
        {
            User.Balance += Amount;
        }

        public override string ToString()
        {
            return string.Format("{0} Type=InsertCash", base.ToString());
        }
    }
}