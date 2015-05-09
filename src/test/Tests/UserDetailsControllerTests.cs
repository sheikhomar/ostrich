using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using ostrich.Core;
using ostrich.Core.Exceptions;

namespace ostrich.Tests
{
    [TestFixture]
    public class UserDetailsControllerTests
    {
        private IUserInterface ui;
        private IBackendSystem system;
        private UserDetailsController controller;

        [SetUp]
        public void Setup()
        {
            ui = Substitute.For<IUserInterface>();
            system = Substitute.For<IBackendSystem>();
            controller = new UserDetailsController(ui, system);
        }
        
        [Test]
        public void Process_should_check_whether_user_exists()
        {
            system.GetUser("ahmed-dead-terrorist").Returns(
                _ => { throw new UserNotFoundException("ahmed-dead-terrorist"); });

            var args = new CommandArgumentCollection("ahmed-dead-terrorist");
            controller.Process(args);

            ui.Received().DisplayUserNotFound("ahmed-dead-terrorist");
        }

        [Test]
        public void Process_should_call_correct_ui_methods()
        {
            User user = new User(1, "Johnny", "Bravo", "wobra");
            Product product = new Product(12, "Shawarma");
            InsertCashTransaction t1 = new InsertCashTransaction(1, user, DateTime.Now, 100);
            BuyTransaction t2 = new BuyTransaction(2, user, DateTime.Now, product);
            BuyTransaction t3 = new BuyTransaction(3, user, DateTime.Now, product);
            Transaction[] transactions = { t1, t2, t3 };

            system.GetUser("wobra").Returns(user);
            system.GetTransactionList(user).Returns(transactions);

            var args = new CommandArgumentCollection("wobra");
            controller.Process(args);

            BuyTransaction[] expectedTransactions = { t2, t3 };
            var argChecker = Arg.Is<IEnumerable<BuyTransaction>>(l => l.SequenceEqual(expectedTransactions));
            ui.Received().DisplayUserInfo(user, argChecker);
        }
    }
}