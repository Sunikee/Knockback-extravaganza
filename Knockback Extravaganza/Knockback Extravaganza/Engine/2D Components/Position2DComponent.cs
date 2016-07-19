using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine._2D_Components {
    public class Position2DComponent : IComponent{
        public Vector2 Postion { get; set; }
    }
}
