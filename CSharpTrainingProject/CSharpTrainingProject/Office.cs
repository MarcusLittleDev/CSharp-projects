using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CSharpTrainingProject
{
    class Office
    {
        public List<Furniture> furniture { get; }
        public Office()
        {
            this.furniture = new List<Furniture>();

        }

        public void addFurniture(Furniture item)
        {

            this.furniture.Add(item);
            Console.WriteLine("The furniture has been added to the office!");
        }
        public void removeFurniture(string ident,string destination)
        {
            for(var i = 0; i < this.furniture.Count; i++)
            {
                if (this.furniture[i].ident== ident) {
                    this.furniture[i].toInventory(destination);
                    this.furniture.Remove(this.furniture[i]);
                    Console.WriteLine("Furniture has been removed");
                }
            }
            
        }

        public int getCount<T>()
        {
            int amount = 0;

            IEnumerable<Furniture> piece =
                from chair in this.furniture
                where chair is T
                select chair;

            foreach (var item in piece)
            {
                amount += 1;
            }
            return amount;

        }

    }
}
