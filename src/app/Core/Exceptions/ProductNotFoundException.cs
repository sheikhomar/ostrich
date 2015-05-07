using System;

namespace ostrich.Core.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(int id) :
            base(String.Format("Product with the ID {0} was not found.", id))
        {
            ProductID = id;
        }

        public int ProductID { get; private set; }
    }
}