/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using ECS_Engine.Engine.Systems.Interfaces;
using ECS_Engine.Engine.Component.Network;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine.Systems.Network {
    class ClientNetworkSystem : INetworkSystem {

        /// <summary>
        /// Starts a client with the nessesary data
        /// </summary>
        /// <param name="componentManager"></param>
        /// <param name="hostIp"></param>
        /// <param name="port"></param>
        public void Start(ComponentManager componentManager, string hostIp, int port) {
            Entity ClientEntity = new Entity();
            ClientNetworkComponent clientC = new ClientNetworkComponent {
                HostIp = hostIp,
                Port = port,
                Config = new NetPeerConfiguration("KnockbackGame")
            };
            clientC.Client = new NetClient(clientC.Config);
            componentManager.AddComponent(ClientEntity, clientC);
        }

        /// <summary>
        /// Connects to a server and starts the running loop
        /// </summary>
        /// <param name="componentManager"></param>
        public void Run(ComponentManager componentManager) {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<ClientNetworkComponent>();
            if (components != null) {
                foreach (KeyValuePair<Entity, IComponent> comp in components) {
                    ClientNetworkComponent client = componentManager.GetComponent<ClientNetworkComponent>(comp.Key);
                    client.Client.Start();
                    client.Client.Connect(host: client.HostIp, port: client.Port);
                    Console.WriteLine("Client connected");

                    NetIncomingMessage message;
                    while ((message = client.Client.ReadMessage()) != null) {
                        switch (message.MessageType) {
                            case NetIncomingMessageType.Data:
                            // handle custom messages
                            var data = message.ReadInt32();
                            break;

                            case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            switch (message.SenderConnection.Status) {
                                // if (message.SenderConnection.Status == NetConnectionStatus.Connected)
                                ////Handle when client connects
                                //else
                                //Handle disconnect from server
                            }
                            break;

                            case NetIncomingMessageType.DebugMessage:
                            //For debugging

                            Console.WriteLine(message.ReadString());
                            break;


                            default:
                            Console.WriteLine("unhandled message with type: "
                                + message.MessageType);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientId"></param>
        public void SendMessage(ComponentManager componentManager, string message, int clientId) {
            var clientE = componentManager.GetEntity(clientId);
            var clientC = componentManager.GetComponent<ClientNetworkComponent>(clientE);

            var peerMessage = clientC.Client.CreateMessage();
            peerMessage.Write(message);

            clientC.Client.SendMessage(peerMessage, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Parses a message from the server to 
        /// the correct format
        /// </summary>
        /// <param name="messageManager"></param>
        /// <param name="message"></param>
        public void ParseToMessage(MessageManager messageManager, string message) {
            string[] parameters = message.Split(',');

            var sender = parameters[0];
            var receiver = parameters[1];
            var activateInSeconds = parameters[2];
            var msg = parameters[3];
            messageManager.RegMessage(int.Parse(sender), int.Parse(receiver), int.Parse(activateInSeconds), msg);
        }

        /// <summary>
        /// Parses a message to send to the server
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string ParseToString(Message msg) {
            var sendMessage = msg.sender.ToString() + ",";
            sendMessage = string.Concat(sendMessage, msg.receiver.ToString() + ",");
            sendMessage = string.Concat(sendMessage, msg.receiver.ToString() + ",");
            sendMessage = string.Concat(sendMessage, msg.msg + ",");

            //sendMessage = msg.sender + "," + msg.receiver + "," + msg.activateInSeconds + "," + msg.msg;

            return sendMessage;

        }
    }
}
*/