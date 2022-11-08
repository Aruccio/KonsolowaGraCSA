using KonsolowaGraCSA;
using KonsolowaGraCSA.Handlers;
using KonsolowaGraCSA.Models;
using System.Diagnostics;

Random rnd = new Random();
int dlug = 10, wys = 10;
int hashes = 30;
int iteracje = 10;
int populacja = 10;

//utworzenie modeli
Board board = new Board(dlug, wys, hashes); //plansza jest uworzona
Player player = new Player(dlug - 1, wys - 1, "&", "Ja");
Enemy enemy = new Enemy("Kukulkowy Wrog", "@", 0, 0);

//podpiecie handlerów
BoardHandler boardHandler = new BoardHandler(board, hashes); //podpiete funkcje do planszy
CuckooHandler cuckooHandler = new CuckooHandler(board, enemy, iteracje, populacja, board.Hashes);
MovingHandler movingHandler = new MovingHandler(board, boardHandler, cuckooHandler); //podpiete obie funkcje ruchu

Stopwatch stoper = new Stopwatch();

stoper.Start();
while (boardHandler.EndGame == false)
{
    movingHandler.Go(player, enemy);
    movingHandler.GoCuckoo(enemy, player);
}
stoper.Stop();
long s = stoper.ElapsedMilliseconds / 1000;
Console.WriteLine(s + " sekund zajela cala gra.");
Console.ReadKey();