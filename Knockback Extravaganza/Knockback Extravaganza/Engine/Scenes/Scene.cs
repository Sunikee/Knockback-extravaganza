using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Scenes
{
    public abstract class Scene
    {
        public string Name { get; set; }
        public Texture2D Background { get; set; }
        public SpriteFont Font { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public List<string> menuChoices { get; set; }
    }
}
