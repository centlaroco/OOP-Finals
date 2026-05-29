using System;
using System.Collections.Generic;
using System.IO;

namespace MiniEcoMarket
{
    // Handles saving and loading using StreamReader and StreamWriter
    public static class FileManager
    {
        static string productFile = "products.txt";
        static string orderFile   = "orders.txt";
        static string userFile    = "users.txt";

        //  PRODUCTS 

        public static void SaveProducts(List<Product> list)
        {
            StreamWriter writer = new StreamWriter(productFile);
            foreach (Product p in list)
                writer.WriteLine(p.ToString());
            writer.Close();
        }

        public static List<Product> LoadProducts()
        {
            List<Product> list = new List<Product>();
            if (!File.Exists(productFile)) return list;

            StreamReader reader = new StreamReader(productFile);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    list.Add(Product.FromString(line));
                }
                catch (Exception ex)
                {
                    // Exception handling: skip corrupted lines
                    Console.WriteLine("Skipped bad product line: " + ex.Message);
                }
            }
            reader.Close();
            return list;
        }

        //  ORDERS 
        public static void SaveOrders(List<Order> list)
        {
            StreamWriter writer = new StreamWriter(orderFile);
            for(int i = 0; i < list.Count; i++)
                writer.WriteLine(list[i].ToString());
            writer.Close();
        }

        public static List<Order> LoadOrders()
        {
            List<Order> list = new List<Order>();
            if (!File.Exists(orderFile)) return list;

            StreamReader reader = new StreamReader(orderFile);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    list.Add(Order.FromString(line));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Skipped bad order line: " + ex.Message);
                }
            }
            reader.Close();
            return list;
        }

        //  USERS (BONUS)
        public static void SaveUsers(List<Farmer> farmers, List<Customer> customers)
        {
            StreamWriter writer = new StreamWriter(userFile);
            for (int i = 0; i < farmers.Count; i++)
                writer.WriteLine(farmers[i].ToString());
            for (int i = 0; i < customers.Count; i++)
                writer.WriteLine(customers[i].ToString());
            writer.Close();
        }

        public static void LoadUsers(List<Farmer> farmers, List<Customer> customers)
        {
            if (!File.Exists(userFile)) return;

            StreamReader reader = new StreamReader(userFile);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    string[] p = line.Split('|');
                    if (p[p.Length - 1] == "Farmer")
                        farmers.Add(new Farmer(int.Parse(p[0]), p[1], p[2], p[3]));
                    else
                        customers.Add(new Customer(int.Parse(p[0]), p[1], p[2]));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Skipped bad user line: " + ex.Message);
                }
            }
            reader.Close();
        }
    }
}
