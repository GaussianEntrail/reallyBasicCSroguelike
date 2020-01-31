using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleThing
{
    class Point
    {
        public int x, y;
        public Point(int x, int y) { this.x = x; this.y = y; }
        public void move(int x, int y) { this.x = x; this.y = y; }
        public static bool equals(Point p1, Point p2) { return (p1.x == p2.x && p1.y == p2.y); }
        public static bool equals(Point p, int x, int y) { return (p.x == x && p.y == y); }
        public static bool adjacent(Point p, int x, int y) {return (Math.Abs(p.x - x) == 1 || Math.Abs(p.y - y) == 1);}
        public static Point NULL = new Point(0, 0);
    }

    class Rect
    {
        public int x, y, w, h;
        public Rect(int x, int y, int w, int h) { this.x = x; this.y = y; this.w = w; this.h = h; }
        public bool pointWithinRect(Point p) { return (p.x >= x && p.x < x + w && p.y >= y && p.y < y + h); }
        public bool rectIntersect(Rect other)
        {
            Point l1 = TOPLEFT(), r1 = BOTTOMRIGHT();
            Point l2 = other.TOPLEFT(), r2 = other.BOTTOMRIGHT();
            
            if (l1.x > r2.x || l2.x > r1.x) {return false;}
            if (l1.y < r2.y || l2.y < r1.y) {return false;}

            return true;

        }
        public Point TOPLEFT() { return new Point(x, y); }
        public Point TOPRIGHT() { return new Point(x + w, y); }
        public Point BOTTOMLEFT() { return new Point(x, y + h); }
        public Point BOTTOMRIGHT() { return new Point(x + w, y + h); }
        public Point CENTER() { return new Point( (int)x+(w/2), (int)y+(h/2) ); }
    }

    class Tile
    {
        public ConsoleColor fg;
        public ConsoleColor bg;
        public char c;
        public bool isSolid;

        public Tile(ConsoleColor fg, ConsoleColor bg, char c, bool isSolid)
        {
            this.fg = fg;
            this.bg = bg;
            this.c = c;
            this.isSolid = isSolid;
        }
        public void draw()
        {
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
            Console.Write(c);
            Console.ResetColor();
        }

        public static Tile VOID = new Tile(ConsoleColor.Black, ConsoleColor.Black, ' ', true);
        public static Tile ROCK = new Tile(ConsoleColor.Gray, ConsoleColor.DarkGray, '#', true);
        public static Tile SAND = new Tile(ConsoleColor.DarkYellow, ConsoleColor.Yellow, '.', false);
        public static Tile FLOOR = new Tile(ConsoleColor.White, ConsoleColor.Gray, '.', false);
        public static Tile GRASS = new Tile(ConsoleColor.DarkGreen, ConsoleColor.Green, '\"', false);
        public static Tile TREE = new Tile(ConsoleColor.DarkGreen, ConsoleColor.Green, '♣', true);
        public static Tile WATER = new Tile(ConsoleColor.DarkBlue, ConsoleColor.Blue, '≈', true);
        public static Tile DEEP_WATER = new Tile(ConsoleColor.DarkBlue, ConsoleColor.DarkBlue, '≈', true);
        public static Tile PLAYER = new Tile(ConsoleColor.Cyan, ConsoleColor.DarkBlue, '☻', true);
        public static Tile MONSTER = new Tile(ConsoleColor.Black, ConsoleColor.DarkRed, '₰', true);
        public static Tile COIN = new Tile(ConsoleColor.Yellow, ConsoleColor.DarkYellow, '☼', true);
        public static Tile HEART = new Tile(ConsoleColor.Magenta, ConsoleColor.DarkMagenta, '♥', true);

    }
    class Map
    {
        public int w;
        public int h;
        public Tile[,] map;
        public Random r;
        public Map(int w, int h)
        {
            this.w = w;
            this.h = h;
            r = new Random();

            map = new Tile[h, w];
            mapCreate();
        }
        public Map(int w, int h, Random r)
        {
            this.w = w;
            this.h = h;
            this.r = r;

            map = new Tile[h, w];
            mapCreate();
        }
        public Tile get(int x, int y)
        {
            return (withinBounds(x, y)) ? map[y, x] : Tile.VOID;
        }
        public void set(int x, int y, Tile t)
        {
            if (withinBounds(x, y)) { map[y, x] = t; }
        }
        public void mapCreate()
        {
            //map = MapGeneratorStuff.defaultMapGenerator(w, h, r);
            map = MapGeneratorStuff.testMapGenerator(w, h, r, 10);
        }
        public bool withinBounds(int x, int y)
        {
            return (x >= 0 && x < w && y >= 0 && y < h);
        }
        public bool isSolid(int x, int y)
        {
            return get(x, y).isSolid;
        }
        public Point getRandomFreePosition()
        {
            int x = -1, y = -1;
            while (true)
            {
                x = r.Next() % w;
                y = r.Next() % h;
                if (!isSolid(x, y) && withinBounds(x, y)) { break; }
            }
            return new Point(x, y);
        }
        public void drawTile(int x, int y)
        {
            get(x, y).draw();
        }
    }
    class Camera
    {
        public Point cameraPosition;
        public int w;
        public int h;

        public Camera(int x, int y, int w, int h)
        {
            cameraPosition = new Point(x, y);
            this.w = w;
            this.h = h;
        }
        public void move(int x, int y)
        {
            cameraPosition.move(x, y);
        }
    }
    class Timer
    {
        double timeElapsed;
        public Timer() { timeElapsed = 0; }
        public void tick(double dt = 1.0) { timeElapsed += dt; }
        public double Time() { return timeElapsed; }
    }

}
