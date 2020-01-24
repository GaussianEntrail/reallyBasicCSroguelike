using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleThing
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            //HELP SCREEN
            Console.WriteLine("If you're seeing this, you're playing a little roguelike demo I made when bored at work");
            Console.Write("Numpad or arrow keys move your dude around, he looks like this: ");
            Tile.PLAYER.draw();
            Console.WriteLine();
            Console.Write("Collect money: ");
            Tile.COIN.draw();
            Console.WriteLine();
            Console.Write("Watch out for enemies, they can kill you: ");
            Tile.MONSTER.draw();
            Console.WriteLine();
            Console.WriteLine("Press the Escape key to end the game");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Press any key to start the game");
            Console.ReadKey();

            Dungeon d = new Dungeon();
            while (d.isRunning)
            {
                d.gameStep();
            }
            Console.Clear();
            Console.ResetColor();
            Console.WriteLine("See you later");
            Console.ReadKey();
        }
    }
}
