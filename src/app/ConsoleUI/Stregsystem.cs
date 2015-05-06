using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ostrich.ConsoleUI;

namespace ConsoleUI
{
    public class Stregsystem : IStregsystem
    {
        private readonly ProductCatalog productCatalog;
        private readonly UserRepository userRepository;

        public Stregsystem()
        {
            // TODO: assume that file encoding is Encoding.Default
            var reader = new StreamReader("products.csv", Encoding.Default);
            ProductCatalogImporter importer = new ProductCatalogImporter(reader);

            productCatalog = importer.Import();

            userRepository = new UserRepository();
            userRepository.Add(new User(1, "Joakim", "Von And") { Balance = int.MaxValue, UserName = "b"});
            userRepository.Add(new User(2, "Anders", "And") { Balance = 100, UserName = "a" });
        }

        public BuyTransaction BuyProduct(User user, Product product)
        {
            if (user == null) throw new ArgumentNullException("user");
            if (product == null) throw new ArgumentNullException("product");
            
            return new BuyTransaction(1, user, DateTime.Now, product, product.Price * -1);
        }

        public void AddCreditsToAccount(User user, int amount)
        {
            if (user == null) throw new ArgumentNullException("user");
            throw new System.NotImplementedException();
        }

        public void ExecuteTransaction(Transaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");

            transaction.Execute();
        }

        public Product GetProduct(int productId)
        {
            if (productId < 1)
                throw new ArgumentOutOfRangeException("productId", productId, "Product id cannot be below 1.");

            Product product = productCatalog.TryFindById(productId);

            if (product == null)
                throw new RecordNotFoundException("Product", productId);

            return product;
        }

        public User GetUser(string userName)
        {
            var user = userRepository.GetUsers().FirstOrDefault(u => u.UserName == userName);

            if (user == null)
                throw new UserNotFoundException(userName);

            return user;
        }

        public IList<Transaction> GetTransactionList(User user)
        {
            if (user == null) throw new ArgumentNullException("user");

            throw new System.NotImplementedException();
        }

        public IEnumerable<Product> GetActiveProducts()
        {
            return productCatalog.GetActiveProducts();
        }
    }
}