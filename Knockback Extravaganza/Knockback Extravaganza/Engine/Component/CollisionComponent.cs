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
        private readonly Dictionary<Entity, bool> _collidedWith  = new Dictionary<Entity, bool>();

        public BoundingBox BoundingBox { get; set; }

        public Vector3 Minimum { get; set; }
        public Vector3 Maximum { get; set; }

        public void RegCollision(Entity entity, bool collided)
        {
            if (!_collidedWith.ContainsKey(entity))
            {
                _collidedWith.Add(entity, collided);
            }
            else
            {
                _collidedWith[entity] = collided;
            }
        }

        public Dictionary<Entity, bool> GetCollision()
        {
            return _collidedWith;
        }

    }
}