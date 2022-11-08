using KonsolowaGraCSA.Handlers;
using KonsolowaGraCSA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    public class MovingHandler
    {
        private Board board;
        private BoardHandler boardHandler;
        private CuckooHandler cuckooHandler;

        public MovingHandler(Board board, BoardHandler boardHandler, CuckooHandler cuckooHandler)
        {
            this.board = board;
            this.boardHandler = boardHandler;
            this.cuckooHandler = cuckooHandler;
        }

        public void BeforeStartSet(Player player, Enemy enemy)
        {
            board.boardTable[enemy.X, enemy.Y] = enemy.Symbol;
            board.boardTable[player.X, player.Y] = player.Symbol;

        }

        public void GoCuckoo(Enemy enemy, Player player)
        {
            if (boardHandler.EndGame) return;

            int a = enemy.X;
            int b = enemy.Y;
            boardHandler.ShowBoard();
            if (board.boardTable[a, b] == "#")
            {
                enemy.AddPoint();
            }

            boardHandler.ShowStats(enemy, player, 4);
            if (boardHandler.CheckIfItIsEnd()
                && enemy.Points >= Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.EndGame = true;
                boardHandler.ShowStats(enemy, player, 2);
            }

            cuckooHandler.Destroy();
            boardHandler.UpdateHashes();

            int[] destiny = cuckooHandler.Run(board.boardTable, board.Hashes);

            List<int[]> possMoves = new List<int[]>();

            if (b + 1 < board.Height && b + 1 > -1) possMoves.Add(new int[] { a, b + 1 }); //góra
            if (b > 0 && b - 1 < board.Height) possMoves.Add(new int[] { a, b - 1 }); //dół
            if (a + 1 < board.Width && a + 1 > -1) possMoves.Add(new int[] { a + 1, b });  //prawo
            if (a - 1 < board.Width && a - 1 > 0) possMoves.Add(new int[] { a - 1, b });//lewo

            boardHandler.ShowBoard();
            int index = 0;
            bool check = false;
            for (int i = 0; i < possMoves.Count; i++)
            {
                if (cuckooHandler.CheckPosition(possMoves[i], board.boardTable))
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

            enemy.AddMove();

            if (possMoves[index][0] < board.Width && possMoves[index][0] > 0
                && possMoves[index][1] < board.Height && possMoves[index][0] > 0
                && board.boardTable[possMoves[index][0], possMoves[index][1]] == "#")
            {
                enemy.AddPoint();
                board.boardTable[a, b] = " ";
                a = possMoves[index][0];
                b = possMoves[index][1];
                enemy.X = a;
                enemy.Y = b;
                board.boardTable[a, b] = enemy.Symbol;
            }
            else
            {
                bool f = false;

                for (int i = 4; i < 8; i++)
                {
                    if (possMoves[index][0] < board.Width && possMoves[index][0] > 0
                        && possMoves[index][1] < board.Height
                        && possMoves[index][0] > 0
                        && board.boardTable[possMoves[index][0], possMoves[index][1]] == "#")
                    {
                        f = true;
                        if (i == 4 || i == 5)
                        {
                            board.boardTable[a, b] = " ";
                            a = possMoves[i][0];
                            b = possMoves[i][1];
                            enemy.X = a + 1;
                            enemy.Y = b;
                            board.boardTable[a, b] = enemy.Symbol;
                            if (board.boardTable[a, b] == "#") enemy.AddPoint();
                        }
                        else
                        {
                            board.boardTable[a, b] = " ";
                            a = possMoves[i][0];
                            b = possMoves[i][1];
                            enemy.X = a;
                            enemy.Y = b + 1;
                            board.boardTable[a, b] = enemy.Symbol;
                            if (board.boardTable[a, b] == "#") enemy.AddPoint();
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
                        if (cuckooHandler.CheckPosition(possMoves[i], board.boardTable))
                        {
                            index = i;
                            check = true;
                            break;
                        }
                    }
                    if (!check)
                    {
                        double distance = Math.Sqrt(Math.Pow(possMoves[0][0] - destiny[0], 2)
                            + Math.Pow(possMoves[0][1] - destiny[1], 2));
                        for (int i = 1; i < possMoves.Count; i++)
                        {
                            if (Math.Sqrt(Math.Pow(possMoves[i][0] - destiny[0], 2)
                                + Math.Pow(possMoves[i][1] - destiny[1], 2)) < distance)
                            {
                                index = i;
                                distance = Math.Sqrt(Math.Pow(possMoves[i][0] - destiny[0], 2)
                                    + Math.Pow(possMoves[i][1] - destiny[1], 2));
                            }
                        }
                    }
                    if (possMoves[index][0] < board.Width && possMoves[index][0] > 0
                        && possMoves[index][1] < board.Height && possMoves[index][0] > 0)
                    {
                        board.boardTable[a, b] = " ";
                        a = possMoves[index][0];
                        b = possMoves[index][1];
                        enemy.X = a;
                        enemy.Y = b;
                        board.boardTable[a, b] = enemy.Symbol;
                        if (board.boardTable[a, b] == "#") enemy.AddPoint();
                    }

                }

            }
            boardHandler.ShowBoard();
            boardHandler.ShowStats(enemy, player, 4);

            if (boardHandler.CheckIfItIsEnd() && enemy.Points > Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.EndGame = true;
                boardHandler.ShowStats(enemy, player, 2);
            }
        }

        public void Go(Player player, Enemy enemy)
        {
            if (boardHandler.EndGame) return;
            int a = player.X;
            int b = player.Y;
            board.boardTable[player.X, player.Y] = player.Symbol;
            board.boardTable[enemy.X, enemy.Y] = enemy.Symbol;

            boardHandler.ShowBoard();
            boardHandler.ShowStats(enemy, player, 4);
            if (boardHandler.CheckIfItIsEnd()
                && player.Points > Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.EndGame = true;
                boardHandler.ShowStats(enemy, player, 1);
            }
            else if (boardHandler.CheckIfItIsEnd()
                && player.Points == Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.ShowStats(enemy, player, 3);
            }
            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:

                    if (b + 1 > -1 && b + 1 < board.Height)
                    {
                        boardHandler.Reset();
                        boardHandler.ClearArea(a, b);
                        player.AddMove();
                        b += 1;
                        MakeMoveAndUpdateGameForPlayer(player, enemy, a, b);
                    }
                    break;

                case ConsoleKey.DownArrow:

                    if (a >= 0 && a < board.Width - 1)
                    {
                        boardHandler.Reset();
                        boardHandler.ClearArea(a, b);
                        a += 1;
                        player.AddMove();
                        MakeMoveAndUpdateGameForPlayer(player, enemy, a, b);
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (a > 0 && a - 1 < board.Width)
                    {
                        player.AddMove();
                        boardHandler.Reset();
                        boardHandler.ClearArea(a, b);
                        a -= 1;
                        MakeMoveAndUpdateGameForPlayer(player, enemy, a, b);
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (b - 1 > -1 && b - 1 < board.Height)
                    {
                        player.AddMove();
                        boardHandler.Reset();
                        boardHandler.ClearArea(a, b);
                        b -= 1;
                        MakeMoveAndUpdateGameForPlayer(player, enemy, a, b);
                    }
                    break;

                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                default: break;
            }
            if (boardHandler.CheckIfItIsEnd() && player.Points > Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.EndGame = true;
                boardHandler.ShowStats(enemy, player, 1);
            }
        }

        public void MakeMoveAndUpdateGameForPlayer(Player player, Enemy enemy, int x, int y)
        {
            if (board.boardTable[x, y] == "#")
            {
                player.AddPoint();
            }
            boardHandler.MovePlayer(player, x, y);
            boardHandler.ShowBoard();
            boardHandler.ShowStats(enemy, player, 4);
        }

    }
}