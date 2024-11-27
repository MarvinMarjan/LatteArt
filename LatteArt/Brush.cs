using SFML.Graphics;

using Latte.Core;
using Latte.Core.Application;
using Latte.Core.Type;


namespace LatteArt;


public class Brush(float size, Color color) : IUpdateable, IDrawable
{
    public Vec2f Position { get; private set; } = new();
    public Vec2f LastPosition { get; private set; } = new();

    public float Size { get; set; } = size;

    public Color PaintColor { get; set; } = color;  
    

    public void Update()
    {
        LastPosition = Position;
        Position = App.Window.WorldMousePosition;
    }


    public void Draw(RenderTarget target)
    {
        float length = CalculateLineLength(LastPosition, Position);
        float angle = CalculateLineAngle(LastPosition, Position);

        RectangleShape line = new(new Vec2f(length, Size));
        line.Origin = new(0, Size / 2);
        line.Rotation = angle;
        line.Position = LastPosition;
        line.FillColor = PaintColor;
        
        target.Draw(line);
    }


    private static float CalculateLineLength(Vec2f start, Vec2f end)
        => MathF.Sqrt(MathF.Pow(end.X - start.X, 2f) + MathF.Pow(end.Y - start.Y, 2f));
    
    private static float CalculateLineAngle(Vec2f start, Vec2f end)
        => MathF.Atan2(end.Y - start.Y, end.X - start.X) * 180f / MathF.PI;
}