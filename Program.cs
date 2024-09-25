using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer;

class platformer
{

    static void main()
    {
        using (var window = new RenderWindow(new VideoMode(800, 600), "platformer"))
        {
            window.Closed += (o, e) => window.Close();
            
            // TODO: Initialize
            Clock clock = new Clock();
            Scene scene = new Scene();

            scene.Spawn(new Platform {
                Position = new Vector2f(54, 270)});

            for (int i = 0; i < 10; i++)
            {
                scene.Spawn(new Platform {
                    Position = new Vector2f(18 + i * 18, 288)
                });
            }
            
            window.SetView(new View(
                new Vector2f(200, 150),
                new Vector2f(400, 300)
            ));
            
            while (window.IsOpen)
            {
                window.DispatchEvents();
                float deltaTime = clock.Restart().AsSeconds();
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