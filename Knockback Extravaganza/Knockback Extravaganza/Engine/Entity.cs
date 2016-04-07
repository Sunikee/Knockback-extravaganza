using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine {
    public class Entity {
        public string Tag { get; set; }
        public bool Active { get; set; }

        public Entity(string tag) {
            Tag = tag;
            Active = true;
        }
    }
}
