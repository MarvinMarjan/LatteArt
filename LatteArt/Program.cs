using SFML.Window;
using SFML.Graphics;

using Latte;
using Latte.Application;
using Latte.Elements;
using Latte.Elements.Primitives.Shapes;


namespace LatteArt;


class Program
{
    static void Main(string[] args)
    {
        App.Init(VideoMode.DesktopMode, "Latte Art", new("../../../../resources/Itim-Regular.ttf"),
            settings: new()
        {
            AntialiasingLevel = 4
        });
        
        App.MainWindow.SetVerticalSyncEnabled(false);
        
        DynamicWindowElement window = new("This is a window", new(100, 100), new(300, 380));

        new RectangleElement(window, new(), new(100, 100))
        {
            Color = Color.Red,
            
            Alignment = AlignmentType.VerticalCenter | AlignmentType.Right,
            AlignmentMargin = new(40, 10)
        };
        
        App.Elements.Add(window);
        
        Canvas canvas = new(Color.White);
        CanvasView view = new(App.MainWindow, App.MainView)
        {
            BoundsLimit = new(-canvas.Size.X / 4f, -canvas.Size.Y / 4f, canvas.Size.X * 1.5f, canvas.Size.Y * 1.5f)
        };

        while (App.MainWindow.IsOpen)
        {
            App.MainWindow.Clear();
            
            canvas.Update();
            canvas.Draw();
            
            view.Update();
            
            App.Update();
            App.Draw();
            
            App.MainWindow.Display();
        }
    }
}