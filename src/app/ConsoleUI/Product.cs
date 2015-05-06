using System;
using System.Diagnostics;

namespace ConsoleUI
{
    public class Product
    {
        public Product(int productId, string name, int price, bool active = true)
        {
            if (productId < 1)
                throw new ArgumentException("Product ID cannot not be lower than 1.", "productId");

            if (name == null) 
                throw new ArgumentNullException("name");
            
            ProductID = productId;
            Name = name;
            Price = price;
            Active = active;
        }

        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public virtual bool Active { get; set; }
        public bool CanBeBoughtOnCredit { get; set; }

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