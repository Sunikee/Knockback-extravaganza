using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Components.AI {
    public class AIComponent : IComponent
    {
        public int Target { get; set; }
        public int Duration { get; set; }

        public string State { get; set; } = "follow";
    }
}
