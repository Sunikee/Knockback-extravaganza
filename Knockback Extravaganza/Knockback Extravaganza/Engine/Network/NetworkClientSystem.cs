using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using ECS_Engine.Engine.Component.Interfaces.Network;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine.Network {
    public class NetworkClientSystem : IUpdateSystem {

        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager) {
            NetworkClientComponent networkClient = componentManager.GetComponents<NetworkClientComponent>().First().Value as NetworkClientComponent;
            var networkData = componentManager.GetComponents<NetworkDataComponent>().First().Value as NetworkDataComponent;

            if(networkClient != null && networkData != null) {
                RecieveMsg(networkClient.Client, networkData);


                var sendComp = componentManager.GetComponentsOfType<INetworkSend>();

                foreach (var comp in sendComp) {
                    var entity = componentManager.GetEntity(comp as IComponent);
                    var id = string.Format("{0},{1},{2},{3}", networkClient.Client.UniqueIdentifier, entity.Tag, entity.ID, comp.ToString());
                    networkClient.Client.SendMessage(comp.PackMessage(id, networkClient.Client), NetDeliveryMethod.ReliableOrdered);
                }
            }
        }

        private void RecieveMsg(NetClient netClient, NetworkDataComponent networkData) {
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

                        if (inc.PositionInBytes == 0 && inc.LengthBytes > 0) {
                            string id = inc.ReadString();
                            inc = networkData.Update(id, inc);
                        }
                        break;
                }
                if (inc != null) {
                    netClient.Recycle(inc);
                }
            }
            
        }
    }
}
