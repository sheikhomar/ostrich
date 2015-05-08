using System;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class BuyTransaction : Transaction
    {
        public Product Product { get; set; }

        public BuyTransaction(int id, User user, DateTime date, Product product)
            : base(id, user, date)
        {
            if (product == null) 
                throw new ArgumentNullException("product");

            Amount = product.Price*-1;

            Product = product;
        }

        public override void Execute()
        {
            int newUserBalance = User.Balance - Product.Price;

            if (User.Balance < 0 && newUserBalance > 0)
                throw new BalanceUnderflowException(User, newUserBalance);

            if (newUserBalance < 0 && !Product.CanBeBoughtOnCredit)
                throw new InsufficientCreditsException(User, Product);

            if (!Product.Active)
                throw new ProductNotSaleableException(Product);

            User.Balance = newUserBalance;
        }

        public override string ToString()
        {
            return String.Format("{0} Type=Buy ProductID={1}", base.ToString(), Product.ProductID);
        }
    }
}