using ECS_Engine.Engine.Network;
using ECS_Engine.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Game.Source.Scenes {
    class MPClientScene : Scene {

        public MPClientScene(string name, ContentManager content, GraphicsDeviceManager graphics) : base(name, content, graphics) { }

        public override void InitScene() {

            var clientEntiry = ComponentManager.MakeEntity();

            var client = new NetworkClientComponent();
            client.Client.Connect("localhost", 14242);

            ComponentManager.AddComponent(clientEntiry, client);

            SystemManager.AddSystem(new NetworkClientSystem());

            base.InitScene();
        }

        public override void ResetScene() {

        }
    }
}
