using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static List<Point> snake;
    private static Point food;
    private static Direction direction;
    private static int score;
    private static Random random;
    private static int width = 20;
    private static int height = 20;

    private enum Direction
    {
        Right,
        Down,
        Left,
        Up
    }

    struct Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
    }

    static void Main(string[] args)
    {
        Console.WindowHeight = height + 2;
        Console.WindowWidth = width + 2;

        InitializeGame();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                        if (direction != Direction.Down)
                            direction = Direction.Up;
                        break;
                    case ConsoleKey.A:
                        if (direction != Direction.Right)
                            direction = Direction.Left;
                        break;
                    case ConsoleKey.S:
                        if (direction != Direction.Up)
                            direction = Direction.Down;
                        break;
                    case ConsoleKey.D:
                        if (direction != Direction.Left)
                            direction = Direction.Right;
                        break;
                }
            }

            MoveSnake();

            if (snake[0] == food)
            {
                score++;
                GenerateFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            DrawGame();

            if (CheckCollision())
            {
                Console.Clear();
                Console.WriteLine("Игра окончена! Ваш счет: " + score);
                InitializeGame();
            }

            Thread.Sleep(200);
        }
    }

    private static void InitializeGame()
    {
        snake = new List<Point> { new Point(10, 10) };
        direction = Direction.Right;
        score = 0;
        random = new Random();
        GenerateFood();
    }

    private static void MoveSnake()
    {
        var head = snake[0];

        switch (direction)
        {
            case Direction.Right:
                head.X++;
                break;
            case Direction.Left:
                head.X--;
                break;
            case Direction.Up:
                head.Y--;
                break;
            case Direction.Down:
                head.Y++;
                break;
        }

        snake.Insert(0, head);
    }

    private static void DrawGame()
    {
        Console.Clear();

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var point = new Point(j, i);

                if (snake.Contains(point))
                {
                    Console.ForegroundColor = ConsoleColor.Green; 
                    Console.Write("O");
                    Console.ResetColor();
                }
                else if (food == point)
                {
                    Console.ForegroundColor = ConsoleColor.Red; 
                    Console.Write("X");
                    Console.ResetColor(); 
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }

        Console.WriteLine("Score: " + score);
    }

    private static void GenerateFood()
    {
        food = new Point(random.Next(0, width), random.Next(0, height));
    }

    private static bool CheckCollision()
    {
        var head = snake[0];

        if (head.X < 0 || head.X >= width || head.Y < 0 || head.Y >= height)
            return true;

        for (int i = 1; i < snake.Count; i++)
        {
            if (head == snake[i])
                return true;
        }

        return false;
    }
}