namespace SnakeApp
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using static System.Console;
    using SnakeApp.Models;
    using System.Diagnostics;
    using System.Linq;

    internal class Program
    {
        private const int MapWidth = 30;
        private const int MapHeight = 20;
        private const int ScreenWidth = MapWidth * 3;
        private const int ScreenHeight = MapHeight * 3;
        private const int FrameMilliseconds = 200;

        private const ConsoleColor BorderColor = ConsoleColor.White;
        private const ConsoleColor FoodColor = ConsoleColor.DarkYellow;
        private const ConsoleColor BodyColor = ConsoleColor.Magenta;
        private const ConsoleColor HeadColor = ConsoleColor.DarkBlue;

        private static readonly Random Random = new ();

        static void Main()
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);
            CursorVisible = false;

            while (true)
            {
                StartGame();
                Thread.Sleep(2000);
                ReadKey();
            }
        }

        static void StartGame()
        {
            int score = 0;

            Clear();
            DrawBorder();

            Snake snake = new (10, 5, HeadColor, BodyColor);

            Pixel food = EatFood(snake);
            food.DrawPixel();

            Direction currentMovement = Direction.Right;

            int lagMs = 0;
            var sw = new Stopwatch();

            while (true)
            {
                sw.Restart();
                Direction oldMovement = currentMovement;

                while (sw.ElapsedMilliseconds <= FrameMilliseconds - lagMs)
                {
                    if (currentMovement == oldMovement)
                    {
                        currentMovement = ReadMovement(currentMovement);
                    }
                }

                sw.Restart();

                if (snake.Head.X == food.X && snake.Head.Y == food.Y)
                {
                    snake.Move(currentMovement, true);
                    food = EatFood(snake);
                    food.DrawPixel();

                    score++;

                    Task.Run(() => Beep(1200, 200));
                }
                else
                {
                    snake.Move(currentMovement);
                }

                if (snake.Head.X == MapWidth - 1
                    || snake.Head.X == 0
                    || snake.Head.Y == MapHeight - 1
                    || snake.Head.Y == 0
                    || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
                {
                    break;
                }

                lagMs = (int)sw.ElapsedMilliseconds;
            }

            snake.ClearSnake();
            food.ClearPixel();

            SetCursorPosition(ScreenWidth / 3, ScreenHeight / 2);
            WriteLine($"Game over! Your score: {score}");

            Task.Run(() => Beep(200, 600));
        }

        static void DrawBorder()
        {
            for (int i = 0; i < MapWidth; i++)
            {
                new Pixel(i, 0, BorderColor).DrawPixel();
                new Pixel(i, MapHeight - 1, BorderColor).DrawPixel();
            }

            for (int i = 0; i < MapHeight; i++)
            {
                new Pixel(0, i, BorderColor).DrawPixel();
                new Pixel(MapWidth - 1, i, BorderColor).DrawPixel();
            }
        }

        static Pixel EatFood(Snake snake)
        {
            Pixel food;

            do
            {
                food = new Pixel(Random.Next(1, MapWidth - 2), Random.Next(1, MapHeight - 2), FoodColor);
            } 
            while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
                     snake.Body.Any(b => b.X == food.X && b.Y == food.Y));

            return food;
        }

        static Direction ReadMovement(Direction currentDirection)
        {
            if (!KeyAvailable)
            {
                return currentDirection;
            }

            ConsoleKey key = ReadKey(true).Key;

            currentDirection = key switch
            {
                ConsoleKey.UpArrow when currentDirection != Direction.Down => Direction.Up,
                ConsoleKey.DownArrow when currentDirection != Direction.Up => Direction.Down,
                ConsoleKey.LeftArrow when currentDirection != Direction.Right => Direction.Left,
                ConsoleKey.RightArrow when currentDirection != Direction.Left => Direction.Right,
                _ => currentDirection,
            };

            return currentDirection;
        }
    }
}
