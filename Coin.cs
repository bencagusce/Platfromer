using SFML.Graphics;
using SFML.System;

namespace Platformer;

class Coin : Entity
{
    private float animationBuffer = 0;
    private const float KEYFRAME_THRESHOLD = 0.25f;
    private bool keyframe = false;
    public Coin() : base("tileset")
    {
        sprite.TextureRect = new IntRect(198, 126, 18, 18);
        sprite.Origin = new Vector2f(9, 9);
    }
    public override void Update(Scene scene, float deltaTime)
    {
        animationBuffer += deltaTime;
        if (animationBuffer > KEYFRAME_THRESHOLD) 
        {
            animationBuffer = 0;
            keyframe = !keyframe;
            if (keyframe) sprite.TextureRect = new IntRect(216, 126, 18, 18);
            else sprite.TextureRect = new IntRect(198, 126, 18, 18);
        }
        if (scene.FindByType<Hero>(out Hero hero))
        {
            if (Collision.RectangleRectangle(Bounds, hero.Bounds, out _))
            {
                //Coin dissapear on pickup and increase score
                Dead = true; 
                if (scene.FindByType<Score>(out Score score)) score.IncreaseScore();
            }
        }
    }
    public override FloatRect Bounds
    { get 
        {
            var bounds = base.Bounds;
            bounds.Left += 2;
            bounds.Width -= 4;
            bounds.Top += 2;
            bounds.Height -= 4;
            return bounds;
        }
    }
    
}