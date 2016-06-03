using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine
{
    public class AIComponent : IComponent
    {
        public int Target { get; set; }
        public int Duration { get; set; }

    }
}
