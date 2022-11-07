using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    public static class DistanceFunctions
    {
        public static double Euclidean(int[] p1, int[] p2)
        {
            return Math.Sqrt(Math.Pow(p1[0] - p2[0], 2) + Math.Pow(p1[1] - p2[1], 2));
        }

        public static double Manhattan(int[] p1, int[] p2)
        {
            return Math.Abs(p1[0] - p2[1]) + Math.Abs(p1[1] - p2[0]);
        }

        public static double Maximum(int[] p1, int[] p2)
        {
            return Math.Abs(p1[0] - p2[1]) < Math.Abs(p1[1] - p2[0]) ? Math.Abs(p1[0] - p2[1]) : Math.Abs(p1[1] - p2[0]);
        }

    }
}