using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component;

namespace Game.Source.Systems
{
    public class GameOverSystem : IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {
            var gameOverComponents = componentManager.GetComponents<GameOverComponent>();
            
            if (gameOverComponents != null)
            {
                foreach (var comp in gameOverComponents)
                {
                    if (comp.Key.Tag == "gameover")
                    {
                        var menu = comp.Value as MenuComponent;
                        var keyboard = componentManager.GetComponent<KeyBoardComponent>(comp.Key);

                        if (keyboard.GetActionState("Restart") == BUTTON_STATE.PRESSED)
                            sceneManager.ChangeScene("MainMenu");                            
                    }
                }
            }
        }
    }
}
