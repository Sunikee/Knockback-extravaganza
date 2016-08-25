using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace ECS_Engine.Engine.Network {
    public class NetworkClientSystem : IUpdateSystem {

        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager) {
            NetworkClientComponent networkClient = componentManager.GetComponents<NetworkClientComponent>().First().Value as NetworkClientComponent;

            if(networkClient != null) {
                RecieveMsg(networkClient.Client);

                SendMsg(networkClient.Client);
            }
        }

        private void RecieveMsg(NetClient netClient) {
            NetIncomingMessage inc;

            while ((inc = netClient.ReadMessage()) != null) {
                switch (inc.MessageType) {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        //Error code
                        break;
                    case NetIncomingMessageType.Data:
                        // Master Server
                        Console.WriteLine(inc.ReadString());
                        break;
                }
                netClient.Recycle(inc);
            }
            
        }

        private void SendMsg(NetClient netClient) {
            NetOutgoingMessage msg = netClient.CreateMessage("Test");
            netClient.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
