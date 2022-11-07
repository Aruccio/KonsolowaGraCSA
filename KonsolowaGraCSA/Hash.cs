using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    internal class Hash
    {
        public Hash(int[] coordinates, string symbol)
        {
            X = coordinates[0];
            Y = coordinates[1];
            Symbol = symbol;
        }

        public string Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}