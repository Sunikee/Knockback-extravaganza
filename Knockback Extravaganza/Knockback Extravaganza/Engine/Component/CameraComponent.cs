using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ECS_Engine.Engine.Component {
    public class CameraComponent : IComponent{

        public Matrix View { get; set; }
        public Matrix Projection {
            get {
                return Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NearPlaneDistace, FarPlaneDistace);
            }
        }
        public Vector3 Target { get; set; }
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlaneDistace { get; set; }
        public float FarPlaneDistace { get; set; }


    }
}
