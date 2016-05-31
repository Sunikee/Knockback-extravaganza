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
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager)
        {
            HandleInput(gametime, componentManager, messageManager);     
        }

        public void HandleInput(GameTime gametime, ComponentManager componentManager, MessageManager messageManager)
        {
            Dictionary<Entity, IComponent> kComponents = componentManager.GetComponents<KeyBoardComponent>();

            foreach (KeyValuePair<Entity, IComponent> component in kComponents)
            {
                KeyBoardComponent keyboardComp = (KeyBoardComponent)component.Value;
                TransformComponent tc = componentManager.GetComponent<TransformComponent>(component.Key);
                MovementComponent mc = componentManager.GetComponent<MovementComponent>(component.Key);
                PhysicsComponent pc = componentManager.GetComponent<PhysicsComponent>(component.Key);
                SoundEffectComponent soundEffComp = componentManager.GetComponent<SoundEffectComponent>(component.Key);

                foreach (KeyValuePair<string, BUTTON_STATE> actionState in keyboardComp.ActionStates)
                {
                    if (actionState.Key.Equals("Forward") && actionState.Value.Equals(BUTTON_STATE.PRESSED))
                    {
                        tc.Forward.Normalize();
                        mc.Velocity = tc.Forward;
                        tc.Position += mc.Velocity * mc.Speed;
                        messageManager.RegMessage(component.Key.ID, component.Key.ID, 0, "START: footstep");
                        //tc.Position += tc.Forward * mc.Speed;
                    }
                    //tc.Position += tc.Forward * mc.Speed;
                    if (actionState.Key.Equals("Forward") && actionState.Value.Equals(BUTTON_STATE.HELD))
                    {
                        mc.Speed += (float)gametime.ElapsedGameTime.TotalSeconds * mc.Acceleration;
                        if (mc.Speed > 6)
                            mc.Speed = 6;
                        tc.Forward.Normalize();
                        mc.Velocity = tc.Forward;
                        tc.Position += mc.Velocity * mc.Speed;
                        messageManager.RegMessage(component.Key.ID, component.Key.ID, 0, "START: footstep");
                        //tc.Position += tc.Forward * mc.Speed;
                    }
                    
                    if (actionState.Key.Equals("Forward") && actionState.Value.Equals(BUTTON_STATE.RELEASED))
                    {
                        if (mc.Speed > 0)
                            mc.Speed -= (float)gametime.ElapsedGameTime.TotalSeconds * mc.Acceleration;
                        else
                            mc.Speed = 0;
                    }
                    //if (actionState.Key.Equals("Forward") && actionState.Value.Equals(BUTTON_STATE.NOT_PRESSED) && actionState.Key.Equals("Backward") && actionState.Value.Equals(BUTTON_STATE.NOT_PRESSED) && actionState.Key.Equals("Right") && actionState.Value.Equals(BUTTON_STATE.NOT_PRESSED) && actionState.Key.Equals("Left") && actionState.Value.Equals(BUTTON_STATE.NOT_PRESSED))
                    //{
                    //    mc.Speed -= (float)gametime.ElapsedGameTime.TotalSeconds * mc.Acceleration;
                    //    if (mc.Speed < 0)
                    //        mc.Speed = 0;
                    //    mc.Velocity = tc.Forward;
                    //    tc.Position += mc.Velocity * mc.Speed;
                    //}

                    if (actionState.Key.Equals("Backward") && actionState.Value.Equals(BUTTON_STATE.HELD))
                    {
                        if (mc.Speed < 3)
                            mc.Speed += (float)gametime.ElapsedGameTime.TotalSeconds * mc.Acceleration; 
                        else
                            mc.Speed = 3;
                        mc.Velocity = -tc.Forward;
                        tc.Position += mc.Velocity * mc.Speed;
                        messageManager.RegMessage(component.Key.ID, component.Key.ID, 0, "START: footstep");
                    }
                    //if (actionState.Key.Equals("Backward") && actionState.Value.Equals(BUTTON_STATE.RELEASED) || actionState.Key.Equals("Backward") && actionState.Value.Equals(BUTTON_STATE.NOT_PRESSED))
                    //{
                    //    if (mc.Speed < 0)
                    //        mc.Speed -= (float)gametime.ElapsedGameTime.TotalSeconds * mc.Acceleration;
                    //    else
                    //        mc.Speed = 0;
                    //    mc.Velocity = -tc.Forward;
                    //    tc.Position += mc.Velocity * mc.Speed;
                    //}
                    if (actionState.Key.Equals("Right") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("Right") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        //tc.Rotation += new Vector3(0, -.1f, 0);
                        mc.Velocity = tc.Right;
                        tc.Position += mc.Velocity * mc.Speed;
                        messageManager.RegMessage(component.Key.ID, component.Key.ID, 0, "START: footstep");
                    }
                    if (actionState.Key.Equals("Left") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("Left") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        //tc.Rotation += new Vector3(0, .1f, 0);
                        mc.Velocity = -tc.Right;
                        tc.Position += mc.Velocity * mc.Speed;
                        messageManager.RegMessage(component.Key.ID, component.Key.ID, 0, "START: footstep");
                    }
                    if (actionState.Key.Equals("RotateRight") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("RotateRight") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        tc.Rotation += new Vector3(0, -0.1f, 0f);
                    }
                    if (actionState.Key.Equals("RotateLeft") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("RotateLeft") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        tc.Rotation += new Vector3(0, 0.1f, 0f);
                    }
                    if ((actionState.Key.Equals("Jump") && actionState.Value.Equals(BUTTON_STATE.PRESSED)) && !pc.InJump || (actionState.Key.Equals("Jump") && actionState.Value.Equals(BUTTON_STATE.HELD) && !pc.InJump))
                    {
                        pc.InJump = true;
                        //mc.Velocity += tc.Up * (float)gametime.ElapsedGameTime.TotalSeconds * 1000;
                        tc.Position += tc.Up * 100;
                    }
                    if (actionState.Key.Equals("Reset") && actionState.Value.Equals(BUTTON_STATE.PRESSED) || (actionState.Key.Equals("Reset") && actionState.Value.Equals(BUTTON_STATE.HELD)))
                    {
                        tc.Position = new Vector3(0, 0, 0);
                    }
                    if (actionState.Key.Equals("Dash") && actionState.Value.Equals((BUTTON_STATE.HELD)))
                    {
                        DashTimer += (float)gametime.ElapsedGameTime.Milliseconds;
                        if (DashTimer > 10000f )
                        {
                            DashTimer = 10000f;
                        }
                    }
                    if (actionState.Key.Equals("Dash") && actionState.Value.Equals(BUTTON_STATE.RELEASED))
                    {
                        mc.Speed += mc.Acceleration * (DashTimer / 10);
                        tc.Position += tc.Forward * mc.Speed;                        
                    }
                    if (actionState.Key.Equals("Dash") && actionState.Value.Equals(BUTTON_STATE.NOT_PRESSED))
                    {
                       
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
