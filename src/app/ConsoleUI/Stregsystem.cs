﻿using System;
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
        private readonly TransactionManager transactions;

        public Stregsystem()
        {
            // TODO: assume that file encoding is Encoding.Default
            var reader = new StreamReader("products.csv", Encoding.Default);
            ProductCatalogImporter importer = new ProductCatalogImporter(reader);

            productCatalog = importer.Import();

            transactions = new TransactionManager();

            userRepository = new UserRepository();
            userRepository.Add(new User(1, "Joakim", "Von And") { Balance = int.MaxValue, UserName = "b"});
            userRepository.Add(new User(2, "Anders", "And") { Balance = 6000, UserName = "a" });
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
            if (productId < 1)
                throw new ArgumentOutOfRangeException("productId", productId, "Product id cannot be below 1.");

            Product product = productCatalog.TryFindById(productId);

            if (product == null)
                throw new ProductNotFoundException(productId);

            return product;
        }

        public User GetUser(string userName)
        {
            var user = userRepository.GetUsers().FirstOrDefault(u => u.UserName == userName);

            if (user == null)
                throw new UserNotFoundException(userName);

            return user;
        }

        public IEnumerable<Transaction> GetTransactionList(User user)
        {
            if (user == null) 
                throw new ArgumentNullException("user");

            return transactions.GetAll().Where(t => t.User.Equals(user));
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