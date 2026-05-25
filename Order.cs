using System;

namespace MiniEcoMarket
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string Date { get; set; }

        public Order(int orderId, string customerName, string productName, int quantity, double totalPrice)
        {
            OrderId = orderId;
            CustomerName = customerName;
            ProductName = productName;
            Quantity = quantity;
            TotalPrice = totalPrice;
            Date = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void Display()
        {
            Console.WriteLine("Order #" + OrderId + " | " + ProductName + " x" + Quantity + " | Total: P" + TotalPrice + " | Date: " + Date);
        }

        // Save to file
        public override string ToString()
        {
            return OrderId + "|" + CustomerName + "|" + ProductName + "|" + Quantity + "|" + TotalPrice + "|" + Date;
        }

        // Load from file
        public static Order FromString(string line)
        {
            string[] p = line.Split('|');
            if (p.Length != 6)
                throw new Exception("Bad order data: " + line);

            Order o = new Order(int.Parse(p[0]), p[1], p[2], int.Parse(p[3]), double.Parse(p[4]));
            o.Date = p[5];
            return o;
        }
    }
}
