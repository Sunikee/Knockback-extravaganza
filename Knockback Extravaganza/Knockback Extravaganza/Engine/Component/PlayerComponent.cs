using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component
{
    public class PlayerComponent : IComponent
    {
        public int Health { get; set; }
        public float KnockbackResistance { get; set; }
    }
}
