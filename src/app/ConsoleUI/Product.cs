using System;
using System.Diagnostics;

namespace ConsoleUI
{
    public class Product
    {
        private int productId;
        private string name;
        // TODO: Must be unique
        public int ProductID
        {
            get { return productId; }
            set
            {
                if (value < 1)
                    throw new ArgumentException("Product ID must not be lower than 1.");
                productId = value;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value == null)
                    throw new ArgumentException("Name cannot be null.");
                name = value;
            }
        }

        public double Price { get; set; }
        public virtual bool Active { get; set; }
        public bool CanBeBoughtOnCredit { get; set; }
    }
}