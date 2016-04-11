using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class VertexBufferComponent<T> : VertexComponent<T> where T : struct, IVertexType{

        public VertexBufferComponent(T[] vertices, GraphicsDevice device) {
            Vertices = vertices;
            VertexBuffer = new VertexBuffer(device, typeof(T), Vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData(Vertices);
        }

        public VertexBuffer VertexBuffer { get; private set; }
    }
}
