using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Systems.Interfaces;
using ECS_Engine.Engine;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine.Systems
{
    public class KeyBoardSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager)
        {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<KeyBoardComponent>();

            if (components == null) return;
            foreach (KeyValuePair<Entity, IComponent> component in components)
            {
                KeyBoardComponent keyboardComp = (KeyBoardComponent)component.Value;
                UpdateState(keyboardComp);
                UpdateActionStates(keyboardComp);
            }
        }

        public void UpdateState(KeyBoardComponent keyboardComp)
        {
            keyboardComp.OldState = keyboardComp.NewState;
            keyboardComp.NewState = Keyboard.GetState();
        }

        public void UpdateActionStates(KeyBoardComponent keyboardComp)
        {
            foreach (string action in keyboardComp.Actions.Keys)
            {
                foreach (Keys key in keyboardComp.Actions[action])
                {
                    bool newState = keyboardComp.NewState.IsKeyDown(key);
                    bool oldState = keyboardComp.OldState.IsKeyDown(key);

                    if (newState && !oldState)
                    {
                        keyboardComp.ActionStates[action] = BUTTON_STATE.PRESSED;
                        break;
                    }
                    else if (newState && oldState)
                    {
                        keyboardComp.ActionStates[action] = BUTTON_STATE.HELD;
                        break;
                    }
                    else if (!newState && oldState)
                    {
                        keyboardComp.ActionStates[action] = BUTTON_STATE.RELEASED;
                        break;
                    }
                    else
                    {
                        keyboardComp.ActionStates[action] = BUTTON_STATE.NOT_PRESSED;
                    }
                }
            }
        }

    }
}
