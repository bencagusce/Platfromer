using SFML.Graphics;
using SFML.System;

namespace Platformer;

class Score : Entity
{
    public static int score = 0;
    public static int checkPointScore = 0;
    public Score() : base("tileset")
    {
        sprite.TextureRect = new IntRect(18 * score,144,18,18);
        sprite.Origin = new Vector2f(9, 9);
    }
   
    public override void Render(RenderTarget target)
    {
        base.Render(target);
    }
    public void IncreaseScore()
    {
        score ++;
        sprite.TextureRect = new IntRect(
            sprite.TextureRect.Left + 18,
            sprite.TextureRect.Top,
            sprite.TextureRect.Width,
            sprite.TextureRect.Height
        );
    }
}
