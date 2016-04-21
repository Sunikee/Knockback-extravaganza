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
    public class CollisionComponent: IComponent
    {
        public Dictionary<Entity, Boolean> CollidedWith { get; set; }

        public BoundingBox boundingBox { get; set; }

        public CollisionComponent(Model model, Matrix worldTransform)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices*vertexStride;

                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }
            this.boundingBox = new BoundingBox(min, max);
        }
    }
}
/*Vector3 modelMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            Vector3 modelMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

            Matrix[] boneTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                Vector3 meshMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                Vector3 meshMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                    byte[] vertexData = new byte[stride * meshPart.NumVertices];
                    meshPart.VertexBuffer.GetData(meshPart.VertexOffset*stride, vertexData,0,meshPart.NumVertices,1);

                    Vector3 vertPosition = new Vector3();

                    for (int index = 0; index < vertexData.Length; index += stride)
                    {
                        vertPosition.X = BitConverter.ToSingle(vertexData, index);
                        vertPosition.Y = BitConverter.ToSingle(vertexData, index + sizeof(float));
                        vertPosition.Z = BitConverter.ToSingle(vertexData, index + sizeof(float) * 2);

                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }

                }
                meshMin = Vector3.Transform(meshMin, boneTransforms[mesh.ParentBone.Index]);
                meshMax = Vector3.Transform(meshMax, boneTransforms[mesh.ParentBone.Index]);

                meshMin = Vector3.Min(modelMin, meshMin);
                meshMax = Vector3.Max(modelMax, meshMax);
            }
            this.boundingBox = new BoundingBox(modelMin, modelMax);
*/