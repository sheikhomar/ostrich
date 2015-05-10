using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using ostrich.Core;
using ostrich.Core.Processors;

namespace ostrich.Tests
{
    [TestFixture]
    public class CommandParserTests
    {
        private IUserInterface ui;
        private IBackendSystem system;
        private CommandParser parser;

        [SetUp]
        public void Setup()
        {
            ui = Substitute.For<IUserInterface>();
            system = Substitute.For<IBackendSystem>();
            parser = new CommandParser(ui, system);
        }

        [Test]
        public void Constructor_should_not_allow_null_ui()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CommandParser(null, system));
            Assert.AreEqual("ui", ex.ParamName);
        }

        [Test]
        public void Constructor_should_not_allow_null_system()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CommandParser(ui, null));
            Assert.AreEqual("system", ex.ParamName);
        }


        [Test]
        public void Parse_should_respond_empty_command_with_error()
        {
            parser.Parse("");

            ui.Received(1).DisplayGeneralError(InvalidCommandProcessor.EmptyCommandErrorMessage);
        }

        [Test]
        public void Parse_should_respond_to_too_many_arguments()
        {
            parser.Parse("Now the world has gone to bed");

            ui.Received(1).DisplayTooManyArgumentsError();
        }

        [Test]
        public void Parse_should_process_user_details_command()
        {
            parser.Parse("marvin");

            ui.Received(1).DisplayUserInfo(Arg.Any<User>(), Arg.Any<IEnumerable<BuyTransaction>>());
        }

        [Test]
        public void Parse_should_quick_buy_command()
        {
            parser.Parse("marvin 7738");

            ui.Received(1).DisplayUserBuysProduct(Arg.Any<BuyTransaction>());
        }

        [Test]
        public void Parse_should_understand_q_command()
        {
            parser.Parse(":q");

            ui.Received().Close();
        }

        [Test]
        public void Parse_should_understand_quit_command()
        {
            parser.Parse(":quit");

            ui.Received().Close();
        }

        [Test]
        public void Parse_should_understand_activate_command()
        {
            Product product = new Product(123, "Pizza") { Active = false };
            system.GetProduct(123).Returns(product);

            parser.Parse(":activate 123");

            Assert.IsTrue(product.Active);
        }

        [Test]
        public void Parse_should_understand_deactivate_command()
        {
            Product product = new Product(134, "Burger") { Active = true };
            system.GetProduct(134).Returns(product);

            parser.Parse(":deactivate 134");

            Assert.IsFalse(product.Active);
        }

        [Test]
        public void Parse_should_not_activate_seasonal_product()
        {
            Product product = new SeasonalProduct(1, "UniRun");
            system.GetProduct(1).Returns(product);

            parser.Parse(":activate 1");

            ui.Received().DisplayGeneralError(Arg.Is<string>(arg => arg.Contains("seasonal product")));
        }

        [Test]
        public void Parse_should_not_deactivate_seasonal_product()
        {
            Product product = new SeasonalProduct(1, "UniRun");
            system.GetProduct(1).Returns(product);

            parser.Parse(":deactivate 1");
            
            ui.Received().DisplayGeneralError(Arg.Is<string>(arg => arg.Contains("seasonal product")));
        }

        [Test]
        public void Parse_should_understand_crediton_command()
        {
            Product product = new Product(123, "Pizza") { CanBeBoughtOnCredit = false };
            system.GetProduct(123).Returns(product);

            parser.Parse(":crediton 123");

            Assert.IsTrue(product.CanBeBoughtOnCredit);
        }

        [Test]
        public void Parse_should_understand_creditoff_command()
        {
            Product product = new Product(134, "Burger") { CanBeBoughtOnCredit = true };
            system.GetProduct(134).Returns(product);

            parser.Parse(":creditoff 134");

            Assert.IsFalse(product.CanBeBoughtOnCredit);
        }

        [Test]
        public void Parse_should_understand_addcredits_command()
        {
            User user = new User(1, "Homer", "Simpson", "homer") { Balance = 0 };
            InsertCashTransaction transaction = new InsertCashTransaction(1, user, DateTime.Now, 90);
            system.GetUser("homer").Returns(user);
            system.AddCreditsToAccount(user, 1000).Returns(transaction);

            parser.Parse(":addcredits homer 1000");

            system.Received().ExecuteTransaction(transaction);
            ui.Received().DisplayCashInserted(transaction);
        }

        [TestCase(":p")]
        [TestCase(":list-products")]
        public void Parse_should_understand_list_products_command(string commandName)
        {
            IList<Product> products = new List<Product>();
            system.GetActiveProducts().Returns(products);

            parser.Parse(commandName);

            ui.Received().DisplaceProducts(products);
        }

        [TestCase(":u")]
        [TestCase(":list-users")]
        public void Parse_should_understand_list_users_command(string commandName)
        {
            IList<User> users = new List<User>();
            system.GetUsers().Returns(users);

            parser.Parse(commandName);

            ui.Received().DisplayUsers(users);
        }
    }
}