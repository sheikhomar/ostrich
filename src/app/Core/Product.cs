using System;

namespace ostrich.Core
{
    public class Product
    {
        public Product(int productId, string name, int price)
        {
            if (productId < 1)
                throw new ArgumentException("Product ID must be a positive integer.", "productId");

            if (name == null) 
                throw new ArgumentNullException("name");
            
            ProductID = productId;
            Name = name;
            Price = price;
        }

        public int ProductID { get; private set; }

        public string Name { get; private set; }

        public int Price { get; set; }

        public bool CanBeBoughtOnCredit { get; set; }

        public virtual bool Active { get; set; }

        public string FormattedPrice
        {
            get { return String.Format("{0:N2} kr.", Price/100); }
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", ProductID, Name, Price);
        }
    }
}