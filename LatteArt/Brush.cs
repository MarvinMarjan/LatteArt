using SFML.System;
using SFML.Window;
using SFML.Graphics;

using Latte;
using Latte.Application;


namespace LatteArt;


public class Brush(float size, Color color) : IUpdateable, IDrawable
{
    public Vector2f Position { get; private set;  }
    public Vector2f LastPosition { get; private set; }

    public float Size { get; set; } = size;

    public Color PaintColor { get; set; } = color;

    public static bool ShouldDraw => Mouse.IsButtonPressed(Mouse.Button.Left);


    public void Update()
    {
        LastPosition = Position;
        Position = App.MainWindow!.WorldMousePosition;
    }


    public void Draw(RenderTarget target)
    {
        float length = CalculateLineLength(LastPosition, Position);
        float angle = CalculateLineAngle(LastPosition, Position);
        
        RectangleShape line = new(new Vector2f(length, Size))
        {
            Origin = new(0, Size / 2),
            Rotation = angle,
            FillColor = PaintColor
        };
        line.Position = LastPosition;
        
        target.Draw(line);
    }


    private static float CalculateLineLength(Vector2f start, Vector2f end)
        => (float)Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
    
    private static float CalculateLineAngle(Vector2f start, Vector2f end)
        => (float)(Math.Atan2(end.Y - start.Y, end.X - start.X) * 180.0 / Math.PI);
}