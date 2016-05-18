using ECS_Engine.Engine.Component.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Net;

namespace ECS_Engine.Engine.Component.Network
{
    public class ServerNetworkComponent : IComponent
    {
        public NetServer Server { get; set; }
        public NetPeerConfiguration Config { get; set; }
        public int Port { get; set; }
        public string HostIp { get; set; }
        public List<IPEndPoint> ClientConnections { get; set; }
    }
}
