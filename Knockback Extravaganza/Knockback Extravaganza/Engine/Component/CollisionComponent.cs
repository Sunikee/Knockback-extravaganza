using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Component
{
    public abstract class CollisionComponent: IComponent
    {
        public BoundingBox BoundingBox { get; set; }
        public Vector3 Maximum { get; set; }
        public Vector3 Minimum { get; set; }

        public Dictionary<int, BoundingSphere> CollisionList { get; set; } = new Dictionary<int, BoundingSphere>();

        protected CollisionComponent(Model model, Matrix world)
        {
            world = Matrix.Identity;
            Minimum = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Maximum = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            var transforms = new Matrix[model.Bones.Count()];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in model.Meshes)
            {

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    var vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (var i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        var transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), transforms[mesh.ParentBone.Index] * world);

                        Minimum = Vector3.Min(Minimum, transformedPosition);
                        Maximum = Vector3.Max(Maximum, transformedPosition);
                    }
                }
            }
            BoundingBox = new BoundingBox(Minimum,Maximum);
        }

    }
}