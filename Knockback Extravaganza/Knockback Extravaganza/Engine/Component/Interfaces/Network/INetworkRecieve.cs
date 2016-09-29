using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component.Interfaces.Network {
    public interface INetworkRecieve {
        void UnpackMessage(NetIncomingMessage msg);
    }
}
