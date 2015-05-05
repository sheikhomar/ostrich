using System;
using System.Collections.Generic;

namespace ConsoleUI
{
    public class Stregsystem : IStregsystem
    {
        public void BuyProduct(User user, Product product)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (product == null) throw new ArgumentNullException("product");
            throw new System.NotImplementedException();
        }

        public void AddCreditsToAccount(User user, int amount)
        {
            if (user == null) throw new ArgumentNullException("user");
            throw new System.NotImplementedException();
        }

        public void ExecuteTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            throw new System.NotImplementedException();
        }

        public Product GetProduct(int productId)
        {
            if (false)
                throw new RecordNotFoundException("Product", productId);
            
            throw new System.NotImplementedException();
        }

        public User GetUser(int userId)
        {
            if (false)
                throw new RecordNotFoundException("User", userId);

            throw new System.NotImplementedException();
        }

        public IList<Transaction> GetTransactionList(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            throw new System.NotImplementedException();
        }

        public IList<Product> GetActiveProducts()
        {
            throw new System.NotImplementedException();
        }
    }
}