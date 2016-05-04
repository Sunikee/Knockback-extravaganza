using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;

namespace ECS_Engine.Engine.Component {
    public class MouseComponent : IComponent
    {
        public MouseState NewState { get; set; }
        public MouseState OldState { get; set; }
        public Dictionary<string, BUTTON_STATE> actionStates = new Dictionary<string, BUTTON_STATE>();

        public MouseComponent()
        {
            actionStates.Add("LeftButton", BUTTON_STATE.NOT_PRESSED);
            actionStates.Add("MiddleButton", BUTTON_STATE.NOT_PRESSED);
            actionStates.Add("RightButton", BUTTON_STATE.NOT_PRESSED);
        }

        public void SetActionState(string action, BUTTON_STATE state)
        {
            if (actionStates.ContainsKey(action))
            {
                actionStates[action] = state;
            }
        }

        public BUTTON_STATE? GetActionState(string action)
        {
            if (actionStates.ContainsKey(action))
            {
                return actionStates[action];
            }
            return null;
        }

        public int GetX()
        {
            return NewState.X;
        }

        public int GetY()
        {
            return NewState.Y;
        }

        public int GetDeltaX() {
            return OldState.X - NewState.X;
        }

        public int GetDeltaY() {
            return OldState.Y -NewState.Y;
        }

        public int GetScrollValue()
        {
            return NewState.ScrollWheelValue;
        }
    }
}
