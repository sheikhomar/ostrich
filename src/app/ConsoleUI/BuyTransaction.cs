using System;
using ConsoleUI;

namespace ostrich.ConsoleUI
{
    public class BuyTransaction : Transaction
    {
        public Product Product { get; set; }

        public BuyTransaction(int id, User user, DateTime date, Product product, int amount)
            : base(id, user, date, amount)
        {
            if (product == null) throw new ArgumentNullException("product");

            Product = product;
        }

        /// <summary>
        /// Returns whether this transaction can be executed without raising an exception.
        /// </summary>
        public bool IsValid
        {
            get { return User.Balance > Amount && Product.Active; }
        }

        public override void Execute()
        {
            if (User.Balance < Amount)
                throw new InsufficientCreditsException(User, Product);

            if (!Product.Active)
                throw new ProductNotSaleableException(User, Product);

            User.Balance += Amount;
        }
    }
}