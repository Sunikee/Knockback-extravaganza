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

namespace ECS_Engine.Engine.Systems
{
    public class MovementSystem : IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager)
        {
            HandleInput(gametime, componentManager);

            //throw new NotImplementedException();
        }

        public void HandleInput(GameTime gametime, ComponentManager componentManager)
        {
            Dictionary<Entity, IComponent> kComponents = componentManager.GetComponents<KeyBoardComponent>();
            Vector3 changeRotation = new Vector3(0, 0, 0);

            foreach (KeyValuePair<Entity, IComponent> component in kComponents)
            {
                KeyBoardComponent keyboardComp = (KeyBoardComponent)component.Value;
                TransformComponent tc = componentManager.GetComponent<TransformComponent>(component.Key);
                foreach (KeyValuePair<string, BUTTON_STATE> actionState in keyboardComp.ActionStates)
                {
                    if ((actionState.Key.Equals("Down") && actionState.Value.Equals(BUTTON_STATE.PRESSED)) || (actionState.Key.Equals("Down") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        //Console.WriteLine("Trycker på W");
                        tc.Rotation += new Vector3(-.1f, 0, 0);
                    }
                    if (actionState.Key.Equals("Up") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("Up") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        //Console.WriteLine("Trycker på S");
                        tc.Rotation += new Vector3(.1f, 0, 0);
                    }
                    if (actionState.Key.Equals("Right") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("Right") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        //Console.WriteLine("Trycker på D");
                        tc.Rotation += new Vector3(0, -.1f, 0);
                    }
                    if (actionState.Key.Equals("Left") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("Left") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        //Console.WriteLine("Trycker på A");
                        tc.Rotation += new Vector3(0, .1f, 0);
                    }
                    if ((actionState.Key.Equals("Move") && actionState.Value.Equals(BUTTON_STATE.PRESSED)) || (actionState.Key.Equals("Move") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        //Console.WriteLine("Trycker på Space");
                        tc.Position += tc.Forward * (float)gametime.ElapsedGameTime.TotalSeconds * 10;
                    }
                }
            }
        }
    }
}
