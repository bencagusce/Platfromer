
using SFML.Graphics;
using SFML.System;

namespace Platformer;

class Door : Entity
{
    public Door() : base("tileset")
    {
        sprite.TextureRect = new IntRect(180, 103, 18, 23);
        sprite.Origin = new Vector2f(9, 11.5f);
    }
}