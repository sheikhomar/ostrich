using System;
using System.Collections.Generic;
using System.Linq;
using ostrich.Core.Exceptions;

namespace ostrich.Core
{
    public class ProductCatalog
    {
        private readonly IDictionary<int, Product> products;

        public ProductCatalog()
        {
            products = new Dictionary<int, Product>();
        }

        public void Add(Product product)
        {
            if (product == null) 
                throw new ArgumentNullException("product");

            // TODO: Make custom exception here.
            if (products.ContainsKey(product.ProductID))
                throw new Exception("Product already exists.");

            products.Add(product.ProductID, product);
        }

        public Product GetProduct(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException("id", id, "Product ID must be a positive integer.");
            if (!products.ContainsKey(id))
                throw new ProductNotFoundException(id);

            return products[id];
        }

        public IEnumerable<Product> GetActiveProducts()
        {
            return products.Values.Where(product => product.Active);
        }

    }
}