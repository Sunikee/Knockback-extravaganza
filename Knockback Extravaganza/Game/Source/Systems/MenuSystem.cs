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
using ECS_Engine.Engine.Scenes;
using ECS_Engine.Engine;

namespace Game.Source.Systems
{
    /// <summary>
    /// Handles updaing and moving between the different menues and their options.
    /// </summary>
    public class MenuSystem : IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager)
        {
            var menuComponents = componentManager.GetComponents<MenuComponent>();

            var currScene = sceneManager.GetCurrentScene();
            if (menuComponents != null && currScene.Name != "singlePlayerScene")
            {
                foreach (KeyValuePair<Entity, IComponent> component in menuComponents)
                {
                    KeyBoardComponent menuKeys = componentManager.GetComponent<KeyBoardComponent>(component.Key);
                    MenuComponent menu = component.Value as MenuComponent;

                    if (menuKeys.GetActionState("Up") == BUTTON_STATE.PRESSED && menu.ActiveChoice > 0)
                        menu.ActiveChoice--;
                    if (menuKeys.GetActionState("Down") == BUTTON_STATE.PRESSED && menu.ActiveChoice < currScene.menuChoices.Count - 1)
                        menu.ActiveChoice++;
                    if (menuKeys.GetActionState("Select") == BUTTON_STATE.PRESSED)
                    {
                        ChangeScene(sceneManager, menu);
                    }
                }
            }
        }
        public void ChangeScene(SceneManager sceneManager, MenuComponent menu)
        {
            var currScene = sceneManager.GetCurrentScene();
            if (currScene.Name == "startScene")
            {
                switch (menu.ActiveChoice)
                {
                    case 0:
                        sceneManager.SetCurrentScene(sceneManager.GetScene("singlePlayerScene").Name);
                        menu.ActiveChoice = 0;
                        break;
                    case 1:
                        sceneManager.SetCurrentScene(sceneManager.GetScene("connectionScene").Name);
                        menu.ActiveChoice = 0;
                        break;
                }
            }
            if (currScene.Name == "pauseScene")
            {
                switch (menu.ActiveChoice)
                {
                    case 0:
                        sceneManager.SetCurrentScene(sceneManager.GetScene("singlePlayerScene").Name);
                        menu.ActiveChoice = 0;
                        break;
                    case 1:

                        break;
                    case 2:
                        sceneManager.SetCurrentScene(sceneManager.GetScene("startScene").Name);
                        menu.ActiveChoice = 0;
                        break;
                }
            }
            if(currScene.Name == "connectionScene")
            {
                switch (menu.ActiveChoice)
                {
                    case 0:
                       //Host game
                        break;
                    case 1:
                        //Join Game
                        //menu.ActiveChoice = 0;
                        break;
                    case 2:
                        sceneManager.SetCurrentScene(sceneManager.GetScene("startScene").Name);
                        menu.ActiveChoice = 0;
                        break;
                }
            }
        }
    }
}
