using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Platformer;
class Hero : Entity
{
    public const float WALKSPEED = 100.0f;
    public const float JUMPFORCE = 250.0f;
    public const float GRAVITYFORCE = 400.0f;
    private float animationBuffer = 0;
    private const float KEYFRAME_THRESHOLD = 0.25f;
    private bool keyframe = false;
    private float verticalSpeed;
    private bool isGrounded;
    private bool landed;
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
        animationBuffer += deltaTime;
        if ((animationBuffer > KEYFRAME_THRESHOLD) && 
            (isGrounded) &&
            (
                (Keyboard.IsKeyPressed(Keyboard.Key.Left)) || 
                (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            )) 
        {
            animationBuffer = 0;
            keyframe = !keyframe;
            if (keyframe) sprite.TextureRect = new IntRect(0, 0, 24, 24);
            else sprite.TextureRect = new IntRect(24, 0, 24, 24);
        }
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
                sprite.TextureRect = new IntRect(24, 0, 24, 24);
                landed = true;
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
                verticalSpeed = 0.0f;
                if (landed) 
                {
                    sprite.TextureRect = new IntRect(0, 0, 24, 24);
                    landed = false;
                }
            }
            else verticalSpeed = -0.5f * verticalSpeed;
        }
        else isUpPressed = false;
        if (verticalSpeed > 500.0f) verticalSpeed = 500.0f;
        
        if (!Collision.RectangleRectangle(Bounds,new FloatRect(0,0,Program.SCREEN_W,Program.SCREEN_H), out var _))
        {
            scene.Reload();
        }
    }

    public override void Render(RenderTarget target)
    {
        sprite.Scale = new Vector2f(faceRight ? -1 : 1, 1);
        base.Render(target);
    }
    public override FloatRect Bounds
    { get 
        {
            var bounds = base.Bounds;
            bounds.Left += 3;
            bounds.Width -= 6;
            bounds.Top += 3;
            bounds.Height -= 3;
            return bounds;
        }
    }
}