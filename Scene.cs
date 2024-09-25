using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace Platformer;
public class Scene
{
    private readonly Dictionary<string, Texture> textures;
    private readonly List<Entity> entities;

    public bool TryMove(Entity entity, Vector2f movement)
    {
        entity.Position += movement;
        bool collided = false;
        for (int i = 0; i < entities.Count; i++)
        {
            Entity other = entities[i];
            if (!other.Solid) continue;
            if (other == entity) continue;
            FloatRect boundsA = entity.Bounds;
            FloatRect boundsB = other.Bounds;
            if (Collision.RectangleRectangle(boundsA, boundsB, out Collision.Hit hit))
            {
                entity.Position += hit.Normal * hit.Overlap;
                i = -1; //check everything onve again
                collided = true;
                //Todo: resolve collision using "hit variable (31)
            }
        }
        return collided;
    }
    public Scene()
    {
        textures = new Dictionary<string, Texture>();
        entities = new List<Entity>();
    }

    public void Spawn(Entity entity)
    {
        entities.Add(entity);
        entity.Create(this);
    }

    public Texture LoadTexture(string name)
    {
        if (textures.TryGetValue(name, out Texture found)) return found;
        string fileName = $"assets/{name}.png";
        Texture texture = new Texture(fileName);
        textures.Add(name, texture);
        return texture;
    }

    public void UpdateAll(float deltaTime)
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            Entity entity = entities[i];
            entity.Update(this, deltaTime);
        }

        for (int i = 0; i < entities.Count;)
        {
            Entity entity = entities[i];
            if (entity.Dead) entities.RemoveAt(i);
            else i++;
        }
    }

    public void RenderAll(RenderTarget target)
    {
        
    }
}