using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;

namespace GameEngine
{
    public class KeyBoardComponent : IComponent
    {
        public Dictionary<string, List<Keys>> Actions { get; set; }
        public Dictionary<string, BUTTON_STATE> ActionStates { get; set; }
        public KeyboardState NewState { get; set; }
        public KeyboardState OldState { get; set; }

        public KeyBoardComponent()
        {
            Actions = new Dictionary<string, List<Keys>>();
            ActionStates = new Dictionary<string, BUTTON_STATE>();
        }

        public void AddKeyToAction(string action, Keys key)
        {
            if (!Actions.ContainsKey(action))
            {
                Actions[action] = new List<Keys>();
                ActionStates[action] = BUTTON_STATE.NOT_PRESSED;
            }
            Actions[action].Add(key);
        }

        public void RemoveKeyFromAction(string action, Keys key)
        {
            if (Actions.ContainsKey(action))
            {
                Actions[action].Remove(key);
            }
        }

        public BUTTON_STATE? GetActionState(string action)
        {
            if(ActionStates.ContainsKey(action))
            {
                return ActionStates[action];
            }
            return null;
        }

        public void SetAction(string action, BUTTON_STATE state)
        {
            ActionStates[action] = state;
        }

    }
}
