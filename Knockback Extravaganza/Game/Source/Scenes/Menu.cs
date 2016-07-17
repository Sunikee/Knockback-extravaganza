using ECS_Engine.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Scenes {
    public class Menu : Scene {

        public Menu(string name) : base(name) { }

        public override void InitScene() {
            
            base.InitScene();
        }

        public override void ResetScene() {
            throw new NotImplementedException();
        }

        public override void DestroyScene() {
            base.DestroyScene();
        }
    }
}
