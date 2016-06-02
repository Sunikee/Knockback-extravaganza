using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Systems.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine;
using ECS_Engine.Engine.Component.Interfaces;
using GameEngine;

namespace ECS_Engine.Engine.Systems
{
    public class GamePadSystem : IUpdateSystem
    {
        

        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager)
        {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<GamePadComponent>();
            if (components == null) {
                foreach (KeyValuePair<Entity, IComponent> component in components) {
                    GamePadComponent gamePadComp = (GamePadComponent)component.Value;
                    UpdateStates(gamePadComp);
                    UpdateActionStates(gamePadComp);
                }
            }
        }

        private void UpdateStates(GamePadComponent gamePadComp)
        {
            gamePadComp.OldState = gamePadComp.NewState;
            gamePadComp.NewState = GamePad.GetState(gamePadComp.PlayerIndex);
        }

        private void UpdateActionStates(GamePadComponent gamePadComp)
        {
            foreach(string key in gamePadComp.Actions.Keys)
            {
                foreach (Buttons button in gamePadComp.Actions[key])
                {
                    bool newState = gamePadComp.NewState.IsButtonDown(button);
                    bool oldState = gamePadComp.OldState.IsButtonDown(button);
  
                    if (newState && !oldState)
                    {
                        gamePadComp.ActionStates[key] = BUTTON_STATE.PRESSED;
                        break;
                    }
                    else if (newState && oldState)
                    {
                        gamePadComp.ActionStates[key] = BUTTON_STATE.HELD;
                        break;
                    }
                    else if (!newState && oldState)
                    {
                        gamePadComp.ActionStates[key] = BUTTON_STATE.RELEASED;
                        break;
                    }
                    else
                    {
                        gamePadComp.ActionStates[key] = BUTTON_STATE.NOT_PRESSED;
                    }
                }
            }
        }

    }
}
