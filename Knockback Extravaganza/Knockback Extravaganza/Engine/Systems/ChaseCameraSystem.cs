using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine.Systems {
    public class ChaseCameraSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManage, SceneManager sceneManager) {
            var components = componentManager.GetComponents<CameraComponent>();
            if(components != null) {
                foreach(KeyValuePair<Entity, IComponent> component in components) {
                    ChaseCameraComponent chase = componentManager.GetComponent<ChaseCameraComponent>(component.Key);
                    if(chase != default(ChaseCameraComponent)) {
                        CameraComponent camera = (CameraComponent)component.Value;
                        TransformComponent cameraTransform = componentManager.GetComponent<TransformComponent>(component.Key);
                        TransformComponent targetTransform = componentManager.GetComponent<TransformComponent>(chase.Target);
                        camera.Up = targetTransform.Up;
                        camera.Target = targetTransform.Position + chase.TargetOffSet;
                        Vector3 offZ = -targetTransform.Forward * chase.Offset.Z;
                        Vector3 offX = targetTransform.Right * chase.Offset.X;
                        Vector3 offY = targetTransform.Up * chase.Offset.Y;
                        cameraTransform.Position = targetTransform.Position + offX + offY + offZ;
                    }
                }
            }
        }
    }
}
