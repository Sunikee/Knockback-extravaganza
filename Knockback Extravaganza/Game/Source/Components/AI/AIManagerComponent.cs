using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Components.AI {
    class AIManagerComponent : IComponent{
        public Model AIModel { get; set; }
        public Vector3 spawnMin { get; set; }
        public Vector3 spawnMax { get; set; }
        public float spawnTimer { get; set; }
        public float spawnAfterSeconds { get; set; }
    }
}
