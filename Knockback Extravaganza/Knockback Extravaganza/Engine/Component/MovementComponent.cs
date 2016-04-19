using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine
{
    public class MovementComponent : IComponent
    {
        public Vector3 Velocity {get; set;}
        public float Acceleration { get; set; }
        public float Speed { get; set; }
    
    }
}
