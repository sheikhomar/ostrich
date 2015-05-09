using System;
using System.Collections.Generic;
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
        private User user;
        private Transaction transaction1;

        [SetUp]
        public void Setup()
        {
            store = Substitute.For<ITransactionStore>();
            manager = new TransactionManager(store);
            user = new User(1, "Johnny", "Bravo", "wobra") { Balance = 10000 };
            transaction1 = Substitute.For<Transaction>(1, user, DateTime.Now);
        }

        [Test]
        public void Constructor_should_not_allow_null_store()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new TransactionManager(null));
            Assert.AreEqual("transactionStore", ex.ParamName);
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
            var t2 = Substitute.For<Transaction>(1, user, DateTime.Now);

            IList<Transaction> transactions = new List<Transaction> { transaction1 };
            store.GetEnumerator().Returns(transactions.GetEnumerator());

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
    }
}