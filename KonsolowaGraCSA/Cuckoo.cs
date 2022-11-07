using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    internal class Cuckoo
    {
        private List<int[]> popul;
        private int iteration;
        private int sizePop; //liczba kukułek
        public int dlug;
        public int wys;
        private Random rnd;
        public string name;
        public int moves = 0;
        public int pkt = 0;
        private string symbol;
        public List<Enemy> enemies;
        private int x, y;
        private int[] bestPosition;
        public string[,] board;

        public Cuckoo(int it, int pop, string name, List<Enemy> enemies, int dlug, int wys, string symbol, int x, int y, Random rnd)
        {
            bestPosition = new int[2];
            this.x = x;
            this.y = y;
            this.symbol = symbol;
            iteration = it;
            sizePop = pop;
            this.rnd = rnd;
            this.wys = wys;
            this.dlug = dlug;
            this.name = name;
            this.enemies = enemies;
            popul = new List<int[]>();
            Generate();

        }

        public string Symbol //zmienna symbol upubliczniona za pomocą akcesorów
        {
            get { return symbol; }
            set { symbol = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        //public void Wspolrzedne(int x, int y)
        //{
        //    dlug = x;
        //    wys = y;
        //}

        public int Points //zmienna Points upubliczniona za pomocą akcesorów
        {
            get { return pkt; }
            set { pkt = value; }
        }

        public int Moves //zmienna Points upubliczniona za pomocą akcesorów
        {
            get { return moves; }
            set { moves = value; }
        }

        public void AddPoint() //dodaje punkty
        {
            pkt++;
        }

        public void AddMove() //dodaje do sumy ruchów AI
        {
            moves++;
        }

        public void Generate() //generuje kukułki
        {
            /*int xmin = x-dlug/4<0 ? 0 : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(x-dlug/6))) ;
            int xmax = x + dlug / 4 > dlug ? dlug : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(x + dlug / 6)));
            int ymin = x - wys / 4 < 0 ? 0 : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(y - dlug / 6)));
            int ymax = x + wys / 4 > wys ? wys : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(y + dlug /6)));
            */
            for (int i = 0; i < sizePop; i++)
            {
                int tmp1 = rnd.Next(0, dlug);
                int tmp2 = rnd.Next(0, wys);
                popul.Add(new int[] { tmp1, tmp2 });
            }
        }

        public int[] Move(int[] a) //rusza kukułkami?
        {
            int[] b = a;
            if (rnd.Next(0, 100) < 50)
            {
                int tmp = b[0] + Convert.ToInt16(Math.Ceiling(Levy(b[0])));
                if (tmp > 0 && tmp < dlug) b[0] = tmp;
            }
            else
            {
                int tmp = b[0] - Convert.ToInt16(Math.Ceiling(Levy(b[0])));
                if (tmp > 0 && tmp < dlug) b[0] = tmp;
            }
            if (rnd.Next(0, 100) < 50)
            {
                int tmp = b[1] + Convert.ToInt16(Math.Ceiling(Levy(b[1])));
                if (tmp > 0 && tmp < wys) b[1] = tmp;
            }
            else
            {
                int tmp = b[1] - Convert.ToInt16(Math.Ceiling(Levy(b[1])));
                if (tmp > 0 && tmp < wys) b[1] = tmp;
            }
            return b;
        }

        public int[] HostDecision(int[] a, string[,] board) //nowe współzędne się nadają albo nie
        {
            if (rnd.Next(0, 100) < 50) return new int[2] { rnd.Next(0, dlug), rnd.Next(0, wys) };
            else return a;
            //int[] b = new int[2] { rnd.Next(0, dlug) , rnd.Next(0, wys) };
            //if (Host(4, a, board) < Host(4, b, board)) return b;
            //else return a;
        }

        public int Host(int neighneighborhoodSize, int[] p, string[,] board)
        {
            int counter = 0;
            for (int i = -Convert.ToInt32(neighneighborhoodSize / 2); i < Convert.ToInt32(neighneighborhoodSize / 2) + 1; i++)
            {
                for (int j = -Convert.ToInt32(neighneighborhoodSize / 2); j < Convert.ToInt32(neighneighborhoodSize / 2) + 1; j++)
                {
                    if (p[0] + i > -1 && p[0] + i < dlug && p[1] + j > -1 && p[1] + j < wys && board[p[0] + i, p[1] + j] == "#") counter++;
                }
            }
            return counter;
        }

        public double FitnessFunction(int[] p1, int[] p2) //liczy odległość. p2 to hasz
        {
            return Math.Sqrt(Math.Pow(p1[0] - p2[0], 2) + Math.Pow(p1[1] - p2[1], 2));
        }

        public double Euclidean(int[] p1, int[] p2)
        {
            return Math.Sqrt(Math.Pow(p1[0] - p2[0], 2) + Math.Pow(p1[1] - p2[1], 2));
        }

        public double Manhattan(int[] p1, int[] p2)
        {
            return Math.Abs(p1[0] - p2[1]) + Math.Abs(p1[1] - p2[0]);
        }

        public double Maximum(int[] p1, int[] p2)
        {
            return Math.Abs(p1[0] - p2[1]) < Math.Abs(p1[1] - p2[0]) ? Math.Abs(p1[0] - p2[1]) : Math.Abs(p1[1] - p2[0]);
        }

        public double FitnessFunction2(int[] p1, int[] p2, string[,] board) //liczy odległość. i jesli # to zmniejsza
        {
            double dist = Euclidean(p1, p2);//Maximum(p1, p2);

            if (CheckPosition(p1, board) || CheckPosition(p2, board))
                return dist - Math.Sqrt(dlug + wys);
            else return dist + Math.Sqrt(dlug + wys);
        }

        public bool CheckPosition(int[] p, string[,] board) //sprawdza czy p to #
        {
            if (p[0] >= 0 && p[1] >= 0 && p[0] < dlug && p[1] < wys && board[p[0], p[1]] == "#") return true;
            else return false;

        }

        public double Levy(int x) //Levy
        {
            double c = rnd.NextDouble(), d = rnd.NextDouble();
            double val = Math.Sqrt(c / (2 * Math.PI)) * Math.Exp((-c * (2 * (x - d))) / Math.Pow(x - d, 3 / 2)) * (Math.Sqrt(wys + dlug)); //poprawne: -c / (2 * (x - d))
            return Math.Ceiling(val) - val > 0.5 ? Math.Ceiling(val) : Math.Floor(val);
        }

        /*   private bool Pole()
           {
               for (int i = 0; i < Math.Ceiling(Math.Sqrt(dlug)); i++)
               {
                   for (int j = 0; j < Math.Ceiling(Math.Sqrt(dlug)); j++)
                   {
                       if (board[i, j] == "#") pole = false; //jeśli jest gdzieś #, pole = true
                       else pole = true;
                   }
               }
               return pole;
           }
           */

        public int[] FindTheBest(List<Enemy> enemies, string[,] board) //znajduje najbliższ hasz
        {
            int[] a = popul[0];
            foreach (var e in enemies)
            {
                for (int i = 0; i < popul.Count; i++)
                {
                    if (FitnessFunction(popul[i], e.Tab()) < FitnessFunction(a, e.Tab()))
                    {
                        a = popul[i];
                    }
                }
            }
            return a;
        }

        public void Destroy()
        {
            popul.Clear();
            enemies.Clear();
        }

        public int[] Run(string[,] board, List<Enemy> enemies) //znajduje cel podrozy gracza
        {
            Generate();
            this.enemies = enemies;

            for (int i = 0; i < iteration; i++)
            {
                for (int j = 0; j < popul.Count; j++)
                {
                    popul[j] = Move(popul[j]);
                    popul[j] = HostDecision(popul[j], board);
                }
            }
            return Best(board);
        }

        private int[] Best(string[,] board)
        {
            return FindTheBest(enemies, board);
        }
    }
}