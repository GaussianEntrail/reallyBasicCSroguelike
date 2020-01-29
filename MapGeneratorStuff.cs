using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleThing
{
    class MapGeneratorStuff
    {
        public static Tile[,] defaultMapGenerator(int w, int h, Random r)
        {
            Tile[,] map = new Tile[h,w];
            int i, j;
            double t;
            for (j = 0; j < h; j++)
            {
                for (i = 0; i < w; i++)
                {
                    t = r.NextDouble();
                    if (t < 0.5) { map[j, i] = Tile.GRASS; }
                    if (t >= 0.5 && t < 0.7) { map[j, i] = Tile.TREE; }
                    if (t >= 0.7 && t < 0.85) { map[j, i] = Tile.SAND; }
                    if (t >= 0.85 && t < 0.99) { map[j, i] = Tile.WATER; }
                    if (t >= 0.99) { map[j, i] = Tile.ROCK; }
                }
            }
            return map;
        }
    }
}
