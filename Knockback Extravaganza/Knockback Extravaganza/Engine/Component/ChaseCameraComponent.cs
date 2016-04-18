using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class ChaseCameraComponent : IComponent{
        public Entity Target { get; set; }
        public Vector3 TargetOffSet { get; set; }
        public Vector3 Offset { get; set; }
    }
}
