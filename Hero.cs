using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer;
class Hero : Entity
{
    public const float WALKSPEED = 100.0f;
    public const float JUMPFORCE = 250.0f;
    public const float GRAVITYFORCE = 400.0f;
    private float verticalSpeed;
    private bool isGrounded;
    private bool isUpPressed;
    private float speed = 100.0f;
    private bool faceRight = false;
    public Hero() : base("characters")
    {
        sprite.TextureRect = new IntRect(0, 0, 24, 24);
        sprite.Origin = new Vector2f(12, 12);
    }

    public override void Update(Scene scene, float deltaTime)
    {
        verticalSpeed += GRAVITYFORCE * deltaTime;
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            scene.TryMove(this, new Vector2f(-speed * deltaTime, 0));
            faceRight = false;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            scene.TryMove(this, new Vector2f(speed * deltaTime, 0));
            faceRight = true;
        }
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
        {
            if (isGrounded && !isUpPressed) {
                verticalSpeed = -JUMPFORCE;
                isUpPressed = true;
            }
        }
        isGrounded = false;
        Vector2f velocity = new Vector2f(0, verticalSpeed * deltaTime);
        if (scene.TryMove(this, velocity))
        {
            if (verticalSpeed > 0.0f)
            {
                isGrounded = true; //hero is standing on something
            }
            verticalSpeed = 0.0f;
        }
        else isUpPressed = false;
        
        if (verticalSpeed > 500.0f) verticalSpeed = 500.0f;
        
        Vector2f difference = new Vector2f (Position.X - Program.SCREEN_ORIGIN_X, Position.Y - Program.SCREEN_ORIGIN_Y);
        float absDiffX = MathF.Abs(difference.X);
        float absDiffY = MathF.Abs(difference.Y);
        if (absDiffX > (Program.SCREEN_ORIGIN_X + Position.X) || 
            absDiffY > (Program.SCREEN_ORIGIN_Y + Position.Y)) scene.Reload();
    }

    public override void Render(RenderTarget target)
    {
        sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1);
        base.Render(target);
    }
}