using System;

namespace ostrich.Core.Exceptions
{
    public class ProductNotSaleableException : Exception
    {
        public ProductNotSaleableException(Product product)
        {
            if (product == null) 
                throw new ArgumentNullException("product");

            Product = product;
        }
        
        public Product Product { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format("Product '{0}' is not saleable", Product.ProductID);
            }
        }
    }
}