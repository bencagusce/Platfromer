using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using System.IO;
using System.Text;

namespace Platformer;
public class Scene
{
    private readonly Dictionary<string, Texture> textures;
    private readonly List<Entity> entities;
    private string nextScene;
    private string currentScene;

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

    public void Reload(){
        nextScene = currentScene;
    }

    public void Load(string newScene){
        nextScene = newScene;
        HandleSceneChange();
    }

    private void HandleSceneChange(){
        if (nextScene == null) return;
        entities.Clear();
        Spawn(new Background());

        string file = $"assets/{nextScene}.txt";
        Console.WriteLine($"Loading scene '{file}'");

        // Load scene from file
        foreach (var line in File.ReadLines(file, Encoding.UTF8)) {
            // steps 44-45 was skipped
            // string parsed = line.Trim();

            string[] words = line.Split(" ");

            switch (words[0])
            {
                case "w":
                {
                    int posX;
                    int posY;
                    if (!int.TryParse(words[1], out posX)) throw new Exception($"Failed to parse tile coordinates in 'assets/{file}.txt'");
                    if (!int.TryParse(words[2], out posY)) throw new Exception($"Failed to parse tile coordinates in 'assets/{file}.txt'");
                    Spawn(new Platform { Position = new Vector2f(posX, posY) });
                    Console.WriteLine($"Spawned platform at {posX},{posY}");
                    break;
                }
                case "d": break;
                case "k": break;
                case "h": break;

                default: break;
            }
        }

        currentScene = nextScene;
        nextScene = null;
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
        foreach (var entity in entities)
        {
            entity.Render(target);
        }
    }
}