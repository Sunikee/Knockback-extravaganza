using ECS_Engine.Engine.Component.Abstract_Classes;
using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class TransformComponent : ThreadedComponent{
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 Up { get; set; }

        private Matrix[] world = new Matrix[3];

        public TransformComponent() {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
            for(int i = 0; i < 3; i++) {
                world[i] = Matrix.Identity;
            }
        }

        
        /*public Matrix World {
            get {
                return world[CurrentRenderBuffer];
            }
            set {
                world[UpdateBuffer] = value;
            }
        }*/

        public Matrix GetWorld(int buffer) {
            return world[buffer];
        }
        public void SetWorld(int buffer, Matrix matrix) {
            world[buffer] = matrix;
        }

        public override void CopyThreadedData(int to, int from) {
            SetWorld(to, GetWorld(from));
        }
    }
}

