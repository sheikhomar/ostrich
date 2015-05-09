using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ostrich.Core
{
    public class DefaultBackendSystem : IBackendSystem
    {
        private readonly ProductCatalog productCatalog;
        private readonly UserRepository userRepository;
        private readonly TransactionManager transactions;

        public DefaultBackendSystem(ProductCatalog productCatalog, UserRepository userRepository, TransactionManager transactions)
        {
            if (productCatalog == null) 
                throw new ArgumentNullException("productCatalog");
            if (userRepository == null) 
                throw new ArgumentNullException("userRepository");
            if (transactions == null) 
                throw new ArgumentNullException("transactions");

            this.productCatalog = productCatalog;
            this.userRepository = userRepository;
            this.transactions = transactions;
        }

        public BuyTransaction BuyProduct(User user, Product product)
        {
            return transactions.Buy(user, product);
        }

        public InsertCashTransaction AddCreditsToAccount(User user, int amount)
        {
            return transactions.AddCredits(user, amount);
        }

        public void ExecuteTransaction(Transaction transaction)
        {
            transactions.Commit(transaction);
        }

        public Product GetProduct(int productId)
        {
            return productCatalog.GetProduct(productId);
        }

        public User GetUser(string userName)
        {
            return userRepository[userName];
        }

        public IEnumerable<Transaction> GetTransactionList(User user)
        {
            return transactions.GetAllForUser(user);
        }

        public IEnumerable<Product> GetActiveProducts()
        {
            return productCatalog.GetActiveProducts();
        }

        public IEnumerable<User> GetUsers()
        {
            return userRepository.GetUsers();
        }
    }
}