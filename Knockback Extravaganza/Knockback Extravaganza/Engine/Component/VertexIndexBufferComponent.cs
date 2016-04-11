using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class VertexIndexBufferComponent<T> : VertexBufferComponent<T> where T : struct, IVertexType {
        public int[] Indices;
        public IndexBuffer IndexBuffer { get; private set; }
        public VertexIndexBufferComponent(T[] vertices, int[] indices, GraphicsDevice device) : base(vertices, device){
            Indices = indices;
            IndexBuffer = new IndexBuffer(device, typeof(int), Indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData(Indices);
        }

        public override int Count {
            get {
                if(count == 0) {
                    count = Indices.Length / 3; 
                }
                return count;
            }
        }
    }
}
