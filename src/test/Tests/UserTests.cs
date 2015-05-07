using System;
using NUnit.Framework;
using ostrich.Core;

namespace ostrich.Tests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void Constructor_should_initialize_required_properties()
        {
            var user = new User(1457, "Homer", "Simpson", "homes");

            Assert.AreEqual(1457, user.UserID);
            Assert.AreEqual("Homer", user.FirstName);
            Assert.AreEqual("Simpson", user.LastName);
            Assert.AreEqual("homes", user.UserName);
        }

        [Test]
        public void ToString_should_be_formatted_properly()
        {
            var user = new User(1457, "Homer", "Simpson", "homes");
            user.Email = "h.simpson@7g.springfield-power.com";

            Assert.AreEqual("Homer Simpson (h.simpson@7g.springfield-power.com)", user.ToString());
        }

        [TestCase(arguments: new object[] { null, "Simpson", "homes" }, Result = "firstName")]
        [TestCase(arguments: new object[] { "Home", null, "homes" }, Result = "lastName")]
        [TestCase(arguments: new object[] { "Home", "Simpson", null }, Result = "userName")]
        public string Constructor_should_throw_exception(string firstName, string lastName, string userName)
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new User(1, firstName, lastName, userName));
            return ex.ParamName;
        }

        [TestCase("")]
        [TestCase("    ")]
        [TestCase("HomerSimposon")]
        public void UserName_should_be_invalid_when_value_is(string userName)
        {
            var ex = Assert.Throws<ArgumentException>(() => new User(1, "Homer", "Simpson", userName));
            Assert.AreEqual("userName", ex.ParamName);
        }

        [TestCase("h", Result = "h")]
        [TestCase("home_sim", Result = "home_sim")]
        [TestCase("_homersimposon", Result = "_homersimposon")]
        [TestCase("homersimposon_", Result = "homersimposon_")]
        public string UserName_should_be_valid_when_value_is(string userName)
        {
            var user = new User(1, "Homer", "Simpson", userName);
            return user.UserName;
        }

        [Test]
        public void Constructor_should_throw_exception_if_user_name_contains_special_chars()
        {
            string specialChars = "æøåÆØÅ¬!$%^&*()+}{~@?><:.,/';][+*";

            foreach (char specialChar in specialChars)
            {
                var ex = Assert.Throws<ArgumentException>(() => new User(1, "Homer", "Simpson", specialChar.ToString()));
                Assert.AreEqual("userName", ex.ParamName);
            }
        }

        [TestCase("example@domain.com", Result = "example@domain.com")]
        [TestCase("-@domain.com", Result = "-@domain.com")]
        [TestCase("_@domain.com", Result = "_@domain.com")]
        [TestCase(".@domain.com", Result = ".@domain.com")]
        public string Email_should_be_valid_when_value_is(string email)
        {
            var user = new User(1, "Homer", "Simpson", "home_sim");
            user.Email = email;
            return user.Email;
        }

        [TestCase("")]
        [TestCase("   ")]
        [TestCase("example(2)@domain.com")]
        [TestCase("exampledomain.com.")]
        [TestCase("example@domain.com.")]
        [TestCase("example@domain.com-")]
        [TestCase("example@.domain.com")]
        [TestCase("example@-domain.com")]
        public void Email_should_be_invalid_when_value_is(string email)
        {
            var user = new User(1, "Homer", "Simpson", "home_sim");

            var ex = Assert.Throws<ArgumentException>(() => user.Email = email);
            Assert.AreEqual("value", ex.ParamName);
        }

        [Test]
        public void Equals_should_return_true_if_same_object()
        {
            var user = new User(1, "Homer", "Simpson", "home_sim");
            
            Assert.IsTrue(user.Equals(user));
            Assert.IsTrue(user.Equals((object)user));
        }

        [Test]
        public void Equals_should_return_false_if_other_object_is_null()
        {
            var user = new User(1, "Homer", "Simpson", "home_sim");

            Assert.IsFalse(user.Equals((User)null));
            Assert.IsFalse(user.Equals((object)null));
        }

        [Test]
        public void Equals_should_compare_objects_based_on_UserID()
        {
            var homer = new User(1, "Homer", "Simpson", "home_sim");
            var burn = new User(1, "Montgomery", "Burns", "burnitall");

            Assert.IsTrue(homer.Equals(burn));
            Assert.IsTrue(homer.Equals((object)burn));
        }

        [Test]
        public void Equals_should_return_false_if_ids_are_different()
        {
            var homer1 = new User(1, "Homer", "Simpson", "home_sim");
            var homer2 = new User(2, "Homer", "Simpson", "home_sim");

            Assert.IsFalse(homer1.Equals(homer2));
            Assert.IsFalse(homer1.Equals((object)homer2));
        }

        [TestCase(int.MaxValue, Result = false)]
        [TestCase(5000, Result = false)]
        [TestCase(4999, Result = true)]
        [TestCase(0, Result = true)]
        [TestCase(-1, Result = true)]
        [TestCase(int.MinValue, Result = true)]
        public bool HasLowBalance_limit_tests(int newBalance)
        {
            var user = new User(1, "Homer", "Simpson", "home_sim");
            user.Balance = newBalance;
            return user.HasLowBalance;
        }

        [Test]
        public void GetHashCode_should_return_same_value_if_users_are_equal()
        {
            var homer = new User(1, "Homer", "Simpson", "home_sim");
            var burn = new User(1, "Montgomery", "Burns", "burnitall");

            Assert.AreEqual(homer.GetHashCode(), burn.GetHashCode());
        }

        [Test]
        public void GetHashCode_should_use_user_id_as_hashcode_generator()
        {
            var user = new User(2341, "Homer", "Simpson", "home_sim");

            Assert.AreEqual(2341.GetHashCode(), user.GetHashCode());
        }
    }
}
