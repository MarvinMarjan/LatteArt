using SFML.Window;
using SFML.Graphics;

using Latte.Core.Application;


namespace LatteArt;


class Program
{
    static void Main(string[] args)
    {
        App.Init(VideoMode.DesktopMode, "Latte Art", new("../../../../resources/Itim-Regular.ttf"),
            settings: new()
        {
            AntialiasingLevel = 8
        });

        Canvas canvas = new(Color.White);
        CanvasView view = new(App.Window, App.MainView, canvas)
        {
            BoundsLimit = new(-canvas.Size.X / 4f, -canvas.Size.Y / 4f, canvas.Size.X * 1.5f, canvas.Size.Y * 1.5f)
        };

        while (App.Window.IsOpen)
        {
            App.Window.Clear(new(100, 100, 100));
            
            canvas.Update();
            canvas.Draw();
            
            view.Update();
            
            App.Update();
            App.Draw();
            
            App.Window.Display();
        }
    }
}