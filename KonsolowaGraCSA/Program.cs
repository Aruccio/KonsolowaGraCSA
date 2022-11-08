using KonsolowaGraCSA;
using System.Diagnostics;

List<double[]> k = new List<double[]>();
List<string> winners = new List<string>();

//for (int i = 0; i < 5; i++)
//{
Random rnd = new Random();
int dlug = 10, wys = 10;
int hashes = 30;
Board board = new Board(dlug, wys, hashes); //plansza jest uworzona
BoardHandler boardHandler = new BoardHandler(board, hashes); //podpiete funkcje do planszy
MovingHandler movingHandler = new MovingHandler(board, boardHandler); //podpiete obie funkcje ruchu

Stopwatch stoper = new Stopwatch();
Player graczI = new Player(dlug - 1, wys - 1, "&", "Ja");
Enemy kuk = new Enemy(10, 10, "Kukulka", board.Hashes, dlug, wys, "@", 0, 0);
List<Player> gracze = new List<Player>();
gracze.Add(graczI);

stoper.Start();
while (boardHandler.EndGame == false)
{
    movingHandler.Go(graczI, kuk);
    movingHandler.GoCuckoo(kuk, graczI);
    //  board.GoCuckoo(kuku, graczI);
}
stoper.Stop();
long s = stoper.ElapsedMilliseconds / 1000;
Console.WriteLine(s);
k.Add(new double[5] { 400, 50, s, kuk.Moves, graczI.Moves });
winners.Add(boardHandler.Winner);
Console.ReadKey();