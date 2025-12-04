using System;
using System.Collections.Generic;

namespace InventoryManagementSystem
{
    class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }

        public Product(string name, double price, int stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
        }
    }

    class Program
    {
        static List<Product> inventory = new List<Product>();

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== INVENTORY MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View Products");
                Console.WriteLine("3. Update Stock");
                Console.WriteLine("4. Remove Product");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddProduct();
                        break;

                    case "2":
                        ViewProducts();
                        break;

                    case "3":
                        UpdateStock();
                        break;

                    case "4":
                        RemoveProduct();
                        break;

                    case "5":
                        running = false;
                        Console.WriteLine("Exiting program...");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        // Method to add a product
        static void AddProduct()
        {
            Console.Write("Enter product name: ");
            string name = Console.ReadLine();

            Console.Write("Enter product price: ");
            double price = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter stock quantity: ");
            int stock = Convert.ToInt32(Console.ReadLine());

            inventory.Add(new Product(name, price, stock));
            Console.WriteLine("Product added successfully!");
        }

        // Method to view all products
        static void ViewProducts()
        {
            Console.WriteLine("\n--- Product List ---");

            if (inventory.Count == 0)
            {
                Console.WriteLine("No products available.");
                return;
            }

            for (int i = 0; i < inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {inventory[i].Name} - Price: ${inventory[i].Price} - Stock: {inventory[i].Stock}");
            }
        }

        // Method to update stock
        static void UpdateStock()
        {
            ViewProducts();

            if (inventory.Count == 0) return;

            Console.Write("Enter product number to update: ");
            int index = Convert.ToInt32(Console.ReadLine()) - 1;

            if (index < 0 || index >= inventory.Count)
            {
                Console.WriteLine("Invalid product number.");
                return;
            }

            Console.Write("Enter quantity to add/remove (negative for sold items): ");
            int amount = Convert.ToInt32(Console.ReadLine());

            inventory[index].Stock += amount;

            if (inventory[index].Stock < 0)
            {
                inventory[index].Stock = 0;
            }

            Console.WriteLine("Stock updated successfully.");
        }

        // Method to remove a product
        static void RemoveProduct()
        {
            ViewProducts();

            if (inventory.Count == 0) return;

            Console.Write("Enter product number to remove: ");
            int index = Convert.ToInt32(Console.ReadLine()) - 1;

            if (index < 0 || index >= inventory.Count)
            {
                Console.WriteLine("Invalid product number.");
                return;
            }

            inventory.RemoveAt(index);
            Console.WriteLine("Product removed successfully.");
        }
    }
}
