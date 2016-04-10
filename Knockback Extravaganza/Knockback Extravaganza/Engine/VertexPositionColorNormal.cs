using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine {
    public struct VertexPositionColorNormal : IVertexType {
        public Vector3 Position { get; set; }
        public Color Color { get; set; }
        public Vector3 Normal { get; set; }

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );

        public VertexPositionColorNormal(Vector3 position, Color color, Vector3 normal) {
            Position = position;
            Color = color;
            Normal = normal;
        }

        VertexDeclaration IVertexType.VertexDeclaration {
            get { return VertexDeclaration; }
        }

    }
}
