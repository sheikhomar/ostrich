using System;

namespace ostrich.Core.Exceptions
{
    public class InsufficientCreditsException : Exception
    {
        public InsufficientCreditsException(User user, Product product)
        {
            if (user == null) 
                throw new ArgumentNullException("user");
            if (product == null) 
                throw new ArgumentNullException("product");

            User = user;
            Product = product;
        }

        public User User { get; private set; }
        public Product Product { get; private set; }
        public override string Message
        {
            get
            {
                return String.Format("User '{0}' has insufficient funds to buy product '{1}'", User.UserID, Product.ProductID);
            }
        }
    }
}