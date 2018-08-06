using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CSharpTrainingProject
{
    class Program
    {
        static void Main(string[] args)
        {

            bool running = true;
            List<Furniture> inventory = new List<Furniture>();
            Console.WriteLine("Enter your desired inventory path: (press enter for default path)");
            string destination = Console.ReadLine() + "inventory.txt";
            Chair chair1 = new Chair("1","blue",true, true);
            Chair chair2 = new Chair("2","pink", false, true);
            Chair chair3 = new Chair("7", "blue", true, true);
            Chair chair4 = new Chair("8", "pink", false, true);
            inventory.Add(chair1.toInventory(destination));
            inventory.Add(chair2.toInventory(destination));
            inventory.Add(chair3.toInventory(destination));
            inventory.Add(chair4.toInventory(destination));
            Table table1 = new Table("9","orange",4,3,2);
            Table table2 = new Table("3", "orange", 4, 3, 2);
            inventory.Add(table1.toInventory(destination));
            inventory.Add(table2.toInventory(destination));
            Lamp lamp1 = new Lamp("4", "purple", 2, 57);
            Lamp lamp2 = new Lamp("5", "brown", 5, 29);
            Lamp lamp3 = new Lamp("6", "purple", 2, 57);
            Lamp lamp4 = new Lamp("10", "brown", 5, 29);
            inventory.Add(lamp1.toInventory(destination));
            inventory.Add(lamp2.toInventory(destination));
            inventory.Add(lamp3.toInventory(destination));
            inventory.Add(lamp4.toInventory(destination));
            Office office = new Office();

            while (running) {
                displayInventory(destination, office);
                Console.WriteLine(" ");
                Console.WriteLine("What would you like to do; 'Add item to office, remove item from office, or stop, display number of chairs, lamps, tables? ");
                Console.WriteLine("R for remove, A for add, S for stop, C for chairs, L for lamps, T for tables... ");
                string choice = Console.ReadLine();
                running = choices(office, inventory, choice, destination);
                Console.WriteLine(" ");
            }

            File.Delete(destination);
        }

        public static bool choices(Office office, List<Furniture> inventory, string choice,string destination)
        {
            string ident;
            switch (choice.ToUpper())
            {
                case "R":
                    Console.WriteLine("ID of Item to remove from office? ");
                    ident = Console.ReadLine();
                    removeFromOffice(office,  destination, ident);
                    break;

                case "A":
                    Console.WriteLine("ID of Item to add to office?");
                    ident = Console.ReadLine();
                    addToOffice(office, inventory, destination, ident);
                    break;

                case "S":
                    Console.WriteLine("Goodbye...");
                    return false;

                case "C":
                    Console.WriteLine("\nNumber of chairs in the office: " +office.getCount<Chair>());
                    break;

                case "T":
                    Console.WriteLine("\nNumber of chairs in the office: " +office.getCount<Table>());
                    break;

                case "L":
                    Console.WriteLine("\nNumber of chairs in the office: " +office.getCount<Lamp>());
                    break;

                default:
                    Console.WriteLine("Not a valid choice...");
                    break;
            }

            return true;
        }

        public static void removeFromOffice(Office office, string destination, string ident)
        {

            office.removeFurniture(ident, destination);

        }
        public static void addToOffice(Office office, List<Furniture> inventory, string destination, string ident)
        {


            for (var i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].ident == ident)
                {
                    office.addFurniture(inventory[i].fromInventory(destination));

                }
            }
        }
        public static void displayInventory(string destination, Office office)
        {
            Console.WriteLine("\nOffice items:");
            foreach(var item in office.furniture)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("\nInventory Items:");
            using (TextReader streamReader =
                new StreamReader(destination))
            {
                Console.WriteLine(streamReader.ReadToEnd());
            }
            
        }
    }
}
