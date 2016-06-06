//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Lidgren.Network;
//using ECS_Engine.Engine.Systems.Interfaces;
//using ECS_Engine.Engine.Component.Network;
//using ECS_Engine.Engine.Managers;
//using ECS_Engine.Engine.Component.Interfaces;

//namespace ECS_Engine.Engine.Systems.Network
//{
//    class ClientNetworkSystem : INetworkSystem
//    {

//        /// <summary>
//        /// Starts a client with the nessesary data
//        /// </summary>
//        /// <param name="componentManager"></param>
//        /// <param name="hostIp"></param>
//        /// <param name="port"></param>
//        public void Start(ComponentManager componentManager, string hostIp, int port)
//        {
//            Entity ClientEntity = new Entity();
//            var clientC = new ClientNetworkComponent
//            {
//                Config = new NetPeerConfiguration("knockbackGame"),
//            };

//            clientC.Config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

//            clientC.Client = new NetClient(clientC.Config);
//            clientC.Client.DiscoverLocalPeers(14242);
//            componentManager.AddComponent(ClientEntity, clientC);
            
//        }

//        /// <summary>
//        /// Connects to a server and starts the running loop
//        /// </summary>
//        /// <param name="componentManager"></param>
//        public void Run(ComponentManager componentManager)
//        {
//            var clientComponents = componentManager.GetComponents<ClientNetworkComponent>();


//            if (clientComponents != null)
//            {
//                foreach (KeyValuePair<Entity, IComponent> component in clientComponents)
//                {
//                    var clientC = componentManager.GetComponent<ClientNetworkComponent>(component.Key);

//                    NetIncomingMessage message;
//                    while ((message = clientC.Client.ReadMessage()) != null)
//                    {
//                        switch (message.MessageType)
//                        {
//                            case NetIncomingMessageType.Data:
//                                // handle custom messages
//                                var data = message.ReadInt32();
//                                break;

//                            case NetIncomingMessageType.StatusChanged:
//                                // handle connection status messages
//                                switch (message.SenderConnection.Status)
//                                {
//                                    // if (message.SenderConnection.Status == NetConnectionStatus.Connected)
//                                    ////Handle when client connects
//                                    //else
//                                    //Handle disconnect from server
//                                }
//                                break;

//                            case NetIncomingMessageType.DebugMessage:
//                                //For debugging

//                                Console.WriteLine(message.ReadString());
//                                break;
//                                switch (message.MessageType)
//                                {
//                                    case NetIncomingMessageType.DiscoveryResponse:
//                                        Console.WriteLine("Found server at " + message.SenderEndPoint + " name: " + message.ReadString());
//                                        clientC.Client.Connect(message.SenderEndPoint);
//                                        //Handle connection
//                                        break;


//                                    default:
//                                        Console.WriteLine("unhandled message with type: "
//                                            + message.MessageType);
//                                        break;
//                                }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
