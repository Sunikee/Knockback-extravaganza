using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class ModelComponent : IComponent{
        public Model Model { get; set; }
        public Texture2D Texture { get; set; }

        public ModelComponent() {
            Texture = null;
        }
    }
}
