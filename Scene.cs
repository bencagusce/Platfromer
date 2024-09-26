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
    private string? nextScene;
    private string currentScene;

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

    public void Reload(){
        nextScene = currentScene;
    }

    public void Load(string newScene){
        nextScene = newScene;
    }

    public bool FindByType<T>(out T found) where T : Entity
    {
        foreach (var entity in entities)
        {
            if (!entity.Dead && entity is T typed)
            {
                found = typed;
                return true;
            }
        }
        found = default(T);
        return false;
    }
    private void HandleSceneChange(){
        if (nextScene == null) return;
        if (nextScene == "level0") Score.score = 0;
        if (nextScene == currentScene) Score.score = Score.checkPointScore;
        else Score.checkPointScore = Score.score;
        entities.Clear();
        Spawn(new Background());

        string file = $"assets/{nextScene}.txt";
        Console.WriteLine($"Loading scene '{file}'");
        
        // Load scene from file
        foreach (var line in File.ReadLines(file, Encoding.UTF8)) {
            
            string[] words = line.Split(" ");
            
            switch (words[0])
            {
                case "w":
                {
                    int posX;
                    int posY;
                    if (!int.TryParse(words[1], out posX)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    if (!int.TryParse(words[2], out posY)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    Spawn(new Platform { Position = new Vector2f(posX, posY) });
                    break;
                }
                case "d":
                {
                    int posX;
                    int posY;
                    if (!int.TryParse(words[1], out posX)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    if (!int.TryParse(words[2], out posY)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    Spawn(new Door { Position = new Vector2f(posX, posY), NextRoom = words[3]});
                    break;
                }
                case "k":
                {
                    int posX;
                    int posY;
                    if (!int.TryParse(words[1], out posX)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    if (!int.TryParse(words[2], out posY)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    Spawn(new Key { Position = new Vector2f(posX, posY) });
                    break;
                }
                case "h":
                {
                    int posX;
                    int posY;
                    if (!int.TryParse(words[1], out posX)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    if (!int.TryParse(words[2], out posY)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    Spawn(new Hero { Position = new Vector2f(posX, posY) });
                    break;
                }
                case "c":
                {
                    int posX;
                    int posY;
                    if (!int.TryParse(words[1], out posX)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    if (!int.TryParse(words[2], out posY)) throw new Exception($"Failed to parse coordinates in 'assets/{file}.txt'");
                    Spawn(new Coin { Position = new Vector2f(posX, posY) });
                    break;
                }
            }
        }
        Spawn(new Score { Position = new Vector2f(360, 18)});
        Spawn(new Coin { Position = new Vector2f(378, 18)});

        // foreach (Platform platform in entities.Where(a => a is Platform))
        // {
        //     platform.ConnectPlatforms(entities.Where(a => a is Platform).ToList());
        // }

        foreach (Platform platform in entities.OfType<Platform>())
        {
            platform.ConnectPlatforms(entities.OfType<Platform>().ToList());
        }
        
        currentScene = nextScene;
        nextScene = null;
    }

    public void UpdateAll(float deltaTime)
    {
        HandleSceneChange();
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