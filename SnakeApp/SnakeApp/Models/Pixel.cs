namespace SnakeApp.Models
{
    using System;

    internal readonly struct Pixel
    {
        public Pixel(int x, int y, ConsoleColor color, int pixelSize = 3)
        {
            X = x;
            Y = y;
            Color = color;
            PixelSize = pixelSize;
        }

        public int X { get; }

        public int Y { get; }

        public int PixelSize { get; }

        public ConsoleColor Color { get; }

        public void DrawPixel()
        {
            Console.ForegroundColor = Color;

            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write('█');
                }
            }
        }

        public void ClearPixel()
        {
            for (int x = 0; x < PixelSize; x++)
            {
                for (int y = 0; y < PixelSize; y++)
                {
                    Console.SetCursorPosition(X * PixelSize + x, Y * PixelSize + y);
                    Console.Write(' ');
                }
            }
        }
    }
}
