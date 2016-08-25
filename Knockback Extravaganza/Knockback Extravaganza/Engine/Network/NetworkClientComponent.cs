using ECS_Engine.Engine.Component.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Network {
    public class NetworkClientComponent : IComponent{

        public NetClient Client { get; set; }

        public NetworkClientComponent() {
            NetPeerConfiguration config = new NetPeerConfiguration("game");
            Client = new NetClient(config);
            Client.Start();
        }

    }
}
