using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class VertexIndexComponent<T> : VertexComponent<T> where T : struct, IVertexType {
        public int[] Indices { get; set; }

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
