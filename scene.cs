using System.Collections.Generic;
using SFML.Graphics;

namespace Platformer;
public class Scene
{
    private readonly DICTIONARY<string, Texture> textures;
    private readonly LIST<Entity> entities;

    public Scene()
    {
        textures = new DICTIONARY<string, Texture>();
        entities = new LIST<Entity>();
    }
}