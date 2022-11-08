using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    public class Board
    {
        public int ruchy1 = 0;
        private Random rnd;
        public string[,] board;
        public Player player = new Player();

        public Board(int width, int height, int numberOfHashes)
        {
            Width = width;
            Height = height;
            Hashes = new List<Hash>();
            Points = 0;
            rnd = new Random();
            board = new string[width, height];
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

        public List<Hash> Hashes
        {
            get; set;
        }

    }
}