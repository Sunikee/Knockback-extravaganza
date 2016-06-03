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

            if (currScene.Name == "multiplayerScene")
            {
                RenderModels(gameTime, graphicsDevice, componentManager);
            }
            else RenderScenes(graphicsDevice, sceneManager, currScene, componentManager);
        }
        public void RenderScenes(GraphicsDevice graphicsDevice, SceneManager sceneManager, Scene currScene, ComponentManager componentManager)
        {
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
                    //  if (currScene.Name != "hostScene")
                    //     {
                    foreach (var choice in currScene.menuChoices)
                    {
                        if (menu.ActiveChoice == currScene.menuChoices.FindIndex(i => i == choice))
                            color = menu.ActiveColor;
                        currScene.SpriteBatch.DrawString(currScene.Font, choice, new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth * 0.5f - currScene.Font.MeasureString(choice).X * 0.5f, spacing), color);
                        color = menu.InactiveColor;
                        spacing += menu.MenuChoicesSpacing;
                    }
                    currScene.SpriteBatch.End(); 
                     //}
                    //else
                    //{
                        //    var _rectangle = new Rectangle { Height = 80, Width = 400 };
                        //    RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };
                        //    var dummyTexture = new Texture2D(graphicsDevice, 1, 1);
                        //    dummyTexture.SetData(new Color[] { Color.White });


                        //    currScene.SpriteBatch.End();


                        //    currScene.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                        //                      null, null, _rasterizerState);


                        //    currScene.SpriteBatch.Draw(dummyTexture, _rectangle, Color.White);
                        //    currScene.SpriteBatch.DrawString(currScene.Font, "Please insert your IP", Vector2.Zero, Color.Red);

                        //    //End the spritebatch
                        //    currScene.SpriteBatch.End();
                        //}
                        //currScene.SpriteBatch.Begin();
                        //foreach (var choice in currScene.menuChoices)
                        //{
                        //    if (menu.ActiveChoice == currScene.menuChoices.FindIndex(i => i == choice))
                        //        color = menu.ActiveColor;
                        //    currScene.SpriteBatch.DrawString(currScene.Font, choice, new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth * 0.5f - currScene.Font.MeasureString(choice).X * 0.5f, spacing), color);
                        //    color = menu.InactiveColor;
                        //    spacing += menu.MenuChoicesSpacing;
                    //}
                    currScene.SpriteBatch.End();
                
                }
            }
        }
        public void RenderModels(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager)
        {
            var cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;

            var components = componentManager.GetComponents<ModelComponent>();
            foreach (KeyValuePair<Entity, IComponent> component in components)
            {
                ModelComponent model = (ModelComponent)component.Value;
                ModelTransformComponent MeshTransform = componentManager.GetComponent<ModelTransformComponent>(component.Key);
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                MenuComponent menuC = componentManager.GetComponent<MenuComponent>(component.Key);

                if (MeshTransform != default(ModelTransformComponent) && transform != default(TransformComponent))
                {
                    foreach (ModelMesh mesh in model.Model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            CheckForTexture(model, effect);
                            effect.EnableDefaultLighting();
                            effect.View = camera.View;
                            effect.Projection = camera.Projection;
                            effect.World = MeshTransform.GetTransform(mesh.Name).ParentBone * MeshTransform.GetTransform(mesh.Name).GetWorld(MeshTransform.CurrentRenderBuffer) * transform.GetWorld(transform.CurrentRenderBuffer);
                        }
                        mesh.Draw();
                    }
                }
                else if (transform != default(TransformComponent))
                {

                    Matrix[] transforms = new Matrix[model.Model.Bones.Count()];
                    model.Model.CopyAbsoluteBoneTransformsTo(transforms);
                    foreach (ModelMesh mesh in model.Model.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
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
