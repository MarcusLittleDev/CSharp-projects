using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace CSharpTrainingProject
{
    class Furniture
    {
        public string color { get; }
        public string ident { get; }

        public Furniture(string color, string ident)
        {
            this.color = color;
            this.ident = ident;
        }

        public Furniture toInventory(string destination)
        {
            if (File.Exists(destination))
            {
                using (TextWriter streamWriter =
                    new StreamWriter(destination, true))
                {

                    streamWriter.WriteLine(this);
                    streamWriter.Close();
                }
            }
            else
            {
                using (TextWriter streamWriter =
                    new StreamWriter(destination))
                {

                    streamWriter.WriteLine(this);
                    streamWriter.Close();
                }
            }

            return this;
        }

        public Furniture fromInventory(string destination)
        {

            if (File.Exists(destination))
            {

                string tempFile = "tempinventory";
                var lines = File.ReadLines(destination).Where(l => l != this.ToString());

                File.WriteAllLines(tempFile, lines);

                File.Delete(destination);
                File.Move(tempFile, destination);
            }
            else
            {
                Console.WriteLine("File no longer exist!");
            }

     
            return this;


        }
    }
}
