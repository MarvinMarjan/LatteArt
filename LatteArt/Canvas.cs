using SFML.Window;
using SFML.Graphics;

using Latte;
using Latte.Application;


namespace LatteArt;


public class Canvas : RenderTexture, IUpdateable, IDrawable
{
    public Brush Brush { get; }
    
    public Color Color { get; set; }
    
    
    public Canvas(Color color) : base(App.MainWindow.Size.X, App.MainWindow.Size.Y, new ContextSettings
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
    }


    public void Draw() => Draw(App.MainWindow);
    
    public void Draw(RenderTarget target)
    {
        SetActive(true);
        
        if (Brush.ShouldDraw)
            Brush.Draw(this);
        
        Display();
        
        target.Draw(new Sprite(Texture));

        SetActive(false);
    }
}