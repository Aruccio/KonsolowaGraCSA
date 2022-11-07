using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    internal class Board
    {
        public int dlug;
        public int wys;
        public int points;
        public int ruchy1 = 0;
        private Random rnd;
        private int enemy;
        public string[,] board;
        private bool endGame;
        private string winner = "";
        private List<Enemy> enemies;
        public Player player = new Player();

        public Board(int dlug, int wys, int mmm)
        {
            this.dlug = dlug;
            this.wys = wys;
            enemies = new List<Enemy>();
            points = 0;
            rnd = new Random();
            endGame = false;
            board = new string[dlug, wys];
            enemy = rnd.Next(1, Convert.ToInt32(Math.Sqrt(dlug + wys)));
            CreateEmptyBoard(); //tworzymy plansze
            SetEnemy(mmm);
        }

        public void SetEnemy()
        {
            for (int i = 0; i < enemy; i++)
            {
                int a = rnd.Next(0, dlug - 1);
                int b = rnd.Next(0, wys);
                if (board[rnd.Next(1, dlug - 1), rnd.Next(1, wys - 1)] != "#")
                    board[rnd.Next(1, dlug - 1), rnd.Next(1, wys - 1)] = "#";
                enemies.Add(new Enemy(new int[] { a, b }, "#"));
            }
        }

        private void UpdateEnemies()
        {
            enemies.Clear();
            for (int i = 0; i < dlug; i++)
            {
                for (int j = 0; j < wys; j++)
                {
                    if (board[i, j] == "#")
                        enemies.Add(new Enemy(new int[] { i, j }, "#"));
                }
            }
        }

        public void SetEnemy(int noEnemy)
        {
            for (int i = 0; i < noEnemy; i++)
            {
                int a = rnd.Next(1, dlug - 1);
                int b = rnd.Next(1, wys - 1);
                if (board[a, b] != "#")
                    board[a, b] = "#";
                enemies.Add(new Enemy(new int[] { a, b }, "#"));
            }
        }

        private void CreateEmptyBoard()
        {
            for (int i = 0; i < dlug; i++)
            {
                for (int j = 0; j < wys; j++)
                {
                    board[i, j] = " ";
                }
            }
        }

        public void SetPlayerPosition(Player player)
        {
            board[player.X, player.Y] = player.Symbol;
        }

        public int Dlug()
        {
            return dlug;
        }

        public string Winner()
        {
            return winner;
        }

        public int Wys()
        {
            return wys;
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        public bool EndGame
        {
            get { return endGame; }
        }

        public void ShowBoard()
        {
            Reset();
            for (int i = 0; i < dlug; i++)
            {
                for (int j = 0; j < wys; j++)
                {
                    Console.Write(board[i, j]);
                }

                Console.WriteLine();
            }
        }

        private bool CheckIfItIsEnd()
        {
            for (int i = 0; i < dlug; i++)
            {
                for (int j = 0; j < wys; j++)
                {
                    if (board[i, j] == "#") return false;
                }
            }
            return true;
        }

        public void MovePlayer(Player player, int xNew, int yNew)
        {
            board[player.X, player.Y] = " ";
            player.Wspolrzedne(xNew, yNew);
            board[xNew, yNew] = player.Symbol;
        }

        private void ClearArea(int x, int y)
        {
            board[x, y] = " ";
        }

        private void ShowStats(Cuckoo k1, Player g1, int flag)
        {
            switch (flag)
            {
                case 1:// gracz wygrał
                    Reset();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gracz " + g1.Name + " wygrał!");
                    Console.ResetColor();
                    winner = "Player";
                    StatsDuringGame(k1, g1);
                    break;

                case 2: //wygrał komputer
                    Reset();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gracz " + k1.name + " wygrał!");
                    Console.ResetColor();
                    winner = "Cuckoo";
                    StatsDuringGame(k1, g1);
                    break;

                case 3: //remis
                    Remis();
                    StatsDuringGame(k1, g1);
                    break;

                case 4:
                    StatsDuringGame(k1, g1);
                    break;
            }
        }

        private void StatsDuringGame(Cuckoo k1, Player g1)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Punkty (komputera) -- " + k1.Points);
            Console.WriteLine("Liczba ruchów komputera: " + k1.Moves);
            Console.WriteLine("Punkty (gracza) -- " + g1.Points);
            Console.WriteLine("Liczba ruchów gracza: " + g1.Moves);
            Console.ResetColor();
        }

        private void Remis()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Remis!!!");
            Console.ResetColor();
        }

        public void GoCuckoo(Cuckoo kukulka, Player gracz)
        {
            if (endGame) return;

            int a = kukulka.X;
            int b = kukulka.Y;

            ShowBoard();
            if (board[a, b] == "#") kukulka.AddPoint();
            ShowStats(kukulka, gracz, 4);
            if (CheckIfItIsEnd() && kukulka.Points >= Math.Ceiling((decimal)(enemy / 2)))
            {
                endGame = true;
                ShowStats(kukulka, gracz, 2);
            }

            // else if (CheckIfItIsEnd() && kukulka.Points == Math.Ceiling((decimal)(enemy / 2)))
            // {
            //     Remis();
            // }

            kukulka.Destroy();
            UpdateEnemies();

            int[] destiny = kukulka.Run(board, enemies);

            List<int[]> possMoves = new List<int[]>();

            if (b + 1 < wys && b + 1 > -1) possMoves.Add(new int[] { a, b + 1 }); //góra
            if (b > 0 && b - 1 < wys) possMoves.Add(new int[] { a, b - 1 }); //dół
            if (a + 1 < dlug && a + 1 > -1) possMoves.Add(new int[] { a + 1, b });  //prawo
            if (a - 1 < dlug && a - 1 > 0) possMoves.Add(new int[] { a - 1, b });//lewo

            ShowBoard();
            int index = 0;
            bool check = false;
            for (int i = 0; i < possMoves.Count; i++)
            {
                if (kukulka.CheckPosition(possMoves[i], board))
                {
                    index = i;
                    check = true;
                    break;
                }
            }
            if (!check)
            {
                double distance = Math.Sqrt(Math.Pow(possMoves[0][0] - destiny[0], 2) + Math.Pow(possMoves[0][1] - destiny[1], 2));
                for (int i = 1; i < possMoves.Count; i++)
                {
                    if (Math.Sqrt(Math.Pow(possMoves[i][0] - destiny[0], 2) + Math.Pow(possMoves[i][1] - destiny[1], 2)) < distance)
                    {
                        index = i;
                        distance = Math.Sqrt(Math.Pow(possMoves[i][0] - destiny[0], 2) + Math.Pow(possMoves[i][1] - destiny[1], 2));
                    }
                }
            }

            kukulka.AddMove();

            if (possMoves[index][0] < dlug && possMoves[index][0] > 0 && possMoves[index][1] < wys && possMoves[index][0] > 0 && board[possMoves[index][0], possMoves[index][1]] == "#")
            {
                kukulka.AddPoint();
                board[a, b] = " ";
                a = possMoves[index][0];
                b = possMoves[index][1];
                kukulka.X = a;
                kukulka.Y = b;
                board[a, b] = kukulka.Symbol;
            }
            else
            {
                bool f = false;
                if (b > 0 && b - 1 < wys && a + 1 < dlug && a + 1 > -1) possMoves.Add(new int[] { a + 1, b - 1 }); //prawo dół
                if (b + 1 < wys && b + 1 > 0 && a + 1 < dlug && a + 1 > -1) possMoves.Add(new int[] { a + 1, b + 1 });  //prawo góra
                if (b > 0 && b - 1 < wys && a - 1 < dlug && a - 1 > 0) possMoves.Add(new int[] { a - 1, b - 1 }); //lewo dół
                if (b + 1 < wys && b + 1 > 0 && a - 1 < dlug && a - 1 > 0) possMoves.Add(new int[] { a - 1, b + 1 });  //prawo góra

                for (int i = 4; i < 8; i++)
                {
                    if (possMoves[index][0] < dlug && possMoves[index][0] > 0 && possMoves[index][1] < wys && possMoves[index][0] > 0 && board[possMoves[index][0], possMoves[index][1]] == "#")
                    {
                        f = true;
                        if (i == 4 || i == 5)
                        {
                            board[a, b] = " ";
                            a = possMoves[i][0];
                            b = possMoves[i][1];
                            kukulka.X = a + 1;
                            kukulka.Y = b;
                            board[a, b] = kukulka.Symbol;
                            if (board[a, b] == "#") kukulka.AddPoint();
                        }
                        else
                        {
                            board[a, b] = " ";
                            a = possMoves[i][0];
                            b = possMoves[i][1];
                            kukulka.X = a;
                            kukulka.Y = b + 1;
                            board[a, b] = kukulka.Symbol;
                            if (board[a, b] == "#") kukulka.AddPoint();
                        }
                        break;
                    }
                }

                if (f == false)
                {
                    index = 0;
                    check = false;
                    for (int i = 0; i < possMoves.Count; i++)
                    {
                        if (kukulka.CheckPosition(possMoves[i], board))
                        {
                            index = i;
                            check = true;
                            break;
                        }
                    }
                    if (!check)
                    {
                        double distance = Math.Sqrt(Math.Pow(possMoves[0][0] - destiny[0], 2) + Math.Pow(possMoves[0][1] - destiny[1], 2));
                        for (int i = 1; i < possMoves.Count; i++)
                        {
                            if (Math.Sqrt(Math.Pow(possMoves[i][0] - destiny[0], 2) + Math.Pow(possMoves[i][1] - destiny[1], 2)) < distance)
                            {
                                index = i;
                                distance = Math.Sqrt(Math.Pow(possMoves[i][0] - destiny[0], 2) + Math.Pow(possMoves[i][1] - destiny[1], 2));
                            }
                        }
                    }
                    if (possMoves[index][0] < dlug && possMoves[index][0] > 0 && possMoves[index][1] < wys && possMoves[index][0] > 0)
                    {
                        board[a, b] = " ";
                        a = possMoves[index][0];
                        b = possMoves[index][1];
                        kukulka.X = a;
                        kukulka.Y = b;
                        board[a, b] = kukulka.Symbol;
                        if (board[a, b] == "#") kukulka.AddPoint();
                    }

                }

            }
            ShowBoard();
            ShowStats(kukulka, gracz, 4);

            if (CheckIfItIsEnd() && kukulka.Points > Math.Ceiling((decimal)(enemy / 2)))
            {
                endGame = true;
                ShowStats(kukulka, gracz, 2);
            }
        }

        public void Reset()
        {
            Console.Clear();
        }

        public void Go(Player gracz, Cuckoo kukulka)
        {
            if (endGame) return;
            int a = gracz.X;
            int b = gracz.Y;
            board[gracz.X, gracz.Y] = gracz.Symbol;
            ShowBoard();
            ShowStats(kukulka, gracz, 4);
            if (CheckIfItIsEnd() && gracz.Points > Math.Ceiling((decimal)(enemy / 2)))
            {
                endGame = true;
                ShowStats(kukulka, gracz, 1);
            }
            else if (CheckIfItIsEnd() && gracz.Points == Math.Ceiling((decimal)(enemy / 2)))
            {
                ShowStats(kukulka, gracz, 3);
            }
            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:

                    if (b + 1 > -1 && b + 1 < wys)
                    {
                        Reset();
                        ClearArea(a, b);
                        gracz.AddMove();
                        b += 1;
                        MovePlayer(gracz, a, b);
                        if (board[a, b] == "#") gracz.AddPoint();
                        ShowBoard();
                        ShowStats(kukulka, gracz, 4);
                    }
                    break;

                case ConsoleKey.DownArrow:

                    if (a >= 0 && a < dlug - 1)
                    {
                        Reset();
                        ClearArea(a, b);
                        a += 1;
                        gracz.AddMove();
                        MovePlayer(gracz, a, b);
                        if (board[a, b] == "#") gracz.AddPoint();
                        ShowBoard();
                        ShowStats(kukulka, gracz, 4);
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (a > 0 && a - 1 < dlug)
                    {
                        gracz.AddMove();
                        Reset();
                        ClearArea(a, b);
                        a -= 1;
                        MovePlayer(gracz, a, b);
                        if (board[a, b] == "#") gracz.AddPoint();
                        ShowBoard();
                        ShowStats(kukulka, gracz, 4);
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (b - 1 > -1 && b - 1 < wys)
                    {
                        Reset();
                        ClearArea(a, b);
                        gracz.AddMove();
                        b -= 1;
                        if (board[a, b] == "#") gracz.AddPoint();
                        MovePlayer(gracz, a, b);
                        ShowBoard();
                        ShowStats(kukulka, gracz, 4);

                    }
                    break;

                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                default: break;
            }
            if (CheckIfItIsEnd() && gracz.Points > Math.Ceiling((decimal)(enemy / 2)))
            {
                endGame = true;
                ShowStats(kukulka, gracz, 1);
            }
        }
    }
}