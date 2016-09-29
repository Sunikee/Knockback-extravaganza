using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component.Interfaces.Network;
using ECS_Engine.Engine.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Network {

    public class NetworkData {
        public NetIncomingMessage Msg { get; set;}
        public INetworkRecieve Comp { get; set; } = null;
    }

    public class NetworkDataComponent : IComponent{
        public Dictionary<string, NetworkData> NetMessages = new Dictionary<string, NetworkData>();


        public NetIncomingMessage Update(string id, NetIncomingMessage msg) {
            if (!NetMessages.ContainsKey(id)) {
                NetMessages.Add(id, new NetworkData { Msg = msg});
            }
            else {
                var oldMsg = NetMessages[id].Msg;
                if(oldMsg.ReceiveTime < msg.ReceiveTime) {
                    NetMessages[id].Msg = msg;
                    return oldMsg;
                }
            }
            return null;
        }
    }
}
