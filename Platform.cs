using SFML.Graphics;
using SFML.System;

namespace Platformer;

// Connected Tiles
enum CoTi
{
    NONE  = 0b_0000_0000,
    RIGHT = 0b_0000_0001,
    LEFT  = 0b_0000_0010,
    UP    = 0b_0000_0100,
    DOWN  = 0b_0000_1000
}
class Platform : Entity
{
    public override bool Solid => true;
    
    private static Dictionary<byte, Vector2i> textures = new Dictionary<byte, Vector2i>()
    {
        [(byte)CoTi.NONE]                                      = new (0,0),
        [(byte)CoTi.RIGHT]                                     = new (18, 0),
        [(byte)(CoTi.RIGHT | CoTi.LEFT)]                       = new (36,0),
        [(byte)(CoTi.LEFT)]                                    = new (54,0),
        [(byte)(CoTi.DOWN)]                                    = new (0,18),
        [(byte)(CoTi.RIGHT | CoTi.DOWN)]                       = new (18,18),
        [(byte)(CoTi.RIGHT | CoTi.LEFT | CoTi.DOWN)]           = new (36,18),
        [(byte)(CoTi.LEFT | CoTi.DOWN)]                        = new (54,18),
        [(byte)(CoTi.UP | CoTi.DOWN)]                          = new (0,108),
        [(byte)(CoTi.RIGHT | CoTi.UP | CoTi.DOWN)]             = new (18,108),
        [(byte)(CoTi.RIGHT | CoTi.LEFT | CoTi.UP | CoTi.DOWN)] = new (36,108),
        [(byte)(CoTi.LEFT | CoTi.UP | CoTi.DOWN)]              = new (54,108),
        [(byte)(CoTi.UP)]                                      = new (0,126),
        [(byte)(CoTi.RIGHT | CoTi.UP)]                         = new (18,126),
        [(byte)(CoTi.RIGHT | CoTi.LEFT | CoTi.UP)]             = new (36,126),
        [(byte)(CoTi.LEFT | CoTi.UP)]                          = new (54,126)
    };
    
    public Platform() : base("tileset")
    {
        sprite.TextureRect = new IntRect(0, 0, 18, 18);
        sprite.Origin = new Vector2f(9, 9);
    }

    public void ConnectPlatforms(List<Platform> platforms)
    {
        uint connections = (byte)CoTi.NONE;
        
        foreach (var p in platforms)
        {
            if (p.Position == Position) continue;

            // Working with squares allows for simplified trigonometry
            float distance = MathF.Abs(Position.X - p.Position.X) + MathF.Abs(Position.Y - p.Position.Y);
            
            // tile size +1 to account for float point errors
            if (distance < 19)
            {
                if (p.Position.X > Position.X)
                {
                    connections |= (byte)CoTi.RIGHT;
                    continue;
                }
                if (p.Position.X < Position.X)
                {
                    connections |= (byte)CoTi.LEFT;
                    continue;
                }

                if (p.Position.Y < Position.Y)
                {
                    connections |= (byte)CoTi.UP;
                    continue;
                }

                if (p.Position.Y > Position.Y)
                {
                    connections |= (byte)CoTi.DOWN;
                }
            }
        }
        sprite.TextureRect = new IntRect(textures[(byte)connections], new Vector2i(18, 18));
    }
}