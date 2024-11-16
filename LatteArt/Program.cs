using SFML.Window;
using SFML.Graphics;

using Latte.Application;


namespace LatteArt;


class Program
{
    static void Main(string[] args)
    {
        App.MainWindow = new(VideoMode.DesktopMode, "Latte Art", settings: new()
        {
            AntialiasingLevel = 4
        });
        App.MainWindow.SetVerticalSyncEnabled(false);

        Canvas canvas = new(Color.White);
        CanvasView view = new(App.MainWindow)
        {
            BoundsLimit = new(-canvas.Size.X / 4f, -canvas.Size.Y / 4f, canvas.Size.X * 1.5f, canvas.Size.Y * 1.5f)
        };

        while (App.MainWindow.IsOpen)
        {
            App.MainWindow.Clear();
            
            App.Update();
            App.Draw();
            
            canvas.Update();
            canvas.Draw();
            
            view.Update();
            
            App.MainWindow.Display();
        }
    }
}