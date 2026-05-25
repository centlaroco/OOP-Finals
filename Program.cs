using System;
using System.Collections.Generic;

namespace MiniEcoMarket
{
    class Program
    {
        // Collections
        static List<Product>  products  = new List<Product>();
        static List<Order>    orders    = new List<Order>();
        static List<Farmer>   farmers   = new List<Farmer>();
        static List<Customer> customers = new List<Customer>();

        static int nextUserId    = 1;
        static int nextProductId = 1;
        static int nextOrderId   = 1;

        static User loggedIn = null;

        // ─────────────────────────────────────────────────────────────────────
        static void Main(string[] args)
        {
            LoadData();

            bool running = true;
            while (running)
            {
                if (loggedIn == null)
                    running = MainMenu();
                else if (loggedIn is Farmer)
                    FarmerMenu();
                else
                    CustomerMenu();
            }

            SaveData();
            Console.WriteLine("\nThank you for using Mini EcoMarket! Goodbye.");
        }

        //  Load & Save 
        static void LoadData()
        {
            FileManager.LoadUsers(farmers, customers);
            products = FileManager.LoadProducts();
            orders   = FileManager.LoadOrders();

            // Set ID counters so new records don't overwrite old ones
            foreach (Farmer f   in farmers)   if (f.Id > nextUserId - 1)    nextUserId    = f.Id + 1;
            foreach (Customer c in customers) if (c.Id > nextUserId - 1)    nextUserId    = c.Id + 1;
            foreach (Product p  in products)  if (p.Id > nextProductId - 1) nextProductId = p.Id + 1;
            foreach (Order o    in orders)    if (o.OrderId > nextOrderId-1) nextOrderId  = o.OrderId + 1;

            // Reattach orders to matching customers
            foreach (Order o in orders)
            {
                foreach (Customer c in customers)
                {
                    if (c.Name == o.CustomerName)
                    {
                        c.Orders.Add(o);
                        break;
                    }
                }
            }
        }

        static void SaveData()
        {
            FileManager.SaveUsers(farmers, customers);
            FileManager.SaveProducts(products);
            FileManager.SaveOrders(orders);
        }

        //  Main Menu 
        static bool MainMenu()
        {
            Console.WriteLine("\n=== MINI ECOMARKET ===");
            Console.WriteLine("[1] Register as Farmer");
            Console.WriteLine("[2] Register as Customer");
            Console.WriteLine("[3] Login");
            Console.WriteLine("[4] Browse Products");
            Console.WriteLine("[0] Exit");
            Console.Write("Choice: ");
            string choice = Console.ReadLine();

            if (choice == "1") RegisterFarmer();
            else if (choice == "2") RegisterCustomer();
            else if (choice == "3") Login();
            else if (choice == "4") ShowAllProducts();
            else if (choice == "0") return false;
            else Console.WriteLine("Invalid choice.");

            return true;
        }

