using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component;

namespace ECS_Engine.Engine.Systems {
    public class CameraSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager) {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<CameraComponent>();
            if (components != null) {
                foreach (KeyValuePair<Entity, IComponent> cam in components) {
                    TransformComponent transform = componentManager.GetComponent<TransformComponent>(cam.Key);
                    if (transform != default(TransformComponent)) {
                        CameraComponent camera = (CameraComponent)cam.Value;
                        camera.View = Matrix.CreateLookAt(transform.Position, camera.Target, camera.Up);
                    }
                }
            }
        }
    }
}
