using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;

namespace GameEngine
{
    public class GamePadComponent : IComponent
    {
        

        public PlayerIndex PlayerIndex { get; set; }
        public Dictionary<string, List<Buttons>> Actions{ get; set; }
        public Dictionary<string, BUTTON_STATE> ActionStates { get; set; }
        public GamePadState NewState { get; set; }
        public GamePadState OldState { get; set; }

        public GamePadComponent() : this(PlayerIndex.One)
        {
        }

        public GamePadComponent(PlayerIndex playerIndex)
        {
            ActionStates = new Dictionary<string, BUTTON_STATE>();
            Actions = new Dictionary<string, List<Buttons>>();
            PlayerIndex = playerIndex;
        }


        public void AddButtonToAction(string action, Buttons button)
        {
            if (!Actions.ContainsKey(action))
            {
                Actions[action] = new List<Buttons>();
                ActionStates[action] = BUTTON_STATE.NOT_PRESSED;
            }
            Actions[action].Add(button);
        }


        public void RemoveButtonFromAction(string action, Buttons button)
        {
            if (Actions.ContainsKey(action))
            {
                Actions[action].Remove(button);
            }
        }

        public BUTTON_STATE? GetActionState(string action)
        {
            if (ActionStates.ContainsKey(action))
            {
                return ActionStates[action];
            }
            return null;
        }
        
        public void SetAction(string action, BUTTON_STATE state)
        {
            ActionStates[action] = state;
        }

        public Vector2 GetLeftThumbStick(bool leftThumb, float deadZone)
        {
            Vector2 vec2;
            if (leftThumb)
            {
                vec2 = NewState.ThumbSticks.Left;
            }
            else
            {
                vec2 = NewState.ThumbSticks.Right;
            }

            if (vec2.X < deadZone)
            {
                vec2.X = 0;
            }
            if (vec2.Y < deadZone)
            {
                vec2.Y = 0;
            }

            return vec2;
        }


    }
}
