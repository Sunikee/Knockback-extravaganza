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

namespace ECS_Engine.Engine.Systems {

    /// <summary>
    /// Render all menues and sprites
    /// </summary>
    public class SpriteRender{
        public void RenderSprites(GameTime gameTime, GraphicsDevice graphicsDevice, ComponentManager componentManager, SceneManagerFacade sceneManager) 
        {
            /*
            Scene currScene = sceneManager.GetCurrentScene();
            var menuComponents = componentManager.GetComponents<MenuComponent>();
            if (menuComponents != null && sceneManager.GetCurrentScene().Name != "endScene") {
                foreach (KeyValuePair<Entity, IComponent> component in menuComponents) {
                    KeyBoardComponent menuKeys = componentManager.GetComponent<KeyBoardComponent>(component.Key);
                    MenuComponent menu = component.Value as MenuComponent;

                    currScene.SpriteBatch.Begin();
                    currScene.SpriteBatch.Draw(currScene.Background, new Vector2(0), Color.Green);

                    var spacing = menu.MenuChoicesSpacing;
                    var color = menu.InactiveColor;

                    foreach (var choice in currScene.menuChoices) {
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
            else {
                var score = "Your score is: " + (int)sceneManager.GetScene("singlePlayerScene").TimePlayed;
                currScene.SpriteBatch.Begin();
                currScene.SpriteBatch.Draw(currScene.Background, new Vector2(0), Color.Green);
                currScene.SpriteBatch.DrawString(currScene.Font, score, new Vector2(graphicsDevice.PresentationParameters.BackBufferWidth * 0.5f - currScene.Font.MeasureString(score).X * 0.5f, graphicsDevice.PresentationParameters.BackBufferHeight * 0.4f), Color.White);
                currScene.SpriteBatch.End();
                graphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            */
        }
    }
}
