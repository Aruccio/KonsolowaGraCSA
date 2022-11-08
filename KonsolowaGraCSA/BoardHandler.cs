using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    public class BoardHandler
    {
        private Board board;
        private Random random;
        public string Winner = "";
        public bool EndGame = false;

        public BoardHandler(Board board, int numberOfHashes)
        {
            this.board = board;
            random = new Random();
            CreateEmptyBoard(); //tworzymy plansze
            SetEnemy(numberOfHashes);
        }

        public void Reset()
        {
            Console.Clear();
        }

        public void SetEnemy(int numberOfHashes)
        {
            int i = 0;
            while (i < numberOfHashes)
            {
                int a = random.Next(0, board.Width);
                int b = random.Next(0, board.Height);
                if (board.board[a, b] != "#")
                {
                    board.board[a, b] = "#";
                    board.Hashes.Add(new Hash(new int[] { a, b }, "#"));
                    i++;
                }
            }
        }

        public void UpdateEnemies()
        {
            board.Hashes.Clear();
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    if (board.board[i, j] == "#")
                        board.Hashes.Add(new Hash(new int[] { i, j }, "#"));
                }
            }
        }

        public void CreateEmptyBoard()
        {
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    board.board[i, j] = " ";
                }
            }
        }

        public void SetPlayerPosition(Player player)
        {
            board.board[player.X, player.Y] = player.Symbol;
        }

        public void ShowBoard()
        {
            Reset();
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    Console.Write(board.board[i, j]);
                }

                Console.WriteLine();
            }
        }

        public bool CheckIfItIsEnd()
        {
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    if (board.board[i, j] == "#") return false;
                }
            }
            return true;
        }

        public void MovePlayer(Player player, int xNew, int yNew)
        {
            board.board[player.X, player.Y] = " ";
            player.X = xNew;
            player.Y = yNew;
            board.board[xNew, yNew] = player.Symbol;
        }

        public void ClearArea(int x, int y)
        {
            board.board[x, y] = " ";
        }

        public void ShowStats(Enemy k1, Player g1, int flag)
        {
            switch (flag)
            {
                case 1:// gracz wygrał
                    Reset();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gracz " + g1.Name + " wygrał!");
                    Console.ResetColor();
                    Winner = "Player";
                    StatsDuringGame(k1, g1);
                    break;

                case 2: //wygrał komputer
                    Reset();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gracz " + k1.Name + " wygrał!");
                    Console.ResetColor();
                    Winner = "Enemy";
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

    }
}