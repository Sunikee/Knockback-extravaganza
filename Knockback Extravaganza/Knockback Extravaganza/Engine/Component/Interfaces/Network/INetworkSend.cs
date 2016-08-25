using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component.Interfaces.Network {
    interface INetworkSend {
        NetOutgoingMessage PackMessage(int entityID, NetClient client);
    }
}
