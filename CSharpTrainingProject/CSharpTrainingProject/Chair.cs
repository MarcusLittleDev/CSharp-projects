using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSharpTrainingProject
{
    class Chair : Furniture
    {
        bool isPadded { get; }
        bool hasArms { get; }

        public Chair(string ident, string color, bool isPadded, bool hasArms): base(color, ident)
        {
            this.isPadded = isPadded;
            this.hasArms = hasArms;
        }

     

        public override string ToString()
        {
            return String.Format("Chair: ID = {3}, Color = {0}, Is Padded? = {1}, Has Arms? = {2}", this.color, this.isPadded, this.hasArms, this.ident);
        }
    }
}
