using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component.Interfaces.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Lidgren.Network.Xna;

namespace ECS_Engine.Engine.Component.Network {
    public class NetworkRecieveTransformComponent : IComponent, INetworkRecieve{
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; }

        public void UnpackMessage(NetIncomingMessage msg) {
            Position = XNAExtensions.ReadVector3(msg);
            Scale = XNAExtensions.ReadVector3(msg);
            Rotation = XNAExtensions.ReadVector3(msg);
        }   
    }

    public class NetworkSendTransformComponent : IComponent , INetworkSend{
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; }

        public NetOutgoingMessage PackMessage(string identifier, NetClient client) {
            var msg = client.CreateMessage();
            msg.Write(identifier);
            XNAExtensions.Write(msg, Position);
            XNAExtensions.Write(msg, Scale);
            XNAExtensions.Write(msg, Rotation);
            return msg;
        }
        public override string ToString() {
            return "TransformComponent";
        }
    }
}
