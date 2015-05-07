using System;
using NUnit.Framework;
using ostrich.Core;

namespace ostrich.Tests
{
    [TestFixture]
    public class SeasonalProductTests
    {
        private SeasonalProduct product;

        [SetUp]
        public void Setup()
        {
            product = new SeasonalProduct(1, "Party food", 15000);
        }

        [Test]
        public void Active_should_return_true_if_season_has_not_been_specified()
        {
            Assert.IsTrue(product.Active);
        }

        [Test]
        public void Active_should_return_true_if_SeasonEndsAt_is_in_the_future()
        {
            product.SeasonEndsAt = DateTime.Now.AddDays(1);
            
            Assert.IsTrue(product.Active);
        }

        [Test]
        public void Active_should_return_false_if_season_has_ended()
        {
            product.SeasonEndsAt = DateTime.Now.AddDays(-1);

            Assert.IsFalse(product.Active);
        }

        [Test]
        public void Active_should_return_false_if_season_has_not_started()
        {
            product.SeasonStartsAt = DateTime.Now.AddDays(1);
            product.SeasonEndsAt = DateTime.Now.AddDays(2);

            Assert.IsFalse(product.Active);
        }

        [Test]
        public void Active_should_return_true_if_started_season_does_not_end()
        {
            product.SeasonStartsAt = DateTime.Now.AddDays(-1);

            Assert.IsTrue(product.Active);
        }

        [Test]
        public void Active_should_throw_exception_if_set()
        {
            Assert.Throws<InvalidOperationException>(() => product.Active = false);
        }

        [Test]
        public void SeasonEndsAt_must_come_after_SeasonStartsAt()
        {
            product.SeasonStartsAt = DateTime.Now;

            Assert.Throws<InvalidOperationException>(() => product.SeasonEndsAt = DateTime.Now.AddDays(-1));
        }

        [Test]
        public void SeasonStartsAt_must_come_be_before_SeasonEndsAt()
        {
            product.SeasonEndsAt = DateTime.Now;

            Assert.Throws<InvalidOperationException>(() => product.SeasonStartsAt = DateTime.Now.AddDays(1));
        }
    }
}