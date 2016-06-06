using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Components {
    public class PlayerComponent : IComponent
    {
        public float Health { get; set; }
        public float knockBackResistance { get; set; }
        public float ChargeTime { get; set; } = 0;
    }
}
