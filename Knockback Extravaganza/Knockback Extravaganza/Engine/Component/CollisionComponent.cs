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
    public interface CollisionComponent: IComponent
    {
        BoundingBox BoundingBox { get; set; }
        Vector3 Maximum { get; set; }
        Vector3 Minimum { get; set; }
    }
}