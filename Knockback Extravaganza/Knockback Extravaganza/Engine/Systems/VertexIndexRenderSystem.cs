﻿using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Systems {
    /// <summary>
    /// Handles rendering for a indexed Vertex.
    /// </summary>
    /// <typeparam name="T">Is of type IVertexType</typeparam>
    public class VertexIndexRenderSystem<T> : IRenderSystem where T : struct, IVertexType {
        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManagerFacade sceneManager) {
            var cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;

            var components = componentManager.GetComponents<VertexIndexComponent<T>>();
            foreach (KeyValuePair<Entity, IComponent> component in components) {
                VertexIndexComponent<T> model = (VertexIndexComponent<T>)component.Value;
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                if (transform != default(TransformComponent)) {
                    BasicEffect effect = model.Effect;
                    effect.World = transform.GetWorld(transform.UpdateBuffer);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                        pass.Apply();
                        graphicsDevice.DrawUserIndexedPrimitives(model.Type, model.Vertices, 0, model.Vertices.Length, model.Indices, 0, model.Count);
                    }
                }
            }
        }
    }
}
