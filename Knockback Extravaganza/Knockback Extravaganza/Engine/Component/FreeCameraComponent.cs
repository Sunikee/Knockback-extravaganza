using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ECS_Engine.Engine.Component {
    public class FreeCameraComponent : IComponent {
        public float leftRightRot { get; set; }
        public float upDownRot { get; set; }
        public float rotationSpeed { get; set; }
        public float moveSpeed { get; set; }

        public FreeCameraComponent()
        {
            leftRightRot = 0f;
            upDownRot = 0f;
            rotationSpeed = 0.3f;
            moveSpeed = 30.0f;
        }
    }
}
