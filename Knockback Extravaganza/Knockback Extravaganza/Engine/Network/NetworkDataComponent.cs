using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Network {
    public class NetworkDataComponent : IComponent{
        public Dictionary<int, object[]> NetMessages = new Dictionary<int,object[]>();


        public NetIncomingMessage Update(int id, NetIncomingMessage msg) {
            if (!NetMessages.ContainsKey(id)) {
                NetMessages.Add(id, new[] { null, msg });
            }
            else {
                var oldMsg = NetMessages[id][1] as NetIncomingMessage;
                if(oldMsg.ReceiveTime < msg.ReceiveTime) {
                    NetMessages[id][1] = msg;
                    return oldMsg;
                }
            }
            return null;
        }
    }
}
