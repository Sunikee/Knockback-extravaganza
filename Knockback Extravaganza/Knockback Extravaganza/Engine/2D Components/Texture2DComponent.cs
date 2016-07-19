using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine._2D_Components {
    public class Texture2DComponent : IComponent{
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
    }
}
