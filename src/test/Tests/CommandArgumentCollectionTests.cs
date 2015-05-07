using System;
using System.Runtime.Remoting;
using NUnit.Framework;
using ostrich.Core;

namespace ostrich.Tests
{
    [TestFixture]
    public class CommandArgumentCollectionTests
    {
        [Test]
        public void Constructor_should_parse_empty_string()
        {
            var args = new CommandArgumentCollection("");
            Assert.AreEqual(0, args.Count);
        }

        [Test]
        public void Constructor_should_parse_null_string()
        {
            var args = new CommandArgumentCollection((string) null);
            Assert.AreEqual(0, args.Count);
        }

        [Test]
        public void Constructor_should_parse_multiple_arguments()
        {
            var args = new CommandArgumentCollection("arg1 arg2");
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual("arg1", args[0]);
            Assert.AreEqual("arg2", args[1]);
        }

        [Test]
        public void Constructor_should_trim_arguments()
        {
            var args = new CommandArgumentCollection("  arg1   arg2   ");
            Assert.AreEqual(2, args.Count);
            Assert.AreEqual("arg1", args[0]);
            Assert.AreEqual("arg2", args[1]);
        }

        [Test]
        public void Indexer_should_throw_exception_if_index_is_out_of_bounds()
        {
            var args = new CommandArgumentCollection("arg1 arg2");
            Assert.Throws<ArgumentOutOfRangeException>(() => { string s = args[-1]; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { string s = args[2]; });
        }

        [Test]
        public void GetInt_should_convert_string_argument_to_int()
        {
            var args = new CommandArgumentCollection(" asd   1232");
            int? result = args.GetAsInt(1);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(1232, result.Value);
        }

        [Test]
        public void GetInt_should_return_null_if_argument_cannot_be_converted()
        {
            var args = new CommandArgumentCollection("NaN");
            int? result = args.GetAsInt(0);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void GetInt_should_throw_exception_if_argument_does_not_exist()
        {
            var args = new CommandArgumentCollection("cmd");
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => args.GetAsInt(1));
        }
    }
}