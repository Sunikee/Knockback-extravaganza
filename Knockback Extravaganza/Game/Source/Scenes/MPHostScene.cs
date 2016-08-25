using ECS_Engine.Engine.Network;
using ECS_Engine.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Scenes {
    class MPHostScene : Scene {

        public MPHostScene(string name, ContentManager content, GraphicsDeviceManager graphics) : base(name, content, graphics) { }

        public override void InitScene() {

            var hostEntity = ComponentManager.MakeEntity();
            var host = new NetworkServerComponent();

            ComponentManager.AddComponent(hostEntity, host);


            SystemManager.AddSystem(new NetworkServerSystem());

            base.InitScene();
        }

        public override void ResetScene() {
            
        }
    }
}
