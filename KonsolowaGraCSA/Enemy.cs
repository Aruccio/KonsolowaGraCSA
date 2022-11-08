using KonsolowaGraCSA.Models;

namespace KonsolowaGraCSA
{
    public class Enemy
    {
        public Enemy(string name, string symbol, int x, int y)
        {
            X = x;
            Y = y;
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

    }
}