using System;

namespace ostrich.Core
{
    public class Product
    {
        private int price;

        public Product(int productId, string name)
        {
            if (productId < 1)
                throw new ArgumentOutOfRangeException("productId","Product ID must be a positive integer.");

            if (name == null) 
                throw new ArgumentNullException("name");
            
            ProductID = productId;
            Name = name;
            Price = price;
        }

        public int ProductID { get; private set; }

        public string Name { get; private set; }

        public bool CanBeBoughtOnCredit { get; set; }

        public virtual bool Active { get; set; }

        public int Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", value, "Price must be zero or a positive integer.");
                price = value;
            }
        }

        public string FormattedPrice
        {
            get { return string.Format("{0:N2} kr.", Price/100D); }
        }
    }
}