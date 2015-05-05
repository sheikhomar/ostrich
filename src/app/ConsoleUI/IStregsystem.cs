using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleUI
{
    public interface IStregsystem
    {
        void BuyProduct(User user, Product product);
        void AddCreditsToAccount(User user, int amount);
        void ExecuteTransaction(Transaction transaction);
        Product GetProduct(int productId);
        User GetUser(int userId);
        IList<Transaction> GetTransactionList(User user);
        IEnumerable<Product> GetActiveProducts();
    }
}