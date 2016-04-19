using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Component
{
    public class CollisionComponent: IComponent
    {
        public Dictionary<Entity, Boolean> CollidedWith { get; set; }

        public BoundingSphere BoundingSphere { get; set; }
    }
}
