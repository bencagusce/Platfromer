using SFML.Graphics;
using SFML.System;

namespace Platformer;

class Background : Entity
{
    public Background() : base("background")
    {
        sprite.TextureRect = new IntRect(0, 0, 24, 24);
        sprite.Origin = new Vector2f(12, 12);
    }
}