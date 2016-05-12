using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Component
{
    public class PassiveCollisionComponent : CollisionComponent
    {
        public BoundingBox BoundingBox { get; set; }
        public Vector3 Maximum { get; set; }
        public Vector3 Minimum { get; set; }
    }
}
