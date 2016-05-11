//using ECS_Engine.Engine.Component.Interfaces;
//using ECS_Engine.Engine.Component.Network;
//using ECS_Engine.Engine.Managers;
//using ECS_Engine.Engine.Systems.Interfaces;
//using Lidgren.Network;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;


//namespace ECS_Engine.Engine.Systems.Network
//{
//    enum PacketTypes
//    {
//        MOVE,
//        WORLDSTATE
//    }
//    class ServerNetworkSystem : INetworkSystem
//    {
//        /// <summary>
//        /// Starts a server with the nessesary parameters
//        /// </summary>
//        /// <param name="componentManager"></param>
//        /// <param name="hostIp"></param>
//        /// <param name="port"></param>
//        public void Start(ComponentManager componentManager, string hostIp, int port)
//        {
//            Entity serverEntity = new Entity();
//            ServerNetworkComponent serverC = new ServerNetworkComponent
//            {
//                HostIp = hostIp,

//                Port = port,
//                Config = new NetPeerConfiguration("KnockbackGame") { Port = port, MaximumConnections = 4 },
//                ClientConnections = new List<IPEndPoint>()
//            };

//            serverC.Server = new NetServer(serverC.Config);
//            serverC.Server.Start();
//                    Console.WriteLine("Server started");
//            componentManager.AddComponent(serverEntity, serverC);
//        }

//        /// <summary>
//        /// Starts the server and runs the loop
//        /// </summary>
//        /// <param name="componentManager"></param>
//        public void Run(ComponentManager componentManager)
//        {
//            Dictionary<Entity, IComponent> components = componentManager.GetComponents<ServerNetworkComponent>();
//            if (components != null)
//            {
//                foreach (KeyValuePair<Entity, IComponent> comp in components)
//                {
//                    ServerNetworkComponent server = componentManager.GetComponent<ServerNetworkComponent>(comp.Key);               

//                    foreach (var client in server.ClientConnections)
//                    {
//                        //Start thread and listen for each client.. do all the stuff below to all clients.
//                    }

//                    NetIncomingMessage message;
//                    while ((message = server.Server.ReadMessage()) != null)
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
//                                    case (NetConnectionStatus.Connected):

//                                        var byteList = message.ReadInt32();
//                                        byte[] addressBytes = message.ReadBytes(byteList);
//                                        var ip = new IPAddress(addressBytes);
//                                        var port = message.ReadInt32();

//                                        IPEndPoint endpoint = new IPEndPoint(ip, port);
//                                        server.Server.Connect(endpoint);
//                                        server.ClientConnections.Add(endpoint);
//                                        Console.WriteLine("Client {0} connected to server", endpoint.Address);
//                                        break;
//                                    case (NetConnectionStatus.Disconnected):

//                                        var clientConnection = server.ClientConnections.FirstOrDefault(c => c.Address == message.ReadIPEndPoint().Address);
//                                        server.Server.GetConnection(clientConnection).Disconnect("Client disconnected");
//                                        server.ClientConnections.Remove(clientConnection);
//                                        Console.WriteLine("Client {0} disconnected", clientConnection.Address);
//                                        break;
//                                }
//                                break;

//                            case NetIncomingMessageType.DebugMessage:
//                                // For debugging

//                                Console.WriteLine(message.ReadString());
//                                break;

//                            /* .. */
//                            default:
//                                Console.WriteLine("unhandled message with type: "
//                                    + message.MessageType);
//                                break;
//                        }
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// Sends a message to a specific client
//        /// </summary>
//        /// <param name="message"></param>
//        /// <param name="serverId"></param>
//        /// <param name="clientConnection"></param>
//        public void SendMessage(ComponentManager componentManager, string message, int serverId, NetConnection clientConnection)
//        {
//            var serverEntity = componentManager.GetEntity(serverId);
//            var serverC = componentManager.GetComponent<ServerNetworkComponent>(serverEntity);

//            var peerMessage = serverC.Server.CreateMessage();
//            peerMessage.Write(message);

//            serverC.Server.SendMessage(peerMessage, clientConnection, NetDeliveryMethod.ReliableOrdered);
//        }

//        /// <summary>
//        /// Sends a message to all clients
//        /// </summary>
//        /// <param name="message"></param>
//        /// <param name="serverId"></param>
//        /// <param name="clientConnections"></param>
//        public void SendMessage(ComponentManager componentManager, string message, int serverId, List<NetConnection> clientConnections)
//        {
//            var serverEntity = componentManager.GetEntity(serverId);
//            var serverC = componentManager.GetComponent<ServerNetworkComponent>(serverEntity);

//            var peerMessage = serverC.Server.CreateMessage();
//            peerMessage.Write(message);

//            serverC.Server.SendMessage(peerMessage, clientConnections, NetDeliveryMethod.ReliableOrdered, 0);
//        }
//    }
//}


