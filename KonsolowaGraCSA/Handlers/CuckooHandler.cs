using KonsolowaGraCSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA.Handlers
{
    public class CuckooHandler
    {
        private List<int[]> population;
        private int iteration;
        private int populationSize; //liczba kukułek
        private int[] bestPosition;
        public List<Hash> hashes;
        private Random rnd;
        public string[,] boardTab;
        private int boardHeight;
        private int boardWidth;

        public CuckooHandler(Board board, Enemy enemy, int it, int pop, List<Hash> hashes)
        {
            bestPosition = new int[2];
            iteration = it;
            populationSize = pop;
            this.hashes = hashes;
            population = new List<int[]>();
            rnd = new Random();
            boardHeight = board.Height;
            boardWidth = board.Width;
            boardTab = board.boardTable;
            Generate();
        }

        public void Generate()
        {
            for (int i = 0; i < populationSize; i++)
            {
                int tmp1 = rnd.Next(0, boardWidth);
                int tmp2 = rnd.Next(0, boardHeight);
                population.Add(new int[] { tmp1, tmp2 });
            }
        }

        public int[] Move(int[] a)
        {
            int[] b = a;
            if (rnd.Next(0, 100) < 50)
            {
                int tmp = b[0] + Convert.ToInt16(Math.Ceiling(Levy(b[0])));
                if (tmp > 0 && tmp < boardWidth) b[0] = tmp;
            }
            else
            {
                int tmp = b[0] - Convert.ToInt16(Math.Ceiling(Levy(b[0])));
                if (tmp > 0 && tmp < boardWidth) b[0] = tmp;
            }
            if (rnd.Next(0, 100) < 50)
            {
                int tmp = b[1] + Convert.ToInt16(Math.Ceiling(Levy(b[1])));
                if (tmp > 0 && tmp < boardHeight) b[1] = tmp;
            }
            else
            {
                int tmp = b[1] - Convert.ToInt16(Math.Ceiling(Levy(b[1])));
                if (tmp > 0 && tmp < boardHeight) b[1] = tmp;
            }
            return b;
        }

        public int[] HostDecision(int[] a, string[,] board)
        {
            if (rnd.Next(0, 100) < 50) return new int[2] { rnd.Next(0, boardWidth), rnd.Next(0, boardHeight) };
            else return a;
        }

        public int Host(int neighneighborhoodSize, int[] p, string[,] board)
        {
            int counter = 0;
            for (int i = -Convert.ToInt32(neighneighborhoodSize / 2);
                i < Convert.ToInt32(neighneighborhoodSize / 2) + 1; i++)
            {
                for (int j = -Convert.ToInt32(neighneighborhoodSize / 2);
                    j < Convert.ToInt32(neighneighborhoodSize / 2) + 1; j++)
                {
                    if (p[0] + i > -1 && p[0] + i < boardWidth && p[1] + j > -1
                        && p[1] + j < boardHeight && board[p[0] + i, p[1] + j] == "#")
                        counter++;
                }
            }
            return counter;
        }

        public double FitnessFunction(int[] p1, int[] p2, string[,] board)
        {
            double dist = DistanceFunctions.Euclidean(p1, p2);//Maximum(p1, p2);

            if (CheckPosition(p1, board) || CheckPosition(p2, board))
                return dist - Math.Sqrt(boardWidth + boardHeight);
            else return dist + Math.Sqrt(boardWidth + boardHeight);
        }

        public bool CheckPosition(int[] p, string[,] board) //sprawdza czy p to #
        {
            if (p[0] >= 0 && p[1] >= 0 && p[0] < boardWidth && p[1] < boardHeight && board[p[0], p[1]] == "#") return true;
            else return false;

        }

        public double Levy(int x) //Levy
        {
            double c = rnd.NextDouble(), d = rnd.NextDouble();
            double val = Math.Sqrt(c / (2 * Math.PI)) * Math.Exp((-c * (2 * (x - d))) / Math.Pow(x - d, 3 / 2))
                * (Math.Sqrt(boardHeight + boardWidth)); //poprawne: -c / (2 * (x - d))
            return Math.Ceiling(val) - val > 0.5 ? Math.Ceiling(val) : Math.Floor(val);
        }

        public int[] FindTheBest(List<Hash> enemies, string[,] board) //znajduje najbliższ hasz
        {
            int[] a = population[0];
            foreach (var e in enemies)
            {
                for (int i = 0; i < population.Count; i++)
                {
                    if (FitnessFunction(population[i], new int[2] { e.X, e.Y }, board)
                        < FitnessFunction(a, new int[2] { e.X, e.Y }, board))
                    {
                        a = population[i];
                    }
                }
            }
            return a;
        }

        public void Destroy()
        {
            population.Clear();
            hashes.Clear();
        }

        public int[] Run(string[,] board, List<Hash> enemies)
        {
            Generate();
            this.hashes = enemies;

            for (int i = 0; i < iteration; i++)
            {
                for (int j = 0; j < population.Count; j++)
                {
                    population[j] = Move(population[j]);
                    population[j] = HostDecision(population[j], board);
                }
            }
            return Best(board);
        }

        private int[] Best(string[,] board)
        {
            return FindTheBest(hashes, board);
        }
    }
}