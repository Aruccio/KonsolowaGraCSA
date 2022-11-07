using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonsolowaGraCSA
{
    internal class Player
    {
        public Player()
        {
            X = 0;
            Y = 0;
            Points = 0;
            Moves = 0;
            Symbol = "*";
            Name = "??";
        }

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            Points = 0;
            Moves = 0;
            Symbol = "*";
            Name = "??";
        }

        public Player(int x, int y, string symbol)
        {
            X = x;
            Y = y;
            Points = 0;
            Moves = 0;
            Symbol = symbol;
            Name = "??";
        }

        public Player(int x, int y, string symbol, string name)
        {
            X = x;
            Y = y;
            Points = 0;
            Moves = 0;
            Symbol = symbol;
            Name = name;
        }

        public string Name
        {
            get; set;
        }
        public string Symbol //zmienna symbol upubliczniona za pomocą akcesorów
        {
            get; set;
        }

        public int Moves //zmienna Moves upubliczniona za pomocą akcesorów
        {
            get; set;
        }
        public int Points //zmienna Points upubliczniona za pomocą akcesorów
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

        public void AddPoint() //dodaje punkty
        {
            Points++;
        }

        public void AddMove() //dodaje sumę ruchów wszystkich graczy
        {
            Moves++;
        }

    }
}