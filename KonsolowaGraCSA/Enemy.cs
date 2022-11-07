using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    internal class Enemy
    {
        private int a;
        private int b;
        private string symbol;
        //double distance;

        public Enemy(int[] wspolrzedne, string symbol)
        {
            a = wspolrzedne[0];
            b = wspolrzedne[1];
            this.symbol = symbol;
        }

        public int[] Tab()
        {
            return new int[] { a, b };
        }

        /*public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        */
    }
}