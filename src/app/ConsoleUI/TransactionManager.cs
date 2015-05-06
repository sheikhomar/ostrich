using System;
using System.Collections.Generic;
using ostrich.ConsoleUI;

namespace ConsoleUI
{
    public class TransactionManager
    {
        private readonly IList<Transaction> transactions;

        public TransactionManager()
        {
            transactions = new List<Transaction>();
        }

        public BuyTransaction Buy(User user, Product product)
        {
            if (user == null) 
                throw new ArgumentNullException("user");
            if (product == null) 
                throw new ArgumentNullException("product");

            int id = GenerateNextTransactionId();
            return new BuyTransaction(id, user, DateTime.Now, product, product.Price * -1);
        }

        public void Commit(Transaction transaction)
        {
            if (transaction == null) 
                throw new ArgumentNullException("transaction");

            transaction.Execute();
            transactions.Add(transaction);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return transactions;
        }

        private int GenerateNextTransactionId()
        {
            // TODO: Fix this naive ID generator if there is time.
            return transactions.Count + 1;
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
    }
}