using System;

namespace ostrich.Core.Exceptions
{
    public class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException(User user, Product product)
            : base(String.Format("User '{0}' has insufficient funds to buy product '{1}'", user.UserID, product.ProductID))
        {
            User = user;
            Product = product;
        }

        public User User { get; private set; }
        public Product Product { get; private set; }
    }
}