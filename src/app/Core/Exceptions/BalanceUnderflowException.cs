namespace ostrich.Core.Exceptions
{
    public class BalanceUnderflowException : BalanceLimitException
    {
        public BalanceUnderflowException(User user, int newBalance)
            : base(user, newBalance)
        {
        }

        public override string Message
        {
            get { return string.Format("Balance of user '{0}' has underflowed.", User.UserName); }
        }
    }
}