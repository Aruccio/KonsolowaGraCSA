using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    internal class Board
    {
        public int ruchy1 = 0;
        private Random rnd;
        public string[,] board;
        private bool endGame;
        private string winner = "";
        public Player player = new Player();

        public Board(int width, int height, int numberOfHashes)
        {
            Width = width;
            Height = height;
            Hashes = new List<Hash>();
            Points = 0;
            rnd = new Random();
            endGame = false;
            board = new string[width, height];
            CreateEmptyBoard(); //tworzymy plansze
            SetEnemy(numberOfHashes);
        }

        public int Width
        {
            get; set;
        }

        public int Height
        {
            get; set;
        }

        public int Points { get; set; }

        public void SetEnemy(int numberOfHashes)
        {
            int i = 0;
            while (i < numberOfHashes)
            {
                int a = rnd.Next(0, Width);
                int b = rnd.Next(0, Height);
                if (board[a, b] != "#")
                {
                    board[a, b] = "#";
                    Hashes.Add(new Hash(new int[] { a, b }, "#"));
                    i++;
                }
            }
        }

        private void UpdateEnemies()
        {
            Hashes.Clear();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (board[i, j] == "#")
                        Hashes.Add(new Hash(new int[] { i, j }, "#"));
                }
            }
        }

        private void CreateEmptyBoard()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    board[i, j] = " ";
                }
            }
        }

        public void SetPlayerPosition(Player player)
        {
            board[player.X, player.Y] = player.Symbol;
        }

        public string Winner()
        {
            return winner;
        }

        public List<Hash> Hashes
        {
            get; set;
        }

        public bool EndGame
        {
            get { return endGame; }
        }

        public void ShowBoard()
        {
            Reset();
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Console.Write(board[i, j]);
                }

                Console.WriteLine();
            }
        }

        private bool CheckIfItIsEnd()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (board[i, j] == "#") return false;
                }
            }
            return true;
        }

        public void MovePlayer(Player player, int xNew, int yNew)
        {
            board[player.X, player.Y] = " ";
            player.X = xNew;
            player.Y = yNew;
            board[xNew, yNew] = player.Symbol;
        }

        private void ClearArea(int x, int y)
        {
            board[x, y] = " ";
        }

        private void ShowStats(Enemy k1, Player g1, int flag)
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
                    Console.WriteLine("Gracz " + k1.Name + " wygrał!");
                    Console.ResetColor();
                    winner = "Enemy";
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

        private void StatsDuringGame(Enemy k1, Player g1)
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

        public void GoCuckoo(Enemy kukulka, Player gracz)
        {
            if (endGame) return;

            int a = kukulka.X;
            int b = kukulka.Y;

            ShowBoard();
            if (board[a, b] == "#") kukulka.AddPoint();
            ShowStats(kukulka, gracz, 4);
            if (CheckIfItIsEnd() && kukulka.Points >= Math.Ceiling((decimal)(Hashes.Count / 2)))
            {
                endGame = true;
                ShowStats(kukulka, gracz, 2);
            }

            kukulka.Destroy();
            UpdateEnemies();

            int[] destiny = kukulka.Run(board, Hashes);

            List<int[]> possMoves = new List<int[]>();

            if (b + 1 < Height && b + 1 > -1) possMoves.Add(new int[] { a, b + 1 }); //góra
            if (b > 0 && b - 1 < Height) possMoves.Add(new int[] { a, b - 1 }); //dół
            if (a + 1 < Width && a + 1 > -1) possMoves.Add(new int[] { a + 1, b });  //prawo
            if (a - 1 < Width && a - 1 > 0) possMoves.Add(new int[] { a - 1, b });//lewo

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

            if (possMoves[index][0] < Width && possMoves[index][0] > 0 && possMoves[index][1] < Height && possMoves[index][0] > 0 && board[possMoves[index][0], possMoves[index][1]] == "#")
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
                //if (b > 0 && b - 1 < height && a + 1 < width && a + 1 > -1) possMoves.Add(new int[] { a + 1, b - 1 }); //prawo dół
                //if (b + 1 < height && b + 1 > 0 && a + 1 < width && a + 1 > -1) possMoves.Add(new int[] { a + 1, b + 1 });  //prawo góra
                //if (b > 0 && b - 1 < height && a - 1 < width && a - 1 > 0) possMoves.Add(new int[] { a - 1, b - 1 }); //lewo dół
                //if (b + 1 < height && b + 1 > 0 && a - 1 < width && a - 1 > 0) possMoves.Add(new int[] { a - 1, b + 1 });  //prawo góra

                for (int i = 4; i < 8; i++)
                {
                    if (possMoves[index][0] < Width && possMoves[index][0] > 0 && possMoves[index][1] < Height && possMoves[index][0] > 0 && board[possMoves[index][0], possMoves[index][1]] == "#")
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
                    if (possMoves[index][0] < Width && possMoves[index][0] > 0 && possMoves[index][1] < Height && possMoves[index][0] > 0)
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

            if (CheckIfItIsEnd() && kukulka.Points > Math.Ceiling((decimal)(Hashes.Count / 2)))
            {
                endGame = true;
                ShowStats(kukulka, gracz, 2);
            }
        }

        public void Reset()
        {
            Console.Clear();
        }

        public void Go(Player gracz, Enemy kukulka)
        {
            if (endGame) return;
            int a = gracz.X;
            int b = gracz.Y;
            board[gracz.X, gracz.Y] = gracz.Symbol;
            ShowBoard();
            ShowStats(kukulka, gracz, 4);
            if (CheckIfItIsEnd() && gracz.Points > Math.Ceiling((decimal)(Hashes.Count / 2)))
            {
                endGame = true;
                ShowStats(kukulka, gracz, 1);
            }
            else if (CheckIfItIsEnd() && gracz.Points == Math.Ceiling((decimal)(Hashes.Count / 2)))
            {
                ShowStats(kukulka, gracz, 3);
            }
            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:

                    if (b + 1 > -1 && b + 1 < Height)
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

                    if (a >= 0 && a < Width - 1)
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
                    if (a > 0 && a - 1 < Width)
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
                    if (b - 1 > -1 && b - 1 < Height)
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
            if (CheckIfItIsEnd() && gracz.Points > Math.Ceiling((decimal)(Hashes.Count / 2)))
            {
                endGame = true;
                ShowStats(kukulka, gracz, 1);
            }
        }
    }
}