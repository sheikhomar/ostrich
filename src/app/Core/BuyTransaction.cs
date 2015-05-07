using System;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class BuyTransaction : Transaction
    {
        public Product Product { get; set; }

        public BuyTransaction(int id, User user, DateTime date, Product product)
            : base(id, user, date, product.Price * -1)
        {
            if (product == null) throw new ArgumentNullException("product");

            Product = product;
        }

        public override void Execute()
        {
            int newUserBalance = User.Balance - Product.Price;

            if (newUserBalance < 0)
                throw new InsufficientCreditsException(User, Product);

            if (!Product.Active)
                throw new ProductNotSaleableException(Product);

            User.Balance = newUserBalance;
        }

        public override string ToString()
        {
            return String.Format("{0} User '{1}' has bought '{2}'", base.ToString(), User.UserName, Product.Name);
        }
    }
}