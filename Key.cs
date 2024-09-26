using SFML.Graphics;
using SFML.System;

namespace Platformer;

class Key : Entity
{
    public Key() : base("tileset")
    {
        sprite.TextureRect = new IntRect(126, 18, 18, 18);
        sprite.Origin = new Vector2f(9, 9);
    }

    public override void Update(Scene scene, float deltaTime)
    {
        if (scene.FindByType<Hero>(out Hero hero))
        {
            if (Collision.RectangleRectangle(Bounds, hero.Bounds, out _))
            {
                Dead = true;
                if (scene.FindByType<Door>(out Door door))
                {
                    door.Unlock();
                }
            }
        }
    }
    public override FloatRect Bounds
    { get 
        {
            var bounds = base.Bounds;
            bounds.Left += 0;
            bounds.Width += 0;
            bounds.Top += 1;
            bounds.Height -= 3;
            return bounds;
        }
    }
}