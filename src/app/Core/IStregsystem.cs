using System.Collections.Generic;

namespace ostrich.Core
{
    public interface IStregsystem
    {
        BuyTransaction BuyProduct(User user, Product product);
        InsertCashTransaction AddCreditsToAccount(User user, int amount);
        void ExecuteTransaction(Transaction transaction);
        Product GetProduct(int productId);
        User GetUser(string userName);
        IEnumerable<Transaction> GetTransactionList(User user);
        IEnumerable<Product> GetActiveProducts();
        IEnumerable<User> GetUsers();
    }
}