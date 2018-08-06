using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;


namespace CSharpTrainingProject
{
    class Table : Furniture
    {
        int length { get; }
        int width { get; }
        int height { get; }

        public Table(string ident, string color, int length, int width, int height): base(color,ident)
        {
            this.length = length;
            this.width = width;
            this.height = height;
        }


        private void writeText(TextWriter textWriter)
        {
            string inventory = "Table: " + this.color + " " + this.length.ToString()+ " " + this.width.ToString() + " " + this.height.ToString();
            textWriter.WriteLine(inventory);
        }

        public override string ToString()
        {
            return String.Format("Table: ID = {4}, Color = {0}, Length = {1}, Width = {2}, Height = {3}", this.color, this.length, this.width, this.height, this.ident);
        }
    }
}

