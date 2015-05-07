using System;

namespace ostrich.Core.Exceptions
{
    public class ProductNotSaleableException : Exception
    {
        public ProductNotSaleableException(User user, Product product)
            : base(string.Format("User '{0}' tried to buy inactive product '{1}'", user.UserID, product.ProductID))
        {
            
        }
    }
}