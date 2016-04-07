using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine.Component {
    public class VertexPosColorComponent : IComponent{
        public VertexPositionColor[] Vertices { get; set; }
        public short[] Indices { get; set; }
        public PrimitiveType Type { get; set; }

        private int count = 0;

        public int Count() {
            if (count == 0) {
                if (Indices != null) {
                    count = Indices.Count() / 3;
                }
                else {
                    count = Vertices.Count() / 3;
                }
            }
            return count;
        }
    }
}

