using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Scenes;
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
    /// Renders all object with models on them
    /// </summary>
    public class ModelRenderSystem : IRenderSystem{

        public static float r = 0.1f;

            public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManagerFacade sceneManager) {
            var cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;
            BoundingFrustum frustum = new BoundingFrustum(camera.View * camera.Projection);
            var components = componentManager.GetComponents<ModelComponent>();

            foreach (KeyValuePair<Entity, IComponent> component in components) {
                ModelComponent model = (ModelComponent)component.Value;
                ModelTransformComponent MeshTransform = componentManager.GetComponent<ModelTransformComponent>(component.Key);
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                MenuComponent menuC = componentManager.GetComponent<MenuComponent>(component.Key);
                CollisionComponent collisionC = componentManager.GetComponent<ActiveCollisionComponent>(component.Key);
                if (collisionC == null) {
                    collisionC = componentManager.GetComponent<PassiveCollisionComponent>(component.Key);
                }

                if (collisionC == null || frustum.Intersects(collisionC.BoundingBox)) {

                    if (MeshTransform != default(ModelTransformComponent) && transform != default(TransformComponent)) {
                        foreach (ModelMesh mesh in model.Model.Meshes) {
                            foreach (BasicEffect effect in mesh.Effects) {
                                CheckForTexture(model, effect);
                                effect.EnableDefaultLighting();
                                effect.View = camera.View;
                                effect.Projection = camera.Projection;
                                effect.World = MeshTransform.GetTransform(mesh.Name).ParentBone * MeshTransform.GetTransform(mesh.Name).GetWorld(MeshTransform.CurrentRenderBuffer) * transform.GetWorld(transform.CurrentRenderBuffer);
                            }
                            mesh.Draw();
                        }
                    }
                    else if (transform != default(TransformComponent)) {

                        Matrix[] transforms = new Matrix[model.Model.Bones.Count()];
                        model.Model.CopyAbsoluteBoneTransformsTo(transforms);
                        foreach (ModelMesh mesh in model.Model.Meshes) {
                            foreach (BasicEffect effect in mesh.Effects) {
                                CheckForTexture(model, effect);
                                effect.EnableDefaultLighting();
                                effect.View = camera.View;
                                effect.Projection = camera.Projection;
                                effect.World = transforms[mesh.ParentBone.Index] * transform.GetWorld(transform.CurrentRenderBuffer);
                            }
                            mesh.Draw();
                        }
                    }
                }
            }
            DrawScore(graphicsDevice, componentManager);
        }

        private void CheckForTexture(ModelComponent model, BasicEffect effect) {
            if (model.Texture != null) {
                effect.TextureEnabled = true;
                effect.Texture = model.Texture;
            }
        }

        public void DrawScore(GraphicsDevice graphicsDevice, ComponentManager componentManager)
        {
            var scoreTimeComponents = componentManager.GetComponents<ScoreTimeComponent>();
            foreach(KeyValuePair<Entity, IComponent> scoreValuePair in scoreTimeComponents)
            {
                ScoreTimeComponent scoreTComponent = componentManager.GetComponent<ScoreTimeComponent>(scoreValuePair.Key);
                scoreTComponent.spriteBatch.Begin();
                scoreTComponent.spriteBatch.DrawString(scoreTComponent.spriteFont, "Score: " + scoreTComponent.TotalScore, new Vector2(25, 25), Color.Black);
                scoreTComponent.spriteBatch.End();
            }
        }

    }
}
