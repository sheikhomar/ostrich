namespace ostrich.Core.Exceptions
{
    public class BalanceOverflowException : BalanceLimitException
    {
        public BalanceOverflowException(User user, int newBalance) 
            : base(user, newBalance)
        {
        }

        public override string Message
        {
            get { return string.Format("Balance of user '{0}' has overflowed.", User.UserName); }
        }
    }
}