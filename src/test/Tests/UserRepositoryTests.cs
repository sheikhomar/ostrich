using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ostrich.Core;
using ostrich.Core.Exceptions;

namespace ostrich.Tests
{
    [TestFixture]
    public class UserRepositoryTests
    {

        [Test]
        public void Add_should_not_allow_null_user()
        {
            var repository = new UserRepository();

            var ex = Assert.Throws<ArgumentNullException>(() => repository.Add(null));
            Assert.AreEqual("user", ex.ParamName);
        }

        [Test]
        public void Add_should_not_allow_duplicate_user_id()
        {
            var cartman = new User(1, "Eric", "Cartman", "cartman");
            var kenny = new User(1, "Kenny", "McCormick", "kenny");
            var repository = new UserRepository();
            repository.Add(cartman);

            var ex = Assert.Throws<DuplicateUserException>(() => repository.Add(kenny));
            Assert.AreEqual(kenny, ex.User);
        }

        [Test]
        public void Add_should_not_allow_duplicate_user_names()
        {
            var cartman = new User(1, "Eric", "Cartman", "cartman");
            var kenny = new User(2, "Kenny", "McCormick", "cartman");
            var repository = new UserRepository();
            repository.Add(cartman);

            var ex = Assert.Throws<DuplicateUserException>(() => repository.Add(kenny));
            Assert.AreEqual(kenny, ex.User);
            Assert.IsTrue(ex.Message.Contains("username"));
        }

        [Test]
        public void GetUsers_should_return_existing_users()
        {
            var cartman = new User(1, "Eric", "Cartman", "fatkid");
            var kenny = new User(2, "Kenny", "McCormick", "kenny");
            var repository = new UserRepository();
            repository.Add(cartman);
            repository.Add(kenny);

            var expected = new List<User> { cartman, kenny };
            var actual = repository.GetUsers();

            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [Test]
        public void Indexer_should_throw_exception_if_username_is_not_found()
        {
            var repository = new UserRepository();

            var ex = Assert.Throws<UserNotFoundException>(() => repository["kenny"].ToString());
            Assert.AreEqual("kenny", ex.UserName);
        }

        [Test]
        public void Indexer_should_return_existing_users()
        {
            var cartman = new User(1, "Eric", "Cartman", "fatkid");
            var kenny = new User(2, "Kenny", "McCormick", "kenny");
            var repository = new UserRepository();
            repository.Add(cartman);
            repository.Add(kenny);

            Assert.AreSame(cartman, repository["fatkid"]);
            Assert.AreSame(kenny, repository["kenny"]);
        }

    }
}