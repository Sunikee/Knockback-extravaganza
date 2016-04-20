using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ECS_Engine.Engine.Component
{
    public class ActiveCollisionComponent : CollisionComponent
    {
        public ActiveCollisionComponent(Model model, Matrix worldTransform) : base(model, worldTransform)
        {
        }
    }
}
