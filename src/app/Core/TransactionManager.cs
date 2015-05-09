using System;
using System.Collections.Generic;
using System.Linq;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class TransactionManager
    {
        private readonly ITransactionStore transactionStore;

        public TransactionManager(ITransactionStore transactionStore)
        {
            if (transactionStore == null)
                throw new ArgumentNullException("transactionStore");

            this.transactionStore = transactionStore;
        }

        public BuyTransaction Buy(User user, Product product)
        {
            if (user == null) 
                throw new ArgumentNullException("user");
            if (product == null) 
                throw new ArgumentNullException("product");

            int id = GenerateNextTransactionId();
            return new BuyTransaction(id, user, DateTime.Now, product);
        }

        public InsertCashTransaction AddCredits(User user, int amount)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (amount < 0)
                throw new ArgumentOutOfRangeException("amount", amount, "Amount must be a positive integer.");

            int id = GenerateNextTransactionId();
            return new InsertCashTransaction(id, user, DateTime.Now, amount);
        }

        public void Commit(Transaction transaction)
        {
            if (transaction == null) 
                throw new ArgumentNullException("transaction");

            if (transactionStore.Any(t => t.TransactionID == transaction.TransactionID))
                throw new DuplicateTransactionException(transaction);

            transaction.Execute();
            transactionStore.Save(transaction);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return transactionStore;
        }

        public IEnumerable<Transaction> GetAllForUser(User user)
        {
            if (user == null) 
                throw new ArgumentNullException("user");

            return transactionStore.Where(t => t.User.Equals(user));
        }

        private int GenerateNextTransactionId()
        {
            var last = transactionStore.LastOrDefault();
            if (last != null)
                return last.TransactionID + 1;

            return 1;
        }
    }
}