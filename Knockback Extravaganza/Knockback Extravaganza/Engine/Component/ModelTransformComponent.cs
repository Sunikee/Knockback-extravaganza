using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class ModelTransformComponent : IComponent{
        public class MeshTransform {
            
            public Vector3 Position { get; set; }
            public Vector3 Rotation { get; set; }
            public Vector3 Scale { get; set; }

            public MeshTransform() {
                Position = Vector3.Zero;
                Rotation = Vector3.Zero;
                Scale = Vector3.One;
            }

            public Matrix World {
                get {
                    return Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(Quaternion.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z)) * Matrix.CreateTranslation(Position);
                }
            }
        }

        Dictionary<string, MeshTransform> meshTransformations = new Dictionary<string, MeshTransform>();

        public ModelTransformComponent(Model model) {
            foreach (ModelMesh mesh in model.Meshes) {
                meshTransformations.Add(mesh.Name, new MeshTransform());
            }
        }

        public MeshTransform GetTranform(string name) {
            if (meshTransformations.Keys.Contains(name)) {
                return meshTransformations[name];
            }
            return null;
        }
    }
}
