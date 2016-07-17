using ECS_Engine.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Scenes {
    public class Menu : Scene {

        public Menu(string name, ContentManager content, GraphicsDeviceManager graphics) : base(name, content, graphics) { }

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
