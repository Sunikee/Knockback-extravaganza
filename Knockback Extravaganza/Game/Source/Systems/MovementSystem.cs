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
using ECS_Engine.Engine;
using Game.Source.Components;

namespace Game.Source.Systems {
    /// <summary>
    /// Update the players movement depending on input. Also applies frictions to the current momentum.
    /// </summary>
    public class MovementSystem : IUpdateSystem
    {
        public float DashTimer = 1;
        public float DashTime = 1500;
        public float e = 0;
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {
            //if(sceneManager.GetCurrentScene().Name == "singlePlayerScene")
            HandleInput(gametime, componentManager, messageManager, sceneManager);     
        }

        public void HandleInput(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {
            var kComponents = componentManager.GetComponents<KeyBoardComponent>();

            foreach (KeyValuePair<Entity, IComponent> component in kComponents)
            {
                KeyBoardComponent keyboardComp = (KeyBoardComponent)component.Value;
                PlayerComponent player = componentManager.GetComponent<PlayerComponent>(component.Key);
                TransformComponent tc = componentManager.GetComponent<TransformComponent>(component.Key);
                MovementComponent mc = componentManager.GetComponent<MovementComponent>(component.Key);
                PhysicsComponent pc = componentManager.GetComponent<PhysicsComponent>(component.Key);
                SoundEffectComponent soundEffComp = componentManager.GetComponent<SoundEffectComponent>(component.Key);

                if (tc != null && mc != null && pc != null) {
                    float elapsedTime = (float)gametime.ElapsedGameTime.TotalSeconds;

                    Vector3 MoveDir = Vector3.Zero;

                    float tickAcc = mc.Acceleration;//1f * (float)Math.Pow((1 - 0.15), mc.Speed);
                    if (keyboardComp.GetActionState("Forward") != BUTTON_STATE.NOT_PRESSED) {
                        MoveDir += tc.Forward;
                        mc.Velocity += tc.Forward * tickAcc;
                    }
                    else if (keyboardComp.GetActionState("Backward") != BUTTON_STATE.NOT_PRESSED) {
                        MoveDir -= tc.Forward;
                        mc.Velocity -= tc.Forward * tickAcc;
                    }
                    if (keyboardComp.GetActionState("Right") != BUTTON_STATE.NOT_PRESSED)
                    {
                        mc.Velocity += tc.Right * tickAcc;
                    }
                    else if (keyboardComp.GetActionState("Left") != BUTTON_STATE.NOT_PRESSED)
                    {
                        mc.Velocity -= tc.Right * tickAcc;
                    }
                    else if (keyboardComp.GetActionState("Pause") != BUTTON_STATE.NOT_PRESSED)
                    {
                        //sceneManager.SetCurrentScene("pauseScene");
                    }

                    if(keyboardComp.GetActionState("Dash") == BUTTON_STATE.HELD) {
                        player.ChargeTime += elapsedTime;
                        if(player.ChargeTime > 3) {
                            player.ChargeTime = 3;
                        }
                    }
                    else if(keyboardComp.GetActionState("Dash") == BUTTON_STATE.RELEASED) {
                        mc.Velocity += tc.Forward * player.ChargeTime * 1000;
                    }

                        mc.Velocity -= mc.Velocity * elapsedTime * 3;



                    if (keyboardComp.GetActionState("RotateLeft") != BUTTON_STATE.NOT_PRESSED) {
                        tc.Rotation += new Vector3(0, 3 * elapsedTime, 0);
                    }
                    else if (keyboardComp.GetActionState("RotateRight") != BUTTON_STATE.NOT_PRESSED) {
                        tc.Rotation -= new Vector3(0, 3 * elapsedTime, 0);
                    }


                    e += elapsedTime;
                    float timeStep = 0.1f;
                    if (e > timeStep) {
                        e -= timeStep;
                        //Console.WriteLine(mc.Velocity);
                    }
                }

            }
        }
        public void HandleCollision(GameTime gameTime, ComponentManager componentManager)
        {

        }
    }
}