        //  Register 
        static void RegisterFarmer()
        {
            Console.WriteLine("\n-- Register as Farmer --");
            try
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Password: ");
                string pass = Console.ReadLine();

                Console.Write("Farm Name: ");
                string farm = Console.ReadLine();

                Farmer f = new Farmer(nextUserId++, name, pass, farm);
                farmers.Add(f);
                Console.WriteLine("Registered successfully!");
                f.DisplayInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void RegisterCustomer()
        {
            Console.WriteLine("\n-- Register as Customer --");
            try
            {
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Password: ");
                string pass = Console.ReadLine();

                Customer c = new Customer(nextUserId++, name, pass);
                customers.Add(c);
                Console.WriteLine("Registered successfully!");
                c.DisplayInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        //  Login 
        static void Login()
        {
            Console.WriteLine("\n-- Login --");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Password: ");
            string pass = Console.ReadLine();

            foreach (Farmer f in farmers)
            {
                if (f.Name == name && f.Password == pass)
                {
                    loggedIn = f;
                    Console.WriteLine("Welcome, Farmer " + f.Name + "!");
                    return;
                }
            }

            foreach (Customer c in customers)
            {
                if (c.Name == name && c.Password == pass)
                {
                    loggedIn = c;
                    Console.WriteLine("Welcome, " + c.Name + "!");
                    return;
                }
            }

            Console.WriteLine("Invalid name or password.");
        }

        //  Farmer Menu 
        static void FarmerMenu()
        {
            Farmer farmer = (Farmer)loggedIn;

            Console.WriteLine("\n=== FARMER MENU === (" + farmer.Name + ")");
            Console.WriteLine("[1] Add Product");
            Console.WriteLine("[2] View My Products");
            Console.WriteLine("[3] Update Product");
            Console.WriteLine("[4] My Profile");
            Console.WriteLine("[5] Logout");
            Console.Write("Choice: ");
            string choice = Console.ReadLine();

            if (choice == "1") AddProduct(farmer);
            else if (choice == "2") ViewFarmerProducts(farmer);
            else if (choice == "3") UpdateProduct(farmer);
            else if (choice == "4") farmer.DisplayInfo();
            else if (choice == "5")
            {
                SaveData();
                loggedIn = null;
                Console.WriteLine("Logged out.");
            }
            else Console.WriteLine("Invalid choice.");
        }

        static void AddProduct(Farmer farmer)
        {
            Console.WriteLine("\n-- Add Product --");
            try
            {
                Console.Write("Product Name: ");
                string name = Console.ReadLine();

                Console.Write("Price: ");
                double price = double.Parse(Console.ReadLine());

                Console.Write("Stock: ");
                int stock = int.Parse(Console.ReadLine());

                Console.Write("Category (e.g. Vegetables, Fruits): ");
                string category = Console.ReadLine();

                Product p = new Product(nextProductId++, name, price, stock, category, farmer.Name);
                products.Add(p);
                Console.WriteLine("Product added!");
            }
            catch (FormatException)
            {
                // Exception handling: non-numeric input
                Console.WriteLine("Error: Price and stock must be numbers.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void ViewFarmerProducts(Farmer farmer)
        {
            Console.WriteLine("\n-- My Products --");
            bool found = false;
            foreach (Product p in products)
            {
                if (p.FarmerName == farmer.Name)
                {
                    p.Display();
                    found = true;
                }
            }
            if (!found) Console.WriteLine("You have no products listed.");
        }

        static void UpdateProduct(Farmer farmer)
        {
            ViewFarmerProducts(farmer);
            Console.Write("Enter Product ID to update: ");
            try
            {
                int id = int.Parse(Console.ReadLine());
                Product target = null;

                foreach (Product p in products)
                {
                    if (p.Id == id && p.FarmerName == farmer.Name)
                    {
                        target = p;
                        break;
                    }
                }

                // Exception handling: invalid product ID
                if (target == null)
                    throw new Exception("Product ID " + id + " not found.");

                Console.Write("New Price (press Enter to keep " + target.Price + "): ");
                string priceInput = Console.ReadLine();
                if (priceInput != "") target.Price = double.Parse(priceInput);

                Console.Write("New Stock (press Enter to keep " + target.Stock + "): ");
                string stockInput = Console.ReadLine();
                if (stockInput != "") target.Stock = int.Parse(stockInput);

                Console.WriteLine("Product updated!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter valid numbers.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // Customer Menu
        static void CustomerMenu()
        {
            Customer customer = (Customer)loggedIn;

            Console.WriteLine("\n=== CUSTOMER MENU === (" + customer.Name + ")");
            Console.WriteLine("[1] Browse All Products");
            Console.WriteLine("[2] Buy a Product");
            Console.WriteLine("[3] View My Orders");
            Console.WriteLine("[4] My Profile");
            Console.WriteLine("[5] Logout");
            Console.Write("Choice: ");
            string choice = Console.ReadLine();

            if (choice == "1") ShowAllProducts();
            else if (choice == "2") BuyProduct(customer);
            else if (choice == "3") ViewOrders(customer);
            else if (choice == "4") customer.DisplayInfo();
            else if (choice == "5")
            {
                SaveData();
                loggedIn = null;
                Console.WriteLine("Logged out.");
            }
            else Console.WriteLine("Invalid choice.");
        }

        static void ShowAllProducts()
        {
            Console.WriteLine("\n-- Available Products --");
            if (products.Count == 0)
            {
                Console.WriteLine("No products available.");
                return;
            }
            foreach (Product p in products)
                p.Display();
        }

        static void BuyProduct(Customer customer)
        {
            ShowAllProducts();
            Console.Write("\nEnter Product ID to buy: ");
            try
            {
                int id = int.Parse(Console.ReadLine());
                Product target = null;

                foreach (Product p in products)
                {
                    if (p.Id == id)
                    {
                        target = p;
                        break;
                    }
                }

                // Exception handling: invalid product ID
                if (target == null)
                    throw new Exception("Product ID " + id + " not found.");

                Console.Write("Quantity (available: " + target.Stock + "): ");
                int qty = int.Parse(Console.ReadLine());

                // Exception handling: buying more than stock
                if (qty <= 0)
                    throw new Exception("Quantity must be at least 1.");
                if (qty > target.Stock)
                    throw new Exception("Not enough stock. Only " + target.Stock + " left.");

                double total = qty * target.Price;
                target.Stock -= qty;

                Order order = new Order(nextOrderId++, customer.Name, target.Name, qty, total);
                orders.Add(order);
                customer.Orders.Add(order);

                Console.WriteLine("Purchase successful! (no payment needed)");
                order.Display();
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void ViewOrders(Customer customer)
        {
            Console.WriteLine("\n-- My Order History --");
            if (customer.Orders.Count == 0)
            {
                Console.WriteLine("No orders yet.");
                return;
            }
            foreach (Order o in customer.Orders)
                o.Display();
        }
    }
}
