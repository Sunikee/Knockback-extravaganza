using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component.Network;
using ECS_Engine.Engine.Component;

namespace ECS_Engine.Engine.Systems.Network {
    public class NetworkTransformSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager) {
            var components = componentManager.GetComponents<NetworkRecieveTransformComponent>();

            if(components != null) {
                foreach(var component in components) {
                    TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                    if(transform != null) {
                        var netTransform = component.Value as NetworkRecieveTransformComponent;
                        transform.Position = netTransform.Position;
                        transform.Scale = netTransform.Scale;
                        transform.Rotation = netTransform.Rotation;
                    }
                }
            }

            components = componentManager.GetComponents<NetworkSendTransformComponent>();

            if (components != null) {
                foreach (var component in components) {
                    TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                    if (transform != null) {
                        var netTransform = component.Value as NetworkSendTransformComponent;
                        netTransform.Position = transform.Position;
                        netTransform.Scale = transform.Scale;
                        netTransform.Rotation = transform.Rotation;
                    }
                }
            }
        }
    }
}
