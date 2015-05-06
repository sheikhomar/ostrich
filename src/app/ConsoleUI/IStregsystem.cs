using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using ostrich.ConsoleUI;

namespace ConsoleUI
{
    public interface IStregsystem
    {
        BuyTransaction BuyProduct(User user, Product product);
        void AddCreditsToAccount(User user, int amount);
        void ExecuteTransaction(Transaction transaction);
        Product GetProduct(int productId);
        User GetUser(string userName);
        IEnumerable<Transaction> GetTransactionList(User user);
        IEnumerable<Product> GetActiveProducts();
        IEnumerable<User> GetUsers();
    }
}