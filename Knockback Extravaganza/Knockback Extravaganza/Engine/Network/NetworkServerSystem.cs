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
    public class NetworkServerSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager) {
            NetworkServerComponent networkServer = componentManager.GetComponents<NetworkServerComponent>().First().Value as NetworkServerComponent;

            if (networkServer != null) {
                RecieveMsg(networkServer.Server);

                SendMsg(networkServer.Server);
            }
        }

        private void RecieveMsg(NetServer netServer) {
            if (netServer.ConnectionsCount > 0) {
                NetIncomingMessage inc;

                while ((inc = netServer.ReadMessage()) != null) {
                    switch (inc.MessageType) {
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            //Error code
                            break;
                        case NetIncomingMessageType.Data:
                            // Master Server
                            List<NetConnection> connections = netServer.Connections;
                            connections.Remove(inc.SenderConnection);
                            if (connections.Count > 0) {
                                var msg = netServer.CreateMessage();
                                msg.Write(inc.Data);
                                netServer.SendMessage(msg, connections.First(), NetDeliveryMethod.ReliableOrdered, 0);
                            }
                            break;
                    }
                    netServer.Recycle(inc);
                }
            }
        }

        private void SendMsg(NetServer netClient) {
            if (netClient.ConnectionsCount > 0) {
                
            }
        }
    }
}
