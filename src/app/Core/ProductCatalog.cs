using System;
using System.Collections.Generic;
using System.Linq;

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

        public Product TryFindById(int id)
        {
            return !products.ContainsKey(id) ? null : products[id];
        }

        public IEnumerable<Product> GetActiveProducts()
        {
            return products.Values.Where(product => product.Active);
        }

    }
}