using KonsolowaGraCSA;
using KonsolowaGraCSA.Handlers;
using KonsolowaGraCSA.Models;
using System.Diagnostics;

List<double[]> k = new List<double[]>();
List<string> winners = new List<string>();

//for (int i = 0; i < 5; i++)
//{
Random rnd = new Random();
int dlug = 10, wys = 10;
int hashes = 30;
//utworzenie modeli
Board board = new Board(dlug, wys, hashes); //plansza jest uworzona
Player graczI = new Player(dlug - 1, wys - 1, "&", "Ja");
Enemy enemy = new Enemy("Kukulkowy Wrog", "@", 0, 0);

//podpiecie handlerów
BoardHandler boardHandler = new BoardHandler(board, hashes); //podpiete funkcje do planszy
CuckooHandler cuckooHandler = new CuckooHandler(board, enemy, 10, 10, board.Hashes);
MovingHandler movingHandler = new MovingHandler(board, boardHandler, cuckooHandler); //podpiete obie funkcje ruchu

Stopwatch stoper = new Stopwatch();

List<Player> gracze = new List<Player>();
gracze.Add(graczI);

stoper.Start();
while (boardHandler.EndGame == false)
{
    movingHandler.Go(graczI, enemy);
    movingHandler.GoCuckoo(enemy, graczI);
    //  boardTable.GoCuckoo(kuku, graczI);
}
stoper.Stop();
long s = stoper.ElapsedMilliseconds / 1000;
Console.WriteLine(s);
k.Add(new double[5] { 400, 50, s, enemy.Moves, graczI.Moves });
winners.Add(boardHandler.Winner);
Console.ReadKey();