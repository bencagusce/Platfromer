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
            while (window.IsOpen)
            {
                window.DispatchEvents();
                float deltaTime = clock.Restart().AsSeconds();
                // TODO: Updates
                window.Clear();
                // TODO: Drawing
                window.Display();
            }
        }
    }
}