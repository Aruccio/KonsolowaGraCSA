using KonsolowaGraCSA;
using System.Diagnostics;

List<double[]> k = new List<double[]>();
List<string> winners = new List<string>();

//for (int i = 0; i < 5; i++)
//{
Random rnd = new Random();
int dlug = 10, wys = 10;
Board b = new Board(dlug, wys, 30); //plansza jest uworzona
Stopwatch stoper = new Stopwatch();
Player graczI = new Player(dlug - 1, wys - 1, "&", "Ja");
Cuckoo kuk = new Cuckoo(10, 10, "Kukulka", b.Enemies, dlug, wys, "@", 0, 0, rnd);
List<Player> gracze = new List<Player>();
gracze.Add(graczI);

stoper.Start();
while (b.EndGame == false)
{
    b.Go(graczI, kuk);
    b.GoCuckoo(kuk, graczI);
    //  b.GoCuckoo(kuku, graczI);
}
stoper.Stop();
long s = stoper.ElapsedMilliseconds / 1000;
Console.WriteLine(s);
k.Add(new double[5] { 400, 50, s, kuk.moves, graczI.Moves });
winners.Add(b.Winner());
Console.ReadKey();