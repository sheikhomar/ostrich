using System;
using NUnit.Framework;
using ostrich.Core;
using ostrich.Core.Exceptions;

namespace ostrich.Tests
{
    [TestFixture]
    public class BuyTransactionTests
    {

        [Test]
        public void Constructor_should_not_allow_null_product()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = 2147483647 };

            var ex = Assert.Throws<ArgumentNullException>(() => new BuyTransaction(1, user, DateTime.Now, null));
            Assert.AreEqual("product", ex.ParamName);
        }

        [Test]
        public void Constructor_should_not_allow_null_user()
        {
            var product = new Product(1, "Milk", 1000) { Active = true };

            var ex = Assert.Throws<ArgumentNullException>(() => new BuyTransaction(1, null, DateTime.Now, product));
            Assert.AreEqual("user", ex.ParamName);
        }

        [Test]
        public void Execute_should_balance_account()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = 2147483647 };
            var product = new Product(1, "Milk", 1000) { Active = true };
            var transaction = new BuyTransaction(1, user, DateTime.Now, product);
            transaction.Execute();

            Assert.AreEqual(2147482647, user.Balance);
        }

        [Test]
        public void Amount_should_be_negative_integer()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = 2147483647 };
            var product = new Product(1, "Milk", 1000);
            var transaction = new BuyTransaction(1, user, DateTime.Now, product);

            Assert.AreEqual(-1000, transaction.Amount);
        }

        [Test]
        public void Date_should_be_set()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = 2147483647 };
            var product = new Product(1, "Milk", 1000);
            Transaction transaction = new BuyTransaction(1, user, DateTime.Today, product);

            Assert.AreEqual(DateTime.Today, transaction.Date);
        }

        [Test]
        public void User_should_be_set()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = 2147483647 };
            var product = new Product(1, "Milk", 1000);
            Transaction transaction = new BuyTransaction(1, user, DateTime.Today, product);

            Assert.AreSame(user, transaction.User);
        }

        [Test]
        public void Execute_should_throw_exception_if_balance_is_too_low()
        {
            var user = new User(1, "Ol'", "Gil", "oldgil") { Balance = 900 };
            var product = new Product(1, "Milk", 1000);
            var transaction = new BuyTransaction(1, user, DateTime.Now, product);

            var ex = Assert.Throws<InsufficientCreditsException>(() => transaction.Execute());
            Assert.AreSame(user, ex.User);
            Assert.AreSame(product, ex.Product);
            Assert.AreEqual(900, user.Balance);
        }

        [Test]
        public void Execute_should_throw_exception_if_product_is_not_saleable()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = 2147483647 };
            var product = new Product(2, "Slow-Fission Reactor", 10000000) { Active = false };
            var transaction = new BuyTransaction(1, user, DateTime.Now, product);

            var ex = Assert.Throws<ProductNotSaleableException>(() => transaction.Execute());
            Assert.AreSame(product, ex.Product);
            Assert.AreEqual(2147483647, user.Balance);
        }

        [Test]
        public void ToString_should_return_correct_STRING()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = 2147483647 };
            var product = new Product(2, "Slow-Fission Reactor", 10000000) { Active = false };
            var transaction = new BuyTransaction(1, user, DateTime.Now, product);
            var str = transaction.ToString();

            Assert.IsTrue(str.Contains("ProductID=2"));
            Assert.IsTrue(str.Contains("Type=Buy"));
        }
    }
}