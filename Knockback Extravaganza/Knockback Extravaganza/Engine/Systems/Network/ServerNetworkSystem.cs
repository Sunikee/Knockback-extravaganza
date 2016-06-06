﻿//using ECS_Engine.Engine.Component.Interfaces;
//using ECS_Engine.Engine.Component.Network;
//using ECS_Engine.Engine.Managers;
//using ECS_Engine.Engine.Systems.Interfaces;
//using Lidgren.Network;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
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

//                HostIp = GetLocalIP(),
//                Port = 14242,
//                Config = new NetPeerConfiguration("KnockbackGame") { Port = port, MaximumConnections = 4 },
//            };
//            serverC.Config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
//            serverC.Server = new NetServer(serverC.Config);
//            serverC.Server.Start();

//            Console.WriteLine("Server started");
//            componentManager.AddComponent(serverEntity, serverC);
//        }

//        /// <summary>
//        /// Starts the server and runs the loop
//        /// </summary>
//        /// <param name="componentManager"></param>
//        public void Run(ComponentManager componentManager)
//        {
//            var components = componentManager.GetComponents<ServerNetworkComponent>();
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
//                                var data = message.ReadInt32();
//                                break;

//                            case NetIncomingMessageType.StatusChanged:

//                            case NetIncomingMessageType.DiscoveryRequest:
//                                NetOutgoingMessage response = server.Server.CreateMessage();
//                                response.Write("My server name");
//                                //Handle connection

//                                // Send the response to the sender of the request
//                                server.Server.SendDiscoveryResponse(response, message.SenderEndPoint);
//                                break;

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

//                            default:
//                                Console.WriteLine("unhandled message with type: "
//                                    + message.MessageType);
//                                break;
//                        }
//                    }
//                }
//            }
//        }
//        public IPAddress GetLocalIP()
//        {
//            var host = Dns.GetHostEntry(Dns.GetHostName());

//            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
//        }
//    }
//}


