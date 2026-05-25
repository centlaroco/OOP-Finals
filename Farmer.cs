using System;

namespace MiniEcoMarket
{
    // Farmer inherits from User (Inheritance)
    public class Farmer : User
    {
        public string FarmName { get; set; }

        public Farmer(int id, string name, string password, string farmName)
            : base(id, name, password)
        {
            FarmName = farmName;
        }

        // Polymorphism: each user type shows different info
        public override void DisplayInfo()
        {
            Console.WriteLine("--- Farmer Profile ---");
            Console.WriteLine("ID       : " + Id);
            Console.WriteLine("Name     : " + Name);
            Console.WriteLine("Farm     : " + FarmName);
            Console.WriteLine("Role     : " + GetRole());
        }

        public override string GetRole()
        {
            return "Farmer";
        }

        // Save to file
        public override string ToString()
        {
            return Id + "|" + Name + "|" + Password + "|" + FarmName + "|Farmer";
        }
    }
}
