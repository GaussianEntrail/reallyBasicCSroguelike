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

        public static Tile[,] testMapGenerator(int w, int h, Random r, int maxRooms)
        {
            Tile[,] map = new Tile[h, w];


            int x, y;
            for (y = 0; y < h; y++)
            {
                for (x = 0; x < w; x++)
                {
                    map[y, x] = Tile.VOID;
                }
            }

            int minRoomWidth = 3, maxRoomWidth = w / 3;
            int minRoomHeight = 3, maxRoomHeight = h / 3;
            List<Rect> rooms = new List<Rect>();
            while (rooms.Count < maxRooms) {
                int r_w = randomRange(r, minRoomWidth, maxRoomWidth);
                int r_h = randomRange(r, minRoomHeight, maxRoomHeight);
                int r_x = randomRange(r, 1, (w - 1) - r_w);
                int r_y = randomRange(r, 1, (h - 1) - r_h);
                Rect room_add = new Rect(r_x, r_y, r_w, r_h);

                bool roomIntersect = false;
                if (rooms.Count > 0)
                {
                    roomIntersect = (rooms.Find(check => check.rectIntersect(room_add)) != null);
                }
                if (!roomIntersect) { rooms.Add(room_add); }
            }

            List<Rect> halls = new List<Rect>();
            for (int n = 0; n < rooms.Count; n++)
            {
                int next_n = (n + 1) % rooms.Count;
                int start_x = rooms[n].CENTER().x;
                int start_y = rooms[n].CENTER().y;
                int end_x = rooms[next_n].CENTER().x;
                int end_y = rooms[next_n].CENTER().y;

                int hall_start_x = Math.Min(start_x, end_x);
                int hall_end_x = Math.Max(start_x, end_x);

                int hall_start_y = Math.Min(start_y, end_y);
                int hall_end_y = Math.Max(start_y, end_y);

                int hall_w = hall_end_x - hall_start_x;
                int hall_h = hall_end_y - hall_start_y;

                if (r.NextDouble() > 0.5)
                {
                    halls.Add(new Rect(hall_start_x,hall_start_y,hall_w,1)); //horizontal
                    halls.Add(new Rect(hall_end_x, hall_start_y, 1, hall_h)); //vertical
                }
                else
                {
                    halls.Add(new Rect(hall_start_x, hall_start_y, 1, hall_h)); //vertical
                    halls.Add(new Rect(hall_start_x, hall_end_y, hall_w, 1)); //horizonal
                }
            }

            foreach (Rect room in rooms)
            {
                int rx, ry;
                for (ry = room.y; ry < room.y + room.h; ry++)
                {
                    for (rx = room.x; rx < room.x + room.w; rx++)
                    {
                        map[ry, rx] = Tile.FLOOR;
                    }
                }
            }
            foreach (Rect hall in halls)
            {
                int hx, hy;
                for (hy = hall.y; hy < hall.y + hall.h; hy++)
                {
                    for (hx = hall.x; hx < hall.x + hall.w; hx++)
                    {
                        map[hy, hx] = Tile.FLOOR;
                    }
                }
            }

            int i, j;
            bool adjFloorTile = false;
            for (y = 0; y < h; y++)
            {
                for (x = 0; x < w; x++)
                {
                    adjFloorTile = false;
                    if (map[y,x] == Tile.VOID)
                    {
                        for (i = -1; i < 2; i ++)
                        {
                            for (j = -1; j < 2; j++)
                            {
                                if (x + i > 0 && x + i < w && y + j > 0 && y + j < h)
                                {
                                    if (map[y + j, x + i] == Tile.FLOOR) { adjFloorTile = true; break;}
                                }
                            }
                        }
                        if (adjFloorTile) { map[y, x] = Tile.ROCK; }
                    }
                }
            }

            return map;
        }

        public static int randomRange(Random r, int min, int max)
        {
            int range = max - min;
            return (min + r.Next(range));
        }
    }
}
