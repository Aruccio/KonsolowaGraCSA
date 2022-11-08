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

        public MovingHandler(Board board, BoardHandler boardHandler)
        {
            this.board = board;
            this.boardHandler = boardHandler;
        }

        public void GoCuckoo(Enemy kukulka, Player gracz)
        {
            if (boardHandler.EndGame) return;

            int a = kukulka.X;
            int b = kukulka.Y;

            boardHandler.ShowBoard();
            if (board.board[a, b] == "#") kukulka.AddPoint();
            boardHandler.ShowStats(kukulka, gracz, 4);
            if (boardHandler.CheckIfItIsEnd()
                && kukulka.Points >= Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.EndGame = true;
                boardHandler.ShowStats(kukulka, gracz, 2);
            }

            kukulka.Destroy();
            boardHandler.UpdateEnemies();

            int[] destiny = kukulka.Run(board.board, board.Hashes);

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
                if (kukulka.CheckPosition(possMoves[i], board.board))
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

            if (possMoves[index][0] < board.Width && possMoves[index][0] > 0
                && possMoves[index][1] < board.Height && possMoves[index][0] > 0
                && board.board[possMoves[index][0], possMoves[index][1]] == "#")
            {
                kukulka.AddPoint();
                board.board[a, b] = " ";
                a = possMoves[index][0];
                b = possMoves[index][1];
                kukulka.X = a;
                kukulka.Y = b;
                board.board[a, b] = kukulka.Symbol;
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
                    if (possMoves[index][0] < board.Width && possMoves[index][0] > 0
                        && possMoves[index][1] < board.Height
                        && possMoves[index][0] > 0
                        && board.board[possMoves[index][0], possMoves[index][1]] == "#")
                    {
                        f = true;
                        if (i == 4 || i == 5)
                        {
                            board.board[a, b] = " ";
                            a = possMoves[i][0];
                            b = possMoves[i][1];
                            kukulka.X = a + 1;
                            kukulka.Y = b;
                            board.board[a, b] = kukulka.Symbol;
                            if (board.board[a, b] == "#") kukulka.AddPoint();
                        }
                        else
                        {
                            board.board[a, b] = " ";
                            a = possMoves[i][0];
                            b = possMoves[i][1];
                            kukulka.X = a;
                            kukulka.Y = b + 1;
                            board.board[a, b] = kukulka.Symbol;
                            if (board.board[a, b] == "#") kukulka.AddPoint();
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
                        if (kukulka.CheckPosition(possMoves[i], board.board))
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
                        board.board[a, b] = " ";
                        a = possMoves[index][0];
                        b = possMoves[index][1];
                        kukulka.X = a;
                        kukulka.Y = b;
                        board.board[a, b] = kukulka.Symbol;
                        if (board.board[a, b] == "#") kukulka.AddPoint();
                    }

                }

            }
            boardHandler.ShowBoard();
            boardHandler.ShowStats(kukulka, gracz, 4);

            if (boardHandler.CheckIfItIsEnd() && kukulka.Points > Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.EndGame = true;
                boardHandler.ShowStats(kukulka, gracz, 2);
            }
        }

        public void Go(Player gracz, Enemy kukulka)
        {
            if (boardHandler.EndGame) return;
            int a = gracz.X;
            int b = gracz.Y;
            board.board[gracz.X, gracz.Y] = gracz.Symbol;
            boardHandler.ShowBoard();
            boardHandler.ShowStats(kukulka, gracz, 4);
            if (boardHandler.CheckIfItIsEnd()
                && gracz.Points > Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.EndGame = true;
                boardHandler.ShowStats(kukulka, gracz, 1);
            }
            else if (boardHandler.CheckIfItIsEnd()
                && gracz.Points == Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.ShowStats(kukulka, gracz, 3);
            }
            ConsoleKeyInfo key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:

                    if (b + 1 > -1 && b + 1 < board.Height)
                    {
                        boardHandler.Reset();
                        boardHandler.ClearArea(a, b);
                        gracz.AddMove();
                        b += 1;
                        boardHandler.MovePlayer(gracz, a, b);
                        if (board.board[a, b] == "#") gracz.AddPoint();
                        boardHandler.ShowBoard();
                        boardHandler.ShowStats(kukulka, gracz, 4);
                    }
                    break;

                case ConsoleKey.DownArrow:

                    if (a >= 0 && a < board.Width - 1)
                    {
                        boardHandler.Reset();
                        boardHandler.ClearArea(a, b);
                        a += 1;
                        gracz.AddMove();
                        boardHandler.MovePlayer(gracz, a, b);
                        if (board.board[a, b] == "#") gracz.AddPoint();
                        boardHandler.ShowBoard();
                        boardHandler.ShowStats(kukulka, gracz, 4);
                    }
                    break;

                case ConsoleKey.UpArrow:
                    if (a > 0 && a - 1 < board.Width)
                    {
                        gracz.AddMove();
                        boardHandler.Reset();
                        boardHandler.ClearArea(a, b);
                        a -= 1;
                        boardHandler.MovePlayer(gracz, a, b);
                        if (board.board[a, b] == "#") gracz.AddPoint();
                        boardHandler.ShowBoard();
                        boardHandler.ShowStats(kukulka, gracz, 4);
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    if (b - 1 > -1 && b - 1 < board.Height)
                    {
                        boardHandler.Reset();
                        boardHandler.ClearArea(a, b);
                        gracz.AddMove();
                        b -= 1;
                        if (board.board[a, b] == "#") gracz.AddPoint();
                        boardHandler.MovePlayer(gracz, a, b);
                        boardHandler.ShowBoard();
                        boardHandler.ShowStats(kukulka, gracz, 4);

                    }
                    break;

                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;

                default: break;
            }
            if (boardHandler.CheckIfItIsEnd() && gracz.Points > Math.Ceiling((decimal)(board.Hashes.Count / 2)))
            {
                boardHandler.EndGame = true;
                boardHandler.ShowStats(kukulka, gracz, 1);
            }
        }

    }
}