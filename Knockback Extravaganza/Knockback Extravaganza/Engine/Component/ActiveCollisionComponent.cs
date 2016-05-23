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
        private ActiveCollisionComponent(Model model, Matrix world) : base(model, world)
        {
        }
    }
}
