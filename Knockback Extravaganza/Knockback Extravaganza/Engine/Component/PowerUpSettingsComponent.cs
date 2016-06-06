using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component
{
    public class PowerUpSettingsComponent : IComponent
    {
        public float powerUpSpawnTimer { get; set; }
        public int randomSpawnTimerInt { get; set; }
        public Random random { get; set; }
        public float maxCoordX { get; set; }
        public float minCoordX { get; set; }
        public float maxCoordZ { get; set; }
        public float minCoordZ { get; set; }
    }
}