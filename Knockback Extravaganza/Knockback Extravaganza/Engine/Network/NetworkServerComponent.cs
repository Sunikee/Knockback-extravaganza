using ECS_Engine.Engine.Component.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Network {
    public class NetworkServerComponent : IComponent{
        public NetServer Server { get; set; }

        public NetworkServerComponent() {
            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.MaximumConnections = 2;
            config.Port = 14242;
            Server = new NetServer(config);
            Server.Start();
        }
    }
}
