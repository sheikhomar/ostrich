using System;
using NUnit.Framework;
using ostrich.Core;
using ostrich.Core.Exceptions;

namespace ostrich.Tests
{
    [TestFixture]
    public class InsertCashTransactionTests
    {
        [Test]
        public void Constructor_should_not_allow_zero_amount()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = 2147483647 };

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new InsertCashTransaction(1, user, DateTime.Now, 0));
            Assert.AreEqual("amount", ex.ParamName);
        }

        [Test]
        public void Constructor_should_not_allow_negative_amount()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") {Balance = 2147483647};

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new InsertCashTransaction(1, user, DateTime.Now, -1));
            Assert.AreEqual("amount", ex.ParamName);
        }

        [Test]
        public void Constructor_should_not_allow_null_user()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new InsertCashTransaction(1, null, DateTime.Now, 1000));
            Assert.AreEqual("user", ex.ParamName);
        }

        [Test]
        public void Execute_should_balance_account()
        {
            var user = new User(1, "Anders", "And", "wacky") { Balance = 1000 };
            var transaction = new InsertCashTransaction(1, user, DateTime.Now, 1000);
            transaction.Execute();

            Assert.AreEqual(2000, user.Balance);
        }

        [Test]
        public void Execute_should_check_for_overflow_in_balance()
        {
            var user = new User(1, "Mr.", "Burns", "evilcorp111one") { Balance = int.MaxValue };
            var transaction = new InsertCashTransaction(1, user, DateTime.Now, 1);

            Assert.Throws<BalanceOverflowException>(() => transaction.Execute());
            Assert.AreEqual(int.MaxValue, user.Balance);
        }

        [Test]
        public void Execute_should_not_overflow_when_balance_is_negative()
        {
            var user = new User(1, "Anders", "And", "wacky") { Balance = -1000 };
            var transaction = new InsertCashTransaction(1, user, DateTime.Now, 999);

            transaction.Execute();

            Assert.AreEqual(-1, user.Balance);
        }

        [Test]
        public void ToString_should_return_correct_string()
        {
            var user = new User(1, "Anders", "And", "wacky") { Balance = 1000 };
            var transaction = new InsertCashTransaction(1, user, DateTime.Now, 1000);
            var str = transaction.ToString();

            Assert.IsTrue(str.Contains("Type=InsertCash"));
            Assert.IsTrue(str.Contains("Amount=1000"));
        }
    }
}