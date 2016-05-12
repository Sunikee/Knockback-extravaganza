using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component
{
    public class PowerUpComponent : IComponent
    {
        public bool IsActive { get; set; }
        public float ActiveTime { get; set; }
        public int PowerUpType { get; set; }
    }
}
