using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component;

namespace ECS_Engine.Engine.Systems {
    public class VertexBufferRenderSystem<T> : IRenderSystem where T : struct, IVertexType{
        public void Render(GameTime gameTime, GraphicsDeviceManager graphicsDevice, ComponentManager componentManager) {
            Dictionary<Entity, IComponent> cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;

            Dictionary<Entity, IComponent> components = componentManager.GetComponents<VertexBufferComponent<T>>();
            foreach (KeyValuePair<Entity, IComponent> component in components) {
                VertexBufferComponent<T> model = (VertexBufferComponent<T>)component.Value;
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                if (transform != default(TransformComponent)) {
                    BasicEffect effect = model.Effect;
                    effect.World = transform.World;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        graphicsDevice.GraphicsDevice.SetVertexBuffer(model.VertexBuffer);
                        graphicsDevice.GraphicsDevice.DrawPrimitives(model.Type, 0, model.Count);
                    }
                }
            }
        }
    }
}
