﻿using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class TransformComponent : IComponent{
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; }

        public Matrix World() {
            return Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
    }
}
