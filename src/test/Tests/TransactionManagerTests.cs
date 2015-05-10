using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using ostrich.Core;
using ostrich.Core.Exceptions;

namespace ostrich.Tests
{
    [TestFixture]
    public class TransactionManagerTests
    {
        private TransactionManager manager;
        private ITransactionStore store;
        private User johnnyBravo;
        private Transaction transaction1;
        private List<Transaction> transactionsInTheStore;

        [SetUp]
        public void Setup()
        {
            store = Substitute.For<ITransactionStore>();
            transactionsInTheStore = new List<Transaction>();
            store.Load().Returns(transactionsInTheStore);
            manager = new TransactionManager(store);
            johnnyBravo = new User(1, "Johnny", "Bravo", "wobra") { Balance = 10000 };
            transaction1 = Substitute.For<Transaction>(100, johnnyBravo, DateTime.Now);
        }

        [Test]
        public void Constructor_should_not_allow_null_store()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new TransactionManager(null));
            Assert.AreEqual("transactionStore", ex.ParamName);
        }

        [Test]
        public void Constructor_should_load_transactions_from_store()
        {
            var store2 = Substitute.For<ITransactionStore>();
            var manager2 = new TransactionManager(store2);

            store2.Received(1).Load();
        }

        [Test]
        public void Commit_should_check_argument()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => manager.Commit(null));
            Assert.AreEqual("transaction", ex.ParamName);
        }

        [Test]
        public void Commit_should_check_for_duplicate_transaction_ids()
        {
            transactionsInTheStore.Add(transaction1);
            var t2 = Substitute.For<Transaction>(transaction1.TransactionID, johnnyBravo, DateTime.Now);

            var ex = Assert.Throws<DuplicateTransactionException>(() => manager.Commit(t2));
            Assert.AreEqual(t2, ex.Transaction);
        }

        [Test]
        public void Commit_should_write_transaction_to_the_store()
        {
            manager.Commit(transaction1);

            store.Received().Save(transaction1);
        }

        [Test]
        public void Commit_should_execute_transactions()
        {
            manager.Commit(transaction1);

            transaction1.Received().Execute();
        }

        [Test]
        public void GetAll_should_return_all_transactions()
        {
            var t2 = Substitute.For<Transaction>(1201, johnnyBravo, DateTime.Now);
            var t3 = Substitute.For<Transaction>(1202, johnnyBravo, DateTime.Now);
            transactionsInTheStore.Add(transaction1);
            transactionsInTheStore.Add(t2);
            manager.Commit(t3);

            var expected = new List<Transaction> { transaction1, t2, t3 };
            var actual = manager.GetAll();

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public void GetAllForUser_should_accept_null_user()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => manager.GetAllForUser(null));
            Assert.AreEqual("user", ex.ParamName);
        }

        [Test]
        public void GetAllForUser_should_return_transactions_that_belongs_to_user()
        {
            var shredder = new User(5317, "Oroku", "Saki", "shredder");
            var t1 = Substitute.For<Transaction>(1201, johnnyBravo, DateTime.Now);
            var t2 = Substitute.For<Transaction>(1202, shredder, DateTime.Now);
            var t3 = Substitute.For<Transaction>(1203, shredder, DateTime.Now);
            transactionsInTheStore.Add(t1);
            transactionsInTheStore.Add(t2);
            transactionsInTheStore.Add(t3);
            transactionsInTheStore.Add(transaction1);

            var expected = new List<Transaction> { t2, t3 };
            var actual = manager.GetAllForUser(shredder);

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public void Buy_should_not_allow_null_user()
        {
            var product = new Product(12, "Sushi");

            var ex = Assert.Throws<ArgumentNullException>(() => manager.Buy(null, product));

            Assert.AreEqual("user", ex.ParamName);
        }

        [Test]
        public void Buy_should_not_allow_null_product()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => manager.Buy(johnnyBravo, null));

            Assert.AreEqual("product", ex.ParamName);
        }

        [Test]
        public void Buy_should_instantiate_valid_buy_transaction()
        {
            var t1 = Substitute.For<Transaction>(1201, johnnyBravo, DateTime.Now);
            transactionsInTheStore.Add(t1);

            var shredder = new User(5317, "Oroku", "Saki", "shredder");
            var sushi = new Product(12, "Sushi") { Price = 7499900 };

            var newTransaction = manager.Buy(shredder, sushi);

            Assert.AreEqual(1202, newTransaction.TransactionID);
            Assert.AreEqual(shredder, newTransaction.User);
            Assert.AreEqual(sushi, newTransaction.Product);
            Assert.AreEqual(-7499900, newTransaction.Amount);
        }

        [Test]
        public void AddCredits_should_not_allow_negative_credit()
        {
            var shredder = new User(5317, "Oroku", "Saki", "shredder");

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => manager.AddCredits(shredder, -1));

            Assert.AreEqual("creditAmount", ex.ParamName);
            Assert.AreEqual(-1, ex.ActualValue);
        }

        [Test]
        public void AddCredits_should_not_allow_null_user()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => manager.AddCredits(null, 1234));

            Assert.AreEqual("user", ex.ParamName);
        }

        [Test]
        public void AddCredits_should_instantiate_valid_transaction()
        {
            var shredder = new User(5317, "Oroku", "Saki", "shredder");
            var newTransaction = manager.AddCredits(shredder, 100000000);

            Assert.AreEqual(1, newTransaction.TransactionID);
            Assert.AreEqual(shredder, newTransaction.User);
            Assert.AreEqual(100000000, newTransaction.Amount);
        }
    }
}