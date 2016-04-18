using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component
{
    public class PhysicsComponent : IComponent 
    {
        public float Gravity = 9.82f;
        public float GravityStrength { get; set; }
        public float Friction { get; set; }
        public bool InAir { get; set; }
        
    }
}
