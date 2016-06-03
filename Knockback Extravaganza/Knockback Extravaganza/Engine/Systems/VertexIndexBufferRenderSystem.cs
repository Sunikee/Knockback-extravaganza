using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Abstract_Classes;

namespace ECS_Engine.Engine.Systems {
    public class VertexIndexBufferRenderSystem<T> : IRenderSystem where T : struct, IVertexType {
        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManager sceneManager) {
            var cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;

            var components = componentManager.GetComponents<VertexIndexBufferComponent<T>>();
            foreach (KeyValuePair<Entity, IComponent> component in components) {
                VertexIndexBufferComponent<T> model = (VertexIndexBufferComponent<T>)component.Value;
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                if (transform != default(TransformComponent)) {
                    BasicEffect effect = model.Effect;
                    effect.World = transform.GetWorld(transform.CurrentRenderBuffer);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        graphicsDevice.Indices = model.IndexBuffer;
                        graphicsDevice.SetVertexBuffer(model.VertexBuffer);
                        graphicsDevice.DrawIndexedPrimitives(model.Type, 0, 0, model.Count);
                    }
                }
            }
        }
    }
}
