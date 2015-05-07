using System;

namespace ostrich.Core.Exceptions
{
    public class ProductExistsException : Exception
    {
        public ProductExistsException(Product oldProduct, Product newProduct)
        {
            if (oldProduct == null) 
                throw new ArgumentNullException("oldProduct");
            if (newProduct == null) 
                throw new ArgumentNullException("newProduct");

            OldProduct = oldProduct;
            NewProduct = newProduct;
        }

        public Product OldProduct { get; private set; }
        public Product NewProduct { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format("Another product with product ID {0} already exists in the catalog.", NewProduct.ProductID);
            }
        }
    }
}