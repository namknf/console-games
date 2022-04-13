namespace SnakeApp
{
    using System;
    using static System.Console;
    using SnakeApp.Models;

    internal class Program
    {
        private const int Width = 40;
        private const int Height = 30;
        private const int ScreenWidth = Width * 3;
        private const int ScreenHeight = Height * 3;

        public const ConsoleColor BorderColor = ConsoleColor.Gray;

        private static void Main(string[] args)
        {
            SetWindowSize(ScreenWidth, ScreenHeight);
            SetBufferSize(ScreenWidth, ScreenHeight);

            CursorVisible = false;

            DrawBorder();

            ReadKey();
        }

        public static void DrawBorder()
        {
            for (int x = 0; x < Width; x++)
            {
                new Pixel(x, 0, BorderColor).Draw();
                new Pixel(x, Height - 1, BorderColor).Draw();
            }

            for (int y = 0; y < Height; y++)
            {
                new Pixel(0, y, BorderColor).Draw();
                new Pixel(Width - 1, y, BorderColor).Draw();
            }
        }
    }
}
