using System;
using NSubstitute;
using NUnit.Framework;
using ostrich.Core;
using ostrich.Core.Exceptions;

namespace ostrich.Tests
{
    [TestFixture]
    public class QuickBuyControllerTests
    {
        private IUserInterface ui;
        private IBackendSystem system;
        private QuickBuyController controller;

        [SetUp]
        public void Setup()
        {
            ui = Substitute.For<IUserInterface>();
            system = Substitute.For<IBackendSystem>();
            controller = new QuickBuyController(ui, system);
        }


        [Test]
        public void Process_should_throw_exception_if_too_few_arguments()
        {
            var args = new CommandArgumentCollection("yogibear");
            Assert.Throws<InvalidOperationException>(() => controller.Process(args));
        }

        [Test]
        public void Process_should_throw_exception_if_too_many_arguments()
        {
            var args = new CommandArgumentCollection("yogibear 5 35124 12323");
            Assert.Throws<InvalidOperationException>(() => controller.Process(args));
        }
        
        [TestCase("yogibear -1", QuickBuyController.InvalidProductIdMessage)]
        [TestCase("yogibear 0", QuickBuyController.InvalidProductIdMessage)]
        [TestCase("yogibear 1.0", QuickBuyController.InvalidProductIdMessage)]
        [TestCase("yogibear picnic", QuickBuyController.InvalidProductIdMessage)]
        [TestCase("yogibear four 12", QuickBuyController.InvalidQuantityMessage)]
        [TestCase("yogibear 0 12", QuickBuyController.InvalidQuantityMessage)]
        [TestCase("yogibear -1 12", QuickBuyController.InvalidQuantityMessage)]
        [TestCase("yogibear 1.0 12", QuickBuyController.InvalidQuantityMessage)]
        public void Process_should_invalid_args(string command, string errorMessage)
        {
            var args = new CommandArgumentCollection(command);
            controller.Process(args);

            ui.Received().DisplayGeneralError(Arg.Is<string>(str => str.Contains(errorMessage)));
        }

        [Test]
        public void Process_should_allow_buy()
        {
            User user = new User(1, "Yogi", "Bear", "yogibear");
            Product product = new Product(2340, "Picnic Food");
            BuyTransaction transaction = new BuyTransaction(1, user, DateTime.Now, product);
            system.GetProduct(2340).Returns(product);
            system.GetUser("yogibear").Returns(user);
            system.BuyProduct(user, product).Returns(transaction);

            var args = new CommandArgumentCollection("yogibear 2340");
            controller.Process(args);

            system.Received().ExecuteTransaction(transaction);
            ui.Received().DisplayUserBuysProduct(transaction);
            ui.DidNotReceive().DisplayUserBuysProduct(Arg.Any<Product>(),Arg.Any<User>(),Arg.Any<int>());
        }

        [Test]
        public void Process_should_allow_multiple_buy()
        {
            User user = new User(1, "Yogi", "Bear", "yogibear");
            Product product = new Product(12340, "Picnic Food");
            BuyTransaction transaction = new BuyTransaction(1, user, DateTime.Now, product);
            system.GetProduct(12340).Returns(product);
            system.GetUser("yogibear").Returns(user);
            system.BuyProduct(user, product).Returns(transaction);

            var args = new CommandArgumentCollection("yogibear 5 12340");
            controller.Process(args);

            system.Received(5).ExecuteTransaction(transaction);
            ui.Received().DisplayUserBuysProduct(product, user, 5);
            ui.DidNotReceive().DisplayUserBuysProduct(Arg.Any<BuyTransaction>());
        }

        [Test]
        public void Process_should_check_whether_user_exists()
        {
            system.GetUser("darwin").Returns(
                _ => { throw new UserNotFoundException("darwin"); });
            
            var args = new CommandArgumentCollection("darwin 2340");
            controller.Process(args);

            ui.Received().DisplayUserNotFound("darwin");
        }

        [Test]
        public void Process_should_check_whether_product_exists()
        {
            User user = new User(1, "Yogi", "Bear", "yogibear");
            system.GetUser("yogibear").Returns(user);
            system.GetProduct(42).Returns(_ => { throw new ProductNotFoundException(42); });

            var args = new CommandArgumentCollection("yogibear 42");
            controller.Process(args);

            ui.Received().DisplayProductNotFound(42);
        }

        [Test]
        public void Process_should_handle_InsufficientCreditsException()
        {
            User user = new User(1, "Yogi", "Bear", "yogibear111one");
            Product product = new Product(123, "Picnic Food");
            BuyTransaction transaction = new BuyTransaction(1, user, DateTime.Now, product);
            system.GetProduct(123).Returns(product);
            system.GetUser("yogibear111one").Returns(user);
            system.BuyProduct(user, product).Returns(transaction);
            system
                .When(sys => sys.ExecuteTransaction(transaction))
                .Do(_ => { throw new InsufficientCreditsException(user, product); });

            var args = new CommandArgumentCollection("yogibear111one 123");
            controller.Process(args);

            ui.Received().DisplayInsufficientCash(user, product);
        }

        [Test]
        public void Process_should_handle_ProductNotSaleableException()
        {
            User user = new User(1, "Yogi", "Bear", "yogibear111one");
            Product product = new Product(123, "Picnic Food");
            BuyTransaction transaction = new BuyTransaction(1, user, DateTime.Now, product);
            system.GetProduct(123).Returns(product);
            system.GetUser("yogibear111one").Returns(user);
            system.BuyProduct(user, product).Returns(transaction);
            system
                .When(sys => sys.ExecuteTransaction(transaction))
                .Do(_ => { throw new ProductNotSaleableException(product); });

            var args = new CommandArgumentCollection("yogibear111one 123");
            controller.Process(args);

            ui.Received().DisplayProductNotSaleable(product);
        }

        [Test]
        public void Process_should_handle_BalanceUnderflowException()
        {
            User user = new User(1, "Yogi", "Bear", "yogibear111one");
            Product product = new Product(123, "Picnic Food");
            BuyTransaction transaction = new BuyTransaction(1, user, DateTime.Now, product);
            system.GetProduct(123).Returns(product);
            system.GetUser("yogibear111one").Returns(user);
            system.BuyProduct(user, product).Returns(transaction);

            Exception exception = new BalanceUnderflowException(user, 1000);
            system.When(sys => sys.ExecuteTransaction(transaction)).Do(_ => { throw exception; });

            var args = new CommandArgumentCollection("yogibear111one 123");
            controller.Process(args);

            ui.Received().DisplayGeneralError(exception.Message);
        }
    }
}