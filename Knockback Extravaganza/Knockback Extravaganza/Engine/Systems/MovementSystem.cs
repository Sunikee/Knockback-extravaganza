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
        public float DashTimer = 1;
        public float DashTime = 1500;
        public void Update(GameTime gametime, ComponentManager componentManager)
        {
            HandleInput(gametime, componentManager);     
        }

        public void HandleInput(GameTime gametime, ComponentManager componentManager)
        {
            Dictionary<Entity, IComponent> kComponents = componentManager.GetComponents<KeyBoardComponent>();

            foreach (KeyValuePair<Entity, IComponent> component in kComponents)
            {
                KeyBoardComponent keyboardComp = (KeyBoardComponent)component.Value;
                TransformComponent tc = componentManager.GetComponent<TransformComponent>(component.Key);
                MovementComponent mc = componentManager.GetComponent<MovementComponent>(component.Key);
                PhysicsComponent pc = componentManager.GetComponent<PhysicsComponent>(component.Key);

                foreach (KeyValuePair<string, BUTTON_STATE> actionState in keyboardComp.ActionStates)
                {
                    if (actionState.Key.Equals("Forward") && actionState.Value.Equals(BUTTON_STATE.PRESSED))
                    {
                        //mc.Velocity += tc.Forward * mc.Speed;
                        //tc.Position = mc.Velocity;
                        //tc.Position += tc.Forward * mc.Speed;
                    }
                    //tc.Position += tc.Forward * mc.Speed;
                    if (actionState.Key.Equals("Forward") && actionState.Value.Equals(BUTTON_STATE.HELD))
                    {
                        mc.Speed += (float)gametime.ElapsedGameTime.TotalSeconds * mc.Speed * mc.Acceleration;
                        mc.Speed = 1;
                        if (mc.Speed > 3)
                            mc.Speed = 3;
                        //mc.Velocity += tc.Forward * mc.Speed;
                        //tc.Position = mc.Velocity;
                        //tc.Position += tc.Forward * mc.Speed;
                    }
                    tc.Position += tc.Forward * mc.Speed;
                    mc.Speed -= (float) gametime.ElapsedGameTime.TotalSeconds;
                    if (mc.Speed < 0)
                    {
                        mc.Speed = 0;
                    }
                    if (actionState.Key.Equals("Forward") && actionState.Value.Equals(BUTTON_STATE.RELEASED))
                    {
                        mc.Speed = 1;
                        
                        //tc.Position += tc.Forward * mc.Speed;
                    }

                    if (actionState.Key.Equals("Backward") && actionState.Value.Equals(BUTTON_STATE.PRESSED))
                    {
                        tc.Position += tc.Forward *-mc.Speed;
                    }
                    if (actionState.Key.Equals("Backward") && actionState.Value.Equals(BUTTON_STATE.HELD))
                    {
                        mc.Speed += (float)gametime.ElapsedGameTime.TotalSeconds * mc.Speed * mc.Acceleration;
                        if (mc.Speed > 1.5f)
                            mc.Speed = 1.5f;
                        tc.Position += tc.Forward * -mc.Speed * 0.5f;
                    }
                    if (actionState.Key.Equals("Backward") && actionState.Value.Equals(BUTTON_STATE.RELEASED))
                    {
                        mc.Speed = 2;
                    }
                    if (actionState.Key.Equals("Right") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("Right") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        tc.Rotation += new Vector3(0, -.1f, 0);
                    }
                    if (actionState.Key.Equals("Left") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("Left") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        tc.Rotation += new Vector3(0, .1f, 0);
                    }
                    if ((actionState.Key.Equals("Jump") && actionState.Value.Equals(BUTTON_STATE.PRESSED)) && !pc.InJump || (actionState.Key.Equals("Jump") && actionState.Value.Equals(BUTTON_STATE.HELD) && !pc.InJump))
                    {
                        pc.InJump = true;
                        tc.Position += tc.Up * (float)gametime.ElapsedGameTime.TotalSeconds * 3000;
                    }

                    if (actionState.Key.Equals("Dash") && actionState.Value.Equals((BUTTON_STATE.HELD)))
                    {
                        DashTimer += (float)gametime.ElapsedGameTime.Milliseconds;
                        if (DashTimer > 2000f )
                        {
                            DashTimer = 2000f;
                        }

                        
                    }
                    if (actionState.Key.Equals("Dash") && actionState.Value.Equals(BUTTON_STATE.RELEASED))
                    {
                        mc.Speed = 2 * (DashTimer / 1000);
                        //tc.Position += tc.Forward * mc.Speed;
                        
                        DashTime -= (float)gametime.ElapsedGameTime.Milliseconds;
                        if (DashTime < 0)
                        {
                            DashTimer = 1;
                            DashTime = 1500;
                        }
                    }
                    if (actionState.Key.Equals("Dash") && actionState.Value.Equals(BUTTON_STATE.NOT_PRESSED))
                    {
                        
                        //mc.Speed = 6;
                       
                        //tc.Position += tc.Forward * mc.Speed;

                        DashTime -= (float)gametime.ElapsedGameTime.Milliseconds;
                        if (DashTime < 0)
                        {
                            DashTimer = 1;
                            DashTime = 1500;
                        }
                    }
                }

            }
        }
        public void HandleCollision(GameTime gameTime, ComponentManager componentManager)
        {

        }
    }
}
