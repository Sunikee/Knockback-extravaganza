using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine {
    public class Entity {
        public int ID { get; set; }
        public bool Active { get; set; }

        public Entity(int id) {
            ID = id;
            Active = true;
        }
    }
}
