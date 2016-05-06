using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Component {
    public class FreeCameraComponent : IComponent {
        public float LeftRightRot { get; set; }
        public float UpDownRot { get; set; }
        public float RotationSpeed { get; set; }
        public float MoveSpeed { get; set; }

        public GraphicsDevice GraphicsDevice { get; set; }

        public Game Game { get; set; }

        public Vector2 OriginalMouseState { get; set; }

        public FreeCameraComponent()
        {
            OriginalMouseState = Vector2.Zero;
            LeftRightRot = 0f;
            UpDownRot = 0f;
            RotationSpeed = 0.3f;
            MoveSpeed = 30.0f;
        }

        
    }
}
