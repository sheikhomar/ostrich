using System;

namespace ostrich.Core.Exceptions
{
    public class DuplicateTransactionException : Exception
    {
        public DuplicateTransactionException(Transaction transaction)
        {
            if (transaction == null) 
                throw new ArgumentNullException("transaction");

            Transaction = transaction;
        }

        public Transaction Transaction { get; private set; }

        public override string Message
        {
            get { return "Duplicate transactions are not allowed."; }
        }
    }
}