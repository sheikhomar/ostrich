using System;
using NUnit.Framework;
using ostrich.Core;

namespace ostrich.Tests
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void Constructor_should_not_allow_negative_product_id()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Product(-1, "HTC One"));
            Assert.AreEqual("productId", ex.ParamName);
        }

        [Test]
        public void Constructor_should_not_allow_zero_product_id()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Product(0, "HTC One"));
            Assert.AreEqual("productId", ex.ParamName);
        }

        [Test]
        public void Constructor_should_not_allow_null_name()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new Product(1, null));
            Assert.AreEqual("name", ex.ParamName);
        }

        [Test]
        public void Price_should_not_never_be_negative()
        {
            var product = new Product(1, "HTC One");

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => product.Price = -1);
            Assert.AreEqual("value", ex.ParamName);
        }

        [Test]
        public void Construct_should_be_set_properties_correctly()
        {
            var product = new Product(1123, "HTC Two");

            Assert.AreEqual(1123, product.ProductID);
            Assert.AreEqual("HTC Two", product.Name);
        }

        [Test]
        public void FormattedPrice_should_return_correct_string()
        {
            var product = new Product(1123, "HTC One") { Price = 10000 };

            Assert.AreEqual("100.00 kr.", product.FormattedPrice);
        }

        [Test]
        public void Properties_should_be_set_correctly()
        {
            var product = new Product(1110, "HTC Three") { Price = 1, CanBeBoughtOnCredit = true, Active = true };

            Assert.AreEqual(1110, product.ProductID);
            Assert.AreEqual("HTC Three", product.Name);
            Assert.AreEqual(1, product.Price);
            Assert.AreEqual(true, product.CanBeBoughtOnCredit);
            Assert.AreEqual(true, product.Active);
        }
    }
}