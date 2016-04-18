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
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Right { get; set; }
        public Vector3 Up { get; set; }
        public Matrix World { get; set; }

        public MeshTransform() {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
        }
    }
    public class ModelTransformComponent : IComponent{
        

        Dictionary<string, MeshTransform> meshTransformations = new Dictionary<string, MeshTransform>();

        public ModelTransformComponent(Model model) {
            foreach (ModelMesh mesh in model.Meshes) {
                MeshTransform m = new MeshTransform();
                //m.Position = mesh.ParentBone.ModelTransform.Translation;
                //m.Scale = mesh.ParentBone.ModelTransform.Scale;
                //m.Rotation = mesh.ParentBone.ModelTransform.Rotation;
                meshTransformations.Add(mesh.Name, m);
            }
        }

        public MeshTransform GetTranform(string name) {
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
