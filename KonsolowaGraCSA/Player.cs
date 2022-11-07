using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    internal class Player
    {
        public int a; //dlug
        public int b; //wys
        public int pkt;
        public int moves;
        private string symbol;
        private string name;

        public Player()
        {
            a = 0;
            b = 0;
            moves = 0;
            symbol = "*";
            name = "??";
        }

        public Player(int x, int y)
        {
            a = x;
            b = y;
            pkt = 0;
            moves = 0;
            symbol = "*";
            name = "??";
        }

        public Player(int x, int y, string symbol)
        {
            a = x;
            b = y;
            pkt = 0;
            moves = 0;
            this.symbol = symbol;
            name = "??";
        }

        public Player(int x, int y, string symbol, string name)
        {
            a = x;
            b = y;
            pkt = 0;
            moves = 0;
            this.symbol = symbol;
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Symbol //zmienna symbol upubliczniona za pomocą akcesorów
        {
            get { return symbol; }
            set { symbol = value; }
        }

        public int Moves //zmienna Moves upubliczniona za pomocą akcesorów
        {
            get { return moves; }
            set { moves = value; }
        }
        public int Points //zmienna Points upubliczniona za pomocą akcesorów
        {
            get { return pkt; }
            set { pkt = value; }
        }

        public void Wspolrzedne(int x, int y)
        {
            a = x;
            b = y;
        }

        public int IloscPunktow()
        {
            return pkt;
        }

        public void AddPoint() //dodaje punkty
        {
            pkt++;
        }

        public void AddMove() //dodaje sumę ruchów wszystkich graczy
        {
            moves++;
        }

    }
}