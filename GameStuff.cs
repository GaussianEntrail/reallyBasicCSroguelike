using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleThing
{
    class Dungeon
    {
        List<string> messageLog;
        List<ItemPickup> items;
        List<Monster> monsters;
        Map M;
        Player P;
        Camera C;
        Random R;
        Timer T;
        ConsoleKeyInfo commandKey;
        public bool isRunning;
        public Dungeon()
        {
            R = new Random();
            M = new Map(80, 48, R);
            C = new Camera(0,0,64,12);
            P = new Player(0,0);
            T = new Timer();
            Point startingPoint = RandomFreePosition();
            P.move(startingPoint.x, startingPoint.y);
            C.move(startingPoint.x - C.w / 2, startingPoint.y - C.h / 2);
            messageLog = new List<string>();
            items = new List<ItemPickup>();
            monsters = new List<Monster>();
            while (items.Count < 32)
            {
                Point p = RandomFreePosition();
                CreateItem(p, itemPickupType.coin);
            }
            while (monsters.Count < 5)
            {
                Point p = RandomFreePosition();
                CreateMonster(p);
            }
            messageLog.Add("GAME START");
            isRunning = true;
        }
        public void draw()
        {
            int x_min = C.cameraPosition.x;
            int y_min = C.cameraPosition.y;
            int x_max = x_min + C.w;
            int y_max = y_min + C.h;
            int x, y;
            //bool drawMapTile = true;
            Console.Clear();
            Console.WriteLine("TIMER: {0}",T.Time());
            for (y = y_min; y < y_max; y++)
            {
                for (x = x_min; x < x_max; x++)
                {
                    //drawMapTile = true;
                    if (P.isAt(x, y))
                    {
                        P.draw();
                    }
                    else if (monsterAtPosition(x, y) != null)
                    {
                        monsterAtPosition(x, y).draw();
                    }
                    else if (itemAtPosition(x, y) != null)
                    {
                        itemAtPosition(x, y).draw();
                    }
                    else { M.drawTile(x, y); }
                }
                Console.WriteLine();
            }
            P.statsWrite();
            Console.ResetColor();

            //write message log
            Console.WriteLine(messageLog.Last());
        }
        public Point RandomFreePosition()
        {
            Point p = M.getRandomFreePosition();
            bool intersectsItem = false;
            bool intersectsPlayer = false;
            while (true)
            {
                try
                {
                    intersectsItem = (items.FirstOrDefault(x => Point.equals(x.getPosition(), p)) != null);
                    intersectsPlayer = Point.equals(P.getPosition(), p);
                }
                catch { }
                /*
                if (items!= null)
                {intersectsItem = (items.FirstOrDefault(x => Point.equals(x.getPosition(), p)) != null);}
                if (P != null)
                {
                    if (P.getPosition() != null) { intersectsPlayer = Point.equals(P.getPosition(), p); }
                }*/
                if (!intersectsPlayer && !intersectsItem) { break; }
                else { p = M.getRandomFreePosition(); }
            }
            return p;
        }
        public void CreateItem(Point p, itemPickupType i = itemPickupType.coin)
        {
            items.Add(new ItemPickup(p, Tile.COIN, i));
        }
        public void CreateMonster(Point p)
        {
            monsters.Add(new Monster(p));
        }
        public void removeDeadMonsters()
        {
            monsters.RemoveAll(x => x.isDead());
        }
        public void gameStep()
        {
            T.tick();
            collectItems();
            removeDeadMonsters();
            while (!P.isReady())
            {
                P.doTick();
                foreach (Monster mon in monsters)
                {
                    mon.doTick();
                    if (mon.isReady()) {MonsterPathFinding(mon);}
                }
            }
            if (P.isDead()) { isRunning = false; }
            draw();
            commandKey = Console.ReadKey();
            command(commandKey);
        }
        private void command(ConsoleKeyInfo c)
        {
            switch (c.Key)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.NumPad8:
                    movePlayer(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.NumPad2:
                    movePlayer(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.NumPad4:
                    movePlayer(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                case ConsoleKey.NumPad6:
                    movePlayer(1, 0);
                    break;
                case ConsoleKey.NumPad9:
                    movePlayer(1, -1);
                    break;
                case ConsoleKey.NumPad3:
                    movePlayer(1, 1);
                    break;
                case ConsoleKey.NumPad7:
                    movePlayer(- 1, -1);
                    break;
                case ConsoleKey.NumPad1:
                    movePlayer(- 1, 1);
                    break;
                case ConsoleKey.Escape:
                    isRunning = false;
                    break;
            };
        }
        public void movePlayer(int dx, int dy)
        {
            if (dx == 0 && dy == 0) { return; }
            //CHECK NEW POSITION
            int new_x = P.getPosition().x + dx, new_y = P.getPosition().y + dy;
            if (isCollision(new_x, new_y)) { return; }
            else
            {
                Monster targets = monsterAtPosition(new_x, new_y);
                if (targets != null) { playerAttack(targets); P.doAction(); return; }
                else
                {
                    //MOVE TO NEW POSITION
                    P.move(new_x, new_y);
                    P.doAction();
                    //MOVE THE CAMERA WITH THE PLAYER
                    C.move(P.getPosition().x - C.w / 2, P.getPosition().y - C.h / 2);
                    messageLog.Add("Explorer moves");
                    return;
                }
            }
        }
        public void moveMonster(Monster mon, int dx, int dy)
        {
            if (dx == 0 && dy == 0) { return; }
            //CHECK NEW POSITION
            int new_x = mon.getPosition().x + dx, new_y = mon.getPosition().y + dy;
            if (P.isAt(new_x, new_y)) { monsterAttack(mon); mon.doAction(); return; }
            else if ((monsterAtPosition(new_x, new_y) != null) || isCollision(new_x, new_y, false))
            { return; }
            else
            { mon.move(new_x, new_y); mon.doAction(); return; }
        }
        public bool isCollision(int x, int y, bool isPlayer = true)
        {
            //put something in here so monsters don't collide with other monsters?
            bool mapCollision = false;
            try {mapCollision = M.isSolid(x, y);} catch { }
            return mapCollision;
        }
        private void collectItems()
        {
            foreach (ItemPickup i in items)
            {
                if (!i.collect)
                {
                    if (Point.equals(i.getPosition(), P.getPosition()))
                    {
                        if (i.getItemType() == itemPickupType.coin) { P.AddMoney(i.getValue()); }
                        if (i.getItemType() == itemPickupType.heart) { P.LifeUp(i.getValue()); }
                        i.collect = true;
                    }
                }
            }
            items.RemoveAll(x => x.collect);
        }
        private void playerAttack(Monster Defender)
        {
            string msg = "test";
            int attackerPower = (int)(P.ATK() * R.NextDouble());
            int defenderPower = (int)(Defender.DEF() * R.NextDouble());
            int attackerSpeed = (int)(P.AGI() * R.NextDouble());
            int defenderSpeed = (int)(Defender.AGI() * R.NextDouble());

            if (attackerSpeed >= defenderSpeed)
            {
                if (attackerPower >= defenderPower)
                {
                    Defender.LifeUp(-1);
                    if (!Defender.isDead())
                    {
                        msg = P.NAME() + " attacked " + Defender.NAME() + "!";
                    }
                    else
                    {
                        msg = P.NAME() + " killed " + Defender.NAME() + "!";
                    }
                }
                else
                {
                    msg = Defender.NAME() + " blocked " + P.NAME() + "'s attack!"; 
                }
            }
            else { msg = Defender.NAME() + " dodged " + P.NAME() + "'s attack!"; }

            messageLog.Add(msg);
        }
        private void monsterAttack(Monster Attacker)
        {
            string msg = "test";
            int attackerPower = (int)(Attacker.ATK() * R.NextDouble());
            int defenderPower = (int)(P.DEF() * R.NextDouble());
            int attackerSpeed = (int)(Attacker.AGI() * R.NextDouble());
            int defenderSpeed = (int)(P.AGI() * R.NextDouble());

            if (attackerSpeed >= defenderSpeed)
            {
                if (attackerPower >= defenderPower)
                {
                    P.LifeUp(-1);
                    if (!P.isDead())
                    {
                        msg = Attacker.NAME() + " attacked " + P.NAME() + "!";
                    }
                    else
                    {
                        msg = Attacker.NAME() + " killed " + P.NAME() + "!";
                    }
                }
                else
                {
                    msg = P.NAME() + " blocked " + Attacker.NAME() + "'s attack!";
                }
            }
            else { msg = P.NAME() + " dodged " + Attacker.NAME() + "'s attack!"; }

            messageLog.Add(msg);
        }
        public Monster monsterAtPosition(int x, int y)
        {
            Monster m = null;
            try
            {
                m = monsters.FirstOrDefault(d => d.isAt(x, y) && !d.isDead());
            }
            catch { }
            return m;
        }
        public ItemPickup itemAtPosition(int x, int y)
        {
            ItemPickup i = null;
            try
            {
                i = items.FirstOrDefault(d => d.isAt(x, y) && !d.collect);
            }
            catch { }
            return i;
        }
        public List<Point> getAdjacentFreeSpaces(int x, int y, bool isPlayer = false)
        {
            List<Point> freePositions = new List<Point>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i==0 && j==0) {} 
                    else if (!isCollision(x + i, y + j)) { freePositions.Add(new Point(x + i, y + j)); }
                }
            }
            return freePositions;
        }
        private int squareDistance(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            return (dx * dx) + (dy * dy);
        }
        private void MonsterPathFinding (Monster mon)
        {
            Point currentPos = mon.getPosition();
            Point playerPos = P.getPosition();

            int currentDistance = squareDistance(currentPos.x, currentPos.y, playerPos.x, playerPos.y);

            int dx = R.Next(3) - 1;
            int dy = R.Next(3) - 1;

            
            List<Point> listPoints = getAdjacentFreeSpaces(currentPos.x, currentPos.y).FindAll(i => !mon.wasAt(i.x, i.y) && (squareDistance(i.x, i.y, playerPos.x, playerPos.y) <= currentDistance) );
            if (listPoints.Count() > 0) {
                int choice = R.Next(listPoints.Count());
                dx = listPoints[choice].x - currentPos.x;
                dy = listPoints[choice].y - currentPos.y;
            }
            moveMonster(mon, dx, dy);
        }
    }
}
