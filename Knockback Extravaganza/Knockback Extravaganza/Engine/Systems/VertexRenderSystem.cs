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


//TODO: HANDLES TEXTURES DAMN IT!
namespace ECS_Engine.Engine.Systems {
    /// <summary>
    /// Handles rendering for the Vertex.
    /// </summary>
    /// <typeparam name="T">Is of type IVertexType</typeparam>
    public class VertexRenderSystem<T> : IRenderSystem where T : struct, IVertexType{
        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManager sceneManager) {
            var cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;

            var components = componentManager.GetComponents<VertexComponent<T>>();
            foreach(KeyValuePair<Entity, IComponent> component in components) {
                VertexComponent<T> model = (VertexComponent<T>)component.Value;
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                if(transform != default(TransformComponent)) {
                    BasicEffect effect = model.Effect;
                    effect.World = transform.GetWorld(transform.UpdateBuffer);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        graphicsDevice.DrawUserPrimitives(model.Type, model.Vertices, 0, model.Count);
                    }
                }
            }
        }
    }
}
