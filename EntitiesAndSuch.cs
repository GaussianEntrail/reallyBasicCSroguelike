using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleThing
{
    public enum itemPickupType
    {
        coin,
        heart
    }
    class ItemPickup
    {
        Point p;
        Tile t;
        itemPickupType i;
        public bool collect;
        int pointValue;
        public ItemPickup(int x, int y, Tile t, itemPickupType i, int pointValue = 1)
        {
            p = new Point(x, y);
            this.t = t;
            this.i = i;
            this.pointValue = pointValue;
            collect = false;
        }
        public ItemPickup(Point p, Tile t, itemPickupType i, int pointValue = 1)
        {
            this.p = p;
            this.t = t;
            this.i = i;
            this.pointValue = pointValue;
            collect = false;
        }
        public void draw() { t.draw(); }
        public Point getPosition() { return p; }
        public bool isAt(int x, int y) { return (x == p.x && y == p.y); }
        public itemPickupType getItemType() { return i; }
        public int getValue() { return pointValue; }
    }
    class Critter
    {
        Point p;
        Point p_prev;
        Tile t;
        int actiontimer;
        int life, attack, defend, speed;
        string name = "DEFAULTNAME";

        public string NAME() { return name; }
        public int ATK() { return attack; }
        public int DEF() { return defend; }
        public int AGI() { return speed; }
        public void LifeUp(int i) { life += i; }
        public bool isDead() { return life <= 0; }
    }
    class Player : Critter
    {
        Point p;
        Point p_prev;
        Tile t = Tile.PLAYER;
        int actiontimer;
        int life, attack, defend, speed;
        string name;
        int money;
        public Player(int x, int y)
        {
            p = new Point(x, y);
            p_prev = p;
            CharInit();
        }
        public Player(Point p)
        {
            this.p = p;
            p_prev = p;
            CharInit();
        }
        public void CharInit()
        {
            life = 4;
            attack = 10;
            defend = 10;
            speed = 10;
            money = 0;
            name = "Explorer";
        }
        public void draw() { t.draw(); }
        public Point getPosition() { return p; }
        public bool isAt(int x, int y) { return (x == p.x && y == p.y); }
        public bool wasAt(int x, int y) { return (x == p_prev.x && y == p_prev.y); }
        public void move(int x, int y) { p_prev = p; p.move(x, y); }
        public void doTick() { actiontimer += speed; }
        public void doAction() { actiontimer -= 100; }
        public bool isReady() { return (actiontimer >= 100); }

        public string NAME() { return name; }
        public int ATK() { return attack; }
        public int DEF() { return defend; }
        public int AGI() { return speed; }
        public void LifeUp(int i) { life += i; }
        public bool isDead() { return life <= 0; }
        public void AddMoney(int i) { money += i; }
        public void statsWrite()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("X: {0}, Y: {1} ", p.x, p.y);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("♥ LIFE: {0}",life);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("♠ ATK: {0}",attack); 
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("♣ DEF: {0}", defend); 
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("♦ AGI: {0}", speed);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("₹ MONEY: {0}", money);
            Console.WriteLine();
        }
    }
    class Monster : Critter
    {
        Point p;
        Point p_prev;
        Tile t = Tile.MONSTER;
        int actiontimer;
        int life, attack, defend, speed;
        string name;
        public Monster(int x, int y)
        {
            p = new Point(x, y);
            p_prev = p;
            CharInit();
        }
        public Monster(Point p)
        {
            this.p = p;
            p_prev = p;
            CharInit();
        }
        public void CharInit()
        {
            life = 3;
            attack = 8;
            defend = 8;
            speed = 7;
            name = "Beast";
        }
        public void draw() { t.draw(); }
        public Point getPosition() { return p; }
        public bool isAt(int x, int y) { return (x == p.x && y == p.y); }
        public bool wasAt(int x, int y) { return (x == p_prev.x && y == p_prev.y); }
        public void move(int x, int y) { p_prev = p; p.move(x, y); }
        public void doTick() { actiontimer += speed; }
        public void doAction() { actiontimer -= 100; }
        public bool isReady() { return (actiontimer >= 100); }

        public string NAME() { return name; }
        public int ATK() { return attack; }
        public int DEF() { return defend; }
        public int AGI() { return speed; }
        public void LifeUp(int i) { life += i; }
        public bool isDead() { return life <= 0; }
    }
}
