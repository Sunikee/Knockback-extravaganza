using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO: ADD TEXTURE VARIABLES
namespace ECS_Engine.Engine.Component {
    public class VertexComponent<T> : IComponent where T : struct, IVertexType{
        
        public T[] Vertices { get; set; }
        public BasicEffect Effect { get; set; }

        public PrimitiveType Type { get; set; }
        protected int count = 0;

        public virtual int Count {
            get {
                if(count == 0) {
                    count = Vertices.Length / 3;
                }
                return count;
            }
        }

    }
}
