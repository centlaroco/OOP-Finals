using System;
using System.Collections.Generic;

namespace MiniEcoMarket
{
    // Customer inherits from User (Inheritance)
    public class Customer : User
    {
        // List<Order> collection stores purchase history
        public List<Order> Orders { get; set; }

        public Customer(int id, string name, string password)
            : base(id, name, password)
        {
            Orders = new List<Order>();
        }

        // Polymorphism: different DisplayInfo from Farmer
        public override void DisplayInfo()
        {
            Console.WriteLine("--- Customer Profile ---");
            Console.WriteLine("ID       : " + Id);
            Console.WriteLine("Name     : " + Name);
            Console.WriteLine("Role     : " + GetRole());
            Console.WriteLine("Orders   : " + Orders.Count);
        }

        public override string GetRole()
        {
            return "Customer";
        }

        public override string ToString()
        {
            return Id + "|" + Name + "|" + Password + "|Customer";
        }
    }
}
