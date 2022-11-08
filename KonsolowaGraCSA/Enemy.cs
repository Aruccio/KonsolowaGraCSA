namespace KonsolowaGraCSA
{
    public class Enemy
    {
        //czyli kukulki
        private List<int[]> population;
        private int iteration;
        private int populationSize; //liczba kukułek
        private Random rnd;
        public List<Hash> hashes;
        private int[] bestPosition;

        public string[,] board;
        private int boardHeight;
        private int boardWidth;

        public Enemy(int it, int pop, string name, List<Hash> hashes, int dlug, int wys, string symbol, int x, int y)
        {
            bestPosition = new int[2];
            X = x;
            Y = y;
            Symbol = symbol;
            Name = name;
            iteration = it;
            populationSize = pop;
            boardHeight = wys;
            boardWidth = dlug;
            this.hashes = hashes;
            population = new List<int[]>();
            rnd = new Random();
            Generate();

        }

        public string Name
        {
            get; set;
        }

        public string Symbol //zmienna symbol upubliczniona za pomocą akcesorów
        {
            get; set;
        }

        public int X
        {
            get; set;
        }

        public int Y
        {
            get; set;
        }
        public int Points //zmienna Points upubliczniona za pomocą akcesorów
        {
            get; set;
        }

        public int Moves //zmienna Points upubliczniona za pomocą akcesorów
        {
            get; set;
        }

        public void AddPoint() //dodaje punkty
        {
            Points++;
        }

        public void AddMove() //dodaje do sumy ruchów AI
        {
            Moves++;
        }

        public void Generate()
        {
            /*int xmin = x-dlug/4<0 ? 0 : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(x-dlug/6))) ;
            int xmax = x + dlug / 4 > dlug ? dlug : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(x + dlug / 6)));
            int ymin = x - wys / 4 < 0 ? 0 : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(y - dlug / 6)));
            int ymax = x + wys / 4 > wys ? wys : Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(y + dlug /6)));
            */
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

        public int[] HostDecision(int[] a, string[,] board) //nowe współzędne się nadają albo nie
        {
            if (rnd.Next(0, 100) < 50) return new int[2] { rnd.Next(0, boardWidth), rnd.Next(0, boardHeight) };
            else return a;
            //int[] b = new int[2] { rnd.Next(0, dlug) , rnd.Next(0, wys) };
            //if (Host(4, a, board) < Host(4, b, board)) return b;
            //else return a;
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

        public double FitnessFunction(int[] p1, int[] p2, string[,] board) //liczy odległość. i jesli # to zmniejsza
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

        public int[] Run(string[,] board, List<Hash> enemies) //znajduje cel podrozy gracza
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