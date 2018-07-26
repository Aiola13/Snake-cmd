using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApp15
{
    class Program
    {
        static void Main(string[] args)
        {
            Game g = new Game();
            g.GameLoop();
        }
    }

    public class Game
    {
        int x = 55;
        int y = 11;
        char c = ' ';
        int choice;
        ConsoleKey currentKey = ConsoleKey.RightArrow;

        public void GameLoop()
        {
            Point p = new Point(x, y);
            Snake s = new Snake(p, c);
            Food f = new Food();
            ConsoleKey temp;

            Console.WriteLine("Choix 1 : Mode Manuel / Choix 2 : Mode Automatique");
            Int32.TryParse(Console.ReadLine(), out choice);

            while (s.CanMove())
            {
                if (choice == 1)
                    temp = Input();
                else
                    temp = IA(f, s);

                if (temp != ConsoleKey.A)
                    currentKey = temp;
                f.DrawFood();
                f.CheckEat(s);
                s.DrawSnake(currentKey);
                Refresh();
            }

            Console.Clear();
            Console.WriteLine("You Lose");
            Console.ReadLine();
        }

        public void Refresh()
        {
            Thread.Sleep(100);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ResetColor();
            Console.Clear();
        }

        public ConsoleKey IA(Food f, Snake s)
        {
            int difX = f.currentFood.x - s.lp[0].x;
            int difY = f.currentFood.y - s.lp[0].y;

            if (difX < 0)
                return ConsoleKey.LeftArrow;
            if (difX > 0)
                return ConsoleKey.RightArrow;
            if (difY > 0)
                return ConsoleKey.DownArrow;
            if (difY < 0)
                return ConsoleKey.UpArrow;

            return ConsoleKey.A;
        }

        public ConsoleKey Input()
        {
            ConsoleKey currentKey = ConsoleKey.A;
            if (Console.KeyAvailable)
            {
                currentKey = Console.ReadKey().Key;
            }

            return currentKey;
        }
    }

    public class Snake
    {
        public List<Point> lp = new List<Point>();

        char c;

        public Snake(Point _p, char _c)
        {
            c = _c;
            lp.Add(_p);
        }

        public void DrawSnake(ConsoleKey currentKey)
        {
            MoveSnake(currentKey);
            foreach (Point i in lp)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.SetCursorPosition(i.x, i.y);
                Console.Write(c);
            }
            Console.CursorVisible = false;
        }

        public void MoveSnake(ConsoleKey currentKey)
        {
            List<Point> LastPos = new List<Point>();

            /* if (Console.KeyAvailable)
             {
                 ConsoleKeyInfo key = Console.ReadKey();
                 if (key.Key.Equals(ConsoleKey.UpArrow))
                     currentKey = "Up";
                 if (key.Key.Equals(ConsoleKey.DownArrow))
                     currentKey = "Down";
                 if (key.Key.Equals(ConsoleKey.LeftArrow))
                     currentKey = "Left";
                 if (key.Key.Equals(ConsoleKey.RightArrow))
                     currentKey = "Right";
             }*/

            foreach (Point i in lp)
                LastPos.Add(new Point(i.x, i.y));

            if (currentKey == ConsoleKey.UpArrow)
                lp[0].y--;
            if (currentKey == ConsoleKey.DownArrow)
                lp[0].y++;
            if (currentKey == ConsoleKey.LeftArrow)
                lp[0].x--;
            if (currentKey == ConsoleKey.RightArrow)
                lp[0].x++;



            for (int j = 1; j < lp.Count; j++)
            {
                lp[j].x = LastPos[j - 1].x;
                lp[j].y = LastPos[j - 1].y;
            }


        }

        public Point GetPosition()
        {
            //Point _p = new Point(Console.CursorTop, Console.CursorLeft);
            Point _p = new Point(lp[0].x, lp[0].y);
            return _p;
        }

        public bool CanMove()
        {
            if (lp[0].x <= -1 || lp[0].x >= Console.WindowWidth + 1)
                return false;

            if (lp[0].y <= -1 || lp[0].y >= Console.WindowHeight + 1)
                return false;

            return true;
        }

        public void GrowBody()
        {
            lp.Add(GetPosition());
        }
    }

    public class Point
    {
        public int x;
        public int y;

        public Point(int _x, int _y)
        {
            y = _y;
            x = _x;
        }
    }

    public class Food
    {
        public int width;
        public int height;
        public Point currentFood;
        public Random r = new Random();
        public char c;

        public Food()
        {
            width = Console.WindowWidth;
            height = Console.WindowHeight;
            currentFood = new Point(r.Next(1, width - 1), r.Next(1, height - 1));
        }


        public void Generate()
        {
            currentFood = new Point(r.Next(0, width), r.Next(1, height));
            DrawFood();

        }

        public void DrawFood()
        {
            Console.BackgroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(currentFood.x, currentFood.y);

            Console.Write(c);

            Console.CursorVisible = false;
        }

        public void CheckEat(Snake s)
        {
            if (s.GetPosition().x == currentFood.x && s.GetPosition().y == currentFood.y)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Generate();
                s.GrowBody();
            }
        }
    }
}