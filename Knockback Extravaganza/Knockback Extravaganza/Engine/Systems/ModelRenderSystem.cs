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

namespace ECS_Engine.Engine.Systems
{
    public class ModelRenderSystem : IRenderSystem
    {
        public static float r = 0.1f;



        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManager sceneManager)
        {
            Scene currScene = sceneManager.GetCurrentScene();

            if (currScene.Name == "singlePlayerScene")
            {
                RenderModels(gameTime, graphicsDevice, componentManager, sceneManager);
            }
            else RenderScenes(graphicsDevice, componentManager, sceneManager);
        }
        public void RenderScenes(GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManager sceneManager)
        {
            Scene currScene = sceneManager.GetCurrentScene();
            var menuComponents = componentManager.GetComponents<MenuComponent>();
            if (menuComponents != null)
            {
                foreach (KeyValuePair<Entity, IComponent> component in menuComponents)
                {
                    KeyBoardComponent menuKeys = componentManager.GetComponent<KeyBoardComponent>(component.Key);
                    MenuComponent menu = component.Value as MenuComponent;

                    currScene.SpriteBatch.Begin();
                    currScene.SpriteBatch.Draw(currScene.Background, new Vector2(0), Color.Green);

                    var spacing = menu.MenuChoicesSpacing;
                    var color = menu.InactiveColor;
           
                    foreach (var choice in currScene.menuChoices)
                    {
                        if (menu.ActiveChoice == currScene.menuChoices.FindIndex(i => i == choice))
                            color = menu.ActiveColor;
                        currScene.SpriteBatch.DrawString(currScene.Font, choice, new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth * 0.5f - currScene.Font.MeasureString(choice).X * 0.5f, spacing), color);
                        color = menu.InactiveColor;
                        spacing += menu.MenuChoicesSpacing;
                    }
                    currScene.SpriteBatch.End();
                    graphicsDevice.DepthStencilState = DepthStencilState.Default;
      
                }
            }
        }
        public void RenderModels(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManager sceneManager)
        {
            var cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;
            BoundingFrustum frustum = new BoundingFrustum(camera.View * camera.Projection);
            var components = componentManager.GetComponents<ModelComponent>();
            foreach (KeyValuePair<Entity, IComponent> component in components)
            {
                ModelComponent model = (ModelComponent)component.Value;
                ModelTransformComponent MeshTransform = componentManager.GetComponent<ModelTransformComponent>(component.Key);
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                MenuComponent menuC = componentManager.GetComponent<MenuComponent>(component.Key);
                CollisionComponent collisionC = componentManager.GetComponent<ActiveCollisionComponent>(component.Key);
                if(collisionC == null) {
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
                    } else if (transform != default(TransformComponent)) {

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
            var currScene = sceneManager.GetCurrentScene();
            if (sceneManager.GetCurrentScene().Name == "singlePlayerScene")
            {
                currScene.TimePlayed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                currScene.SpriteBatch.Begin();
                currScene.SpriteBatch.DrawString(currScene.Font, "Time: " + (int)currScene.TimePlayed, new Vector2(10), Color.White);
                currScene.SpriteBatch.End();
            }
        }

        private void CheckForTexture(ModelComponent model, BasicEffect effect)
        {
            if (model.Texture != null)
            {
                effect.TextureEnabled = true;
                effect.Texture = model.Texture;
            }
        }

    }
}
