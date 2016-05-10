using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Systems {
    public class FreeCameraSystem : IUpdateSystem {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager) {
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
                        UpdateViewMatrix(free, camera, cameraTransform);
                    } 
                }
            }
        }

        private void ProcessInput(float amount, MouseComponent mouse, FreeCameraComponent free) {
            if (mouse.NewState != mouse.OldState)
            {
                free.OriginalMouseState = new Vector2(free.GraphicsDevice.Viewport.Width / 2, free.GraphicsDevice.Viewport.Height / 2);
                
                free.LeftRightRot -= free.RotationSpeed * (mouse.GetX() - free.OriginalMouseState.X) * amount;
                free.UpDownRot -= free.RotationSpeed * (mouse.GetY() - free.OriginalMouseState.Y) * amount;
                if (free.UpDownRot > Math.PI * 0.5) {
                    free.UpDownRot = (float)Math.PI * 0.5f;
                }
                if (free.UpDownRot < Math.PI * -0.5) {
                    free.UpDownRot = (float)Math.PI * -0.5f;
                }
                
                if (free.Game.IsActive) {
                    Mouse.SetPosition(free.GraphicsDevice.Viewport.Width / 2, free.GraphicsDevice.Viewport.Height / 2);
                }

            }
        }

        private void UpdateViewMatrix(FreeCameraComponent free, CameraComponent camera, TransformComponent cameraTransform) {
            Matrix cameraRotation = Matrix.CreateRotationX(free.UpDownRot) * Matrix.CreateRotationY(free.LeftRightRot);

            Vector3 cameraOriginalTarget = cameraTransform.Forward;
            Vector3 cameraOriginalUpVector = cameraTransform.Up;

            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraTransform.Position + cameraRotatedTarget;

            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            cameraTransform.Rotation = new Vector3(free.UpDownRot, free.LeftRightRot, 0);

            //camera.View = Matrix.CreateLookAt(cameraTransform.Position, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}
