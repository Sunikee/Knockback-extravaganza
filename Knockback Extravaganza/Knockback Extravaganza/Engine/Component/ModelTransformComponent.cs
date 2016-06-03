using ECS_Engine.Engine.Component.Abstract_Classes;
using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class MeshTransform {

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Matrix ParentBone { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 Up { get; set; }

        private Matrix[] world = new Matrix[3];

        public Matrix GetWorld(int buffer) {
            return world[buffer];
        }
        public void SetWorld(int buffer, Matrix world) {
            this.world[buffer] = world;
        }

        public MeshTransform(Matrix parentBone) {
            ParentBone = parentBone;
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
        }
    }
    public class ModelTransformComponent : ThreadedComponent{
        

        Dictionary<string, MeshTransform> meshTransformations = new Dictionary<string, MeshTransform>();

        public ModelTransformComponent(Model model) {
            Matrix[] transforms = new Matrix[model.Bones.Count()];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes) {
                MeshTransform m = new MeshTransform(transforms[mesh.ParentBone.Index]);
                meshTransformations.Add(mesh.Name, m);
            }
        }

        public override void CopyThreadedData(int to, int from = 0) {
            var transforms = GetTransforms();
            foreach(var transform in transforms) {
                transform.Value.SetWorld(to, transform.Value.GetWorld(from));
            }
        }

        public MeshTransform GetTransform(string name) {
            if (meshTransformations.Keys.Contains(name)) {
                return meshTransformations[name];
            }
            return null;
        }

        public Dictionary<string, MeshTransform> GetTransforms() {
            return meshTransformations;
        }
    }
}
