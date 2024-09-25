using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer;

class Hero : Entity
{
    private float speed = 100.0f;
    private bool faceRight = false;
    public Hero() : base("characters")
    {
        sprite.TextureRect = new IntRect(0, 0, 24, 24);
        sprite.Origin = new Vector2f(12, 12);
    }

    public override void Update(Scene scene, float deltaTime)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            Position -= new Vector2f(speed * deltaTime, 0);
            faceRight = false;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            scene.TryMove(this, new Vector2f(speed * deltaTime, 0));
            faceRight = true;
        }
    }

    public override void Render(RenderTarget target)
    {
        sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1);
        base.Render(target);
    }
}