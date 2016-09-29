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
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {
            var menuComponents = componentManager.GetComponents<MenuComponent>();

            if(menuComponents != null) {
                foreach (var comp in menuComponents) {
                    var menu = comp.Value as MenuComponent;
                    var keyboard = componentManager.GetComponent<KeyBoardComponent>(comp.Key);

                    if (keyboard.GetActionState("Select") == BUTTON_STATE.PRESSED)
                        sceneManager.ChangeScene("SinglePlayer");
                    else if (keyboard.GetActionState("Up") == BUTTON_STATE.PRESSED)
                        sceneManager.ChangeScene("MPClient");
                    else if (keyboard.GetActionState("Down") == BUTTON_STATE.PRESSED)
                        sceneManager.ChangeScene("MPHost");
                }
            }
        }
    }
}
