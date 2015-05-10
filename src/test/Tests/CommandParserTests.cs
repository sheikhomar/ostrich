using System;
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

        [TestCase("")]
        [TestCase("Now the world has gone to bed")]
        public void Parse_should_return_correct_processor_if_command_is_invalid(string command)
        {
            ParsingResult result = parser.Parse(command);

            Assert.IsTrue(result.Processor is InvalidCommandProcessor);
        }

        [TestCase("marvin")]
        [TestCase("ouf1111_000")]
        public void Parse_should_return_correct_processor_if_user_details_command(string command)
        {
            ParsingResult result = parser.Parse(command);

            Assert.IsTrue(result.Processor is UserDetailsCommandProcessor);
        }

        [TestCase("marvin 7738")]
        [TestCase("marvin 2 7738")]
        public void Parse_should_quick_buy_command(string command)
        {
            ParsingResult result = parser.Parse(command);

            Assert.IsTrue(result.Processor is QuickBuyCommandProcessor);
        }

        [TestCase(":q")]
        [TestCase(":quit")]
        [TestCase(":activate 123")]
        [TestCase(":addcredits marvin 100000")]
        [TestCase(":makecoffe --username marvin --quantity 100000 --timeframe=1min")]
        public void Parse_should_recognize_administration_commands(string command)
        {
            ParsingResult result = parser.Parse(command);

            Assert.IsTrue(result.Processor is AdministrationCommandProcessor);
        }
    }
}