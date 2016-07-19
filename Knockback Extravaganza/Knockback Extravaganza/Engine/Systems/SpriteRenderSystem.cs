using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ECS_Engine.Engine.Scenes;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine._2D_Components;

namespace ECS_Engine.Engine.Systems {


    /// <summary>
    /// Render all menues and sprites
    /// </summary>
    public class SpriteRenderSystem : IRenderSystem{

        SpriteBatch spriteBatch = null;

        public void Render(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManagerFacade sceneManager)
        {
            if (spriteBatch == null)
                spriteBatch = new SpriteBatch(graphicsDevice);

            spriteBatch.Begin();

            var textureComponents = componentManager.GetComponents<Texture2DComponent>();
            
            if (textureComponents != null) {
                foreach (var component in textureComponents) {
                    var texture = component.Value as Texture2DComponent;
                    var position = componentManager.GetComponent<Position2DComponent>(component.Key);
                    spriteBatch.Draw(texture.Texture, position.Postion, texture.Color);
                    // todo : This expand this to cover different senario. (Source and Dest rectangles)
                }
            }

            var spriteStringComps = componentManager.GetComponents<SpriteTextComponent>();

            if(spriteStringComps != null) {
                foreach (var comp in spriteStringComps) {
                    var spriteString = comp.Value as SpriteTextComponent;
                    var position = componentManager.GetComponent<Position2DComponent>(comp.Key);
                    spriteBatch.DrawString(spriteString.SpriteFont, spriteString.Text, position.Postion, spriteString.Color);
                    // todo : Expand to handles different senarios. (overload methods)
                }
            }

            spriteBatch.End();
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            /*var menuComponents = componentManager.GetComponents<MenuComponent>();
            if (menuComponents != null) {
                foreach (KeyValuePair<Entity, IComponent> component in menuComponents) {
                    KeyBoardComponent menuKeys = componentManager.GetComponent<KeyBoardComponent>(component.Key);
                    MenuComponent menu = component.Value as MenuComponent;

                    spriteBatch.Begin();
                    spriteBatch.Draw(currScene.Background, new Vector2(0), Color.Green);

                    var spacing = menu.MenuChoicesSpacing;
                    var color = menu.InactiveColor;

                    foreach (var choice in currScene.menuChoices) {
                        if (menu.ActiveChoice == currScene.menuChoices.FindIndex(i => i == choice))
                            color = menu.ActiveColor;
                        spriteBatch.DrawString(currScene.Font, choice, new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth * 0.5f - currScene.Font.MeasureString(choice).X * 0.5f, spacing), color);
                        color = menu.InactiveColor;
                        spacing += menu.MenuChoicesSpacing;
                    }
                    spriteBatch.End();
                    graphicsDevice.DepthStencilState = DepthStencilState.Default;
                }
            }
            else {
                var score = "Your score is: " + (int)sceneManager.GetScene("singlePlayerScene").TimePlayed;
                spriteBatch.Begin();
                spriteBatch.Draw(currScene.Background, new Vector2(0), Color.Green);
                spriteBatch.DrawString(currScene.Font, score, new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth * 0.5f - currScene.Font.MeasureString(score).X * 0.5f, graphicsDevice.PresentationParameters.BackBufferHeight * 0.4f), Color.White);
                spriteBatch.End();
                graphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            */
        }
    }
}
