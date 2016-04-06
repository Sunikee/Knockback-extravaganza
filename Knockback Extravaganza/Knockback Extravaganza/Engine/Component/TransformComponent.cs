using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class TransformComponent : IComponent{
        Vector3 Position { get; set; }
        Vector3 Scale { get; set; }
        Vector3 Rotation { get; set; }

        public Matrix World() {
            return Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
    }
}
