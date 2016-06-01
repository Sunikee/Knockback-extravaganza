using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class SceneComponent {

        public bool IsActive { get; set; } = false;
        public bool IsPopup { get; set; }
        public bool IsBackground { get; set; }
    }
}
