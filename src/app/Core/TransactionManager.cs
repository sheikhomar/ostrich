using System;
using System.Collections.Generic;
using System.Linq;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class TransactionManager
    {
        private readonly IList<Transaction> transactions;
        private readonly ITransactionStore transactionStore;

        public TransactionManager(ITransactionStore transactionStore)
        {
            if (transactionStore == null)
                throw new ArgumentNullException("transactionStore");

            this.transactionStore = transactionStore;
            this.transactions = transactionStore.Load();
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

        public InsertCashTransaction AddCredits(User user, int creditAmount)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            if (creditAmount < 0)
                throw new ArgumentOutOfRangeException("creditAmount", creditAmount, "Amount must be a positive integer.");

            int id = GenerateNextTransactionId();
            return new InsertCashTransaction(id, user, DateTime.Now, creditAmount);
        }

        public void Commit(Transaction transaction)
        {
            if (transaction == null) 
                throw new ArgumentNullException("transaction");

            if (transactions.Any(t => t.TransactionID == transaction.TransactionID))
                throw new DuplicateTransactionException(transaction);

            transaction.Execute();
            transactionStore.Save(transaction);
            transactions.Add(transaction);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return transactions;
        }

        public IEnumerable<Transaction> GetAllForUser(User user)
        {
            if (user == null) 
                throw new ArgumentNullException("user");

            return transactions.Where(t => t.User.Equals(user));
        }

        private int GenerateNextTransactionId()
        {
            var last = transactions.LastOrDefault();
            if (last != null)
                return last.TransactionID + 1;

            return 1;
        }
    }
}