using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;


namespace CSharpTrainingProject
{
    class Lamp : Furniture
    {
        int height { get; }
        int wattage { get; }
        public Lamp(string ident, string color, int height, int wattage) : base(color, ident)
        {
            this.height = height;
            this.wattage = wattage;

        }

        private void writeText(TextWriter textWriter)
        {
            string inventory = "Lamp: " + this.color + " " + this.height.ToString() + " " + this.wattage.ToString();
            textWriter.WriteLine(inventory);
        }

        public override string ToString()
        {
            return String.Format("Lamp: ID = {3}, Color = {0}, Height = {1}, Wattage = {2}", this.color, this.height, this.wattage, this.ident);
        }
    }
}

