using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Systems {
    public class FreeCameraSystem : IUpdateSystem {
        public void Update(GameTime gameTime, ComponentManager componentManager) {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<CameraComponent>();
            if(components != null) {
                foreach (KeyValuePair<Entity, IComponent> component in components) {
                    FreeCameraComponent free = componentManager.GetComponent<FreeCameraComponent>(component.Key);
                    if (free != default(FreeCameraComponent)) {
                        CameraComponent camera = (CameraComponent)component.Value;
                        TransformComponent cameraTransform = componentManager.GetComponent<TransformComponent>(component.Key);
                        camera.Up = cameraTransform.Up;
                        camera.Target = cameraTransform.Position + cameraTransform.Forward;
                        MouseComponent mouse = componentManager.GetComponent<MouseComponent>(component.Key);
                        float timeDifference = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        ProcessInput(timeDifference, mouse, free);
                    } 
                }
            }
        }

        private void ProcessInput(float amount, MouseComponent mouse, FreeCameraComponent free) {
            if (mouse.NewState != mouse.OldState)
            {
                free.leftRightRot -= free.rotationSpeed * mouse.GetDeltaX() * amount;
                free.upDownRot -= free.rotationSpeed * mouse.GetDeltaY() * amount;
                //Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);
            }
        }
    }
}
