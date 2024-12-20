using SFML.Window;
using SFML.Graphics;

using Latte.Core;
using Latte.Core.Application;


namespace LatteArt;


public class Canvas : RenderTexture, IUpdateable, IDrawable
{
    public Brush Brush { get; }
    
    public Color Color { get; set; }

    public bool Dragging => !App.IsMouseOverAnyElement() && Mouse.IsButtonPressed(Mouse.Button.Right); 
    public bool Painting { get; set; } 
    
    
    public Canvas(Color color) : base(App.Window.Size.X, App.Window.Size.Y, new ContextSettings
    {
        AntialiasingLevel = 16
    })
    {
        Brush = new(2f, Color.Black);
        Smooth = true;
        
        Color = color;
        
        Clear(Color);
    }


    public void Update()
    {
        Brush.Update();
        
        if (!Painting && !App.IsMouseOverAnyElement() && Mouse.IsButtonPressed(Mouse.Button.Left))
            Painting = true;
        
        if (Painting && !Mouse.IsButtonPressed(Mouse.Button.Left))
            Painting = false;
    }


    public void Draw() => Draw(App.Window);
    
    public void Draw(RenderTarget target)
    {
        SetActive(true);
        
        if (Painting)
            Brush.Draw(this);
        
        Display();
        
        target.Draw(new Sprite(Texture));

        SetActive(false);
    }
}