using System;

namespace MiniEcoMarket
{
    public class Product
    {
        // Private fields (Encapsulation)
        private int _id;
        private string _name;
        private double _price;
        private int _stock;

        // Public properties
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Product name cannot be empty.");
                _name = value;
            }
        }

        public double Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                    throw new Exception("Price cannot be negative.");
                _price = value;
            }
        }

        public int Stock
        {
            get { return _stock; }
            set
            {
                if (value < 0)
                    throw new Exception("Stock cannot be negative.");
                _stock = value;
            }
        }

        public string Category { get; set; }
        public string FarmerName { get; set; }

        public Product(int id, string name, double price, int stock, string category, string farmerName)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
            Category = category;
            FarmerName = farmerName;
        }

        public void Display()
        {
            Console.WriteLine("[" + Id + "] " + Name + " - P" + Price + " | Stock: " + Stock + " | " + Category + " | By: " + FarmerName);
        }

        // Save to file
        public override string ToString()
        {
            return Id + "|" + Name + "|" + Price + "|" + Stock + "|" + Category + "|" + FarmerName;
        }

        // Load from file
        public static Product FromString(string line)
        {
            string[] p = line.Split('|');
            if (p.Length != 6)
                throw new Exception("Bad product data: " + line);
            return new Product(int.Parse(p[0]), p[1], double.Parse(p[2]), int.Parse(p[3]), p[4], p[5]);
        }
    }
}
