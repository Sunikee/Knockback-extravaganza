using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Systems.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine;
using GameEngine;

namespace ECS_Engine.Engine.Systems
{
    public class MouseSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager)
        {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<MouseComponent>();
            if (components != null)
            {
                foreach (KeyValuePair<Entity, IComponent> component in components)
                {
                    MouseComponent mouseComp = (MouseComponent)component.Value;
                    UpdateState(mouseComp);
                    updateActionStates(mouseComp);
                }
            }
        }

        public void UpdateState(MouseComponent mouseComp)
        {
            mouseComp.OldState = mouseComp.NewState;
            mouseComp.NewState = Mouse.GetState();
        }

        public void updateActionStates(MouseComponent mouseComp)
        {
            MouseState newState = mouseComp.NewState;
            MouseState oldState = mouseComp.OldState;
            // LeftButton
            UpdateMouseButton(mouseComp, newState.LeftButton, oldState.LeftButton, "LeftButton");
            // MiddleButton
            UpdateMouseButton(mouseComp, newState.MiddleButton, oldState.MiddleButton, "MiddleButton");
            // RightButton
            UpdateMouseButton(mouseComp, newState.RightButton, oldState.RightButton, "RightButton");
        }

        private void UpdateMouseButton(MouseComponent mouseComp, ButtonState newState, ButtonState oldState, string button)
        {
            if (newState == ButtonState.Pressed && oldState != ButtonState.Pressed)
            {
                mouseComp.actionStates[button] = BUTTON_STATE.PRESSED;
            }
            else if (newState == ButtonState.Pressed && oldState == ButtonState.Pressed)
            {
                mouseComp.actionStates[button] = BUTTON_STATE.HELD;
            }
            else if (newState != ButtonState.Pressed && oldState == ButtonState.Pressed)
            {
                mouseComp.actionStates[button] = BUTTON_STATE.RELEASED;
            }
            else
            {
                mouseComp.actionStates[button] = BUTTON_STATE.NOT_PRESSED;
            }
        }
    }

}
