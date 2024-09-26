using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer;

class Program
{

    static void Main()
    {
        using (var window = new RenderWindow(new VideoMode(800, 600), "platformer"))
        {
            window.Closed += (o, e) => window.Close();
            
            // TODO: Initialize
            Clock clock = new Clock();
            Scene scene = new Scene();
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
                // TODO: Updates
                scene.UpdateAll(deltaTime);
                
                window.Clear();
                // TODO: Drawing
                scene.RenderAll(window);
                window.Display();
            }
        }
    }
}