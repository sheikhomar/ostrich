using System;
using ConsoleUI;

namespace ostrich.ConsoleUI
{
    public class InactiveProductException : Exception
    {
        public InactiveProductException(User user, Product product)
            : base(string.Format("User '{0}' tried to buy inactive product '{1}'", user.UserID, product.ProductID))
        {
            
        }
    }
}