using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Systems {
    public class RenderVertexSystem : IRenderSystem {
        public void Render(GameTime gameTime, GraphicsDeviceManager graphicsDevice, ComponentManager componentManager) {
            Dictionary<Entity, IComponent> cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;

            Dictionary<Entity, IComponent> components = componentManager.GetComponents<VertexPosColorComponent>();
            foreach(KeyValuePair<Entity, IComponent> component in components) {
                VertexPosColorComponent model = (VertexPosColorComponent)component.Value;
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                if(transform != default(TransformComponent)) {
                    BasicEffect effect = componentManager.GetComponent<BasicEffectComponent>(cam.First().Key).Effect;
                    effect.World = transform.World();
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        graphicsDevice.GraphicsDevice.DrawUserPrimitives(model.Type, model.Vertices, 0, model.Count());
                    }
                }
            }
        }
    }
}
