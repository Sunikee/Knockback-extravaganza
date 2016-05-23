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
        public PassiveCollisionComponent(Model model, Matrix world): base(model, world)
        { 
        }
    }
}
