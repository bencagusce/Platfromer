using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer;

class Program
{
    public const int SCREEN_W = 800;
    public const int SCREEN_H = 600;
    public const int SCREEN_ORIGIN_X = SCREEN_W / 2;
    public const int SCREEN_ORIGIN_Y = SCREEN_H / 2;

    static void Main()
    {
        using (var window = new RenderWindow(new VideoMode(800, 600), "platformer"))
        {
            window.Closed += (o, e) => window.Close();
            
            Clock clock = new Clock();
            Scene scene = new Scene();
            scene.Spawn(new Background());
            scene.Load("level0");
            
            window.SetView(new View(
                new Vector2f(200, 150),
                new Vector2f(400, 300)
            ));
            
            while (window.IsOpen)
            {
                window.DispatchEvents();
                float deltaTime = clock.Restart().AsSeconds();
                if (deltaTime > 0.1f) deltaTime = 0.1f;
                scene.UpdateAll(deltaTime);
                
                window.Clear();
                scene.RenderAll(window);
                window.Display();
            }
        }
    }
}