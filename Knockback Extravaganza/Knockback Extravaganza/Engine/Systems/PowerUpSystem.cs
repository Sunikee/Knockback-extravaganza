﻿
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* System Description
 * This system handles all power ups. When a player picks up a power up, a message is saved.
 * This system gets that message and applies a random power. It removes the 
 * powerup from the map and starts a powerup timer for the player. When the timer hits 0,
 * the player loses the power up.
 * */

namespace ECS_Engine.Engine.Systems
{
    public class PowerUpSystem : IUpdateSystem
    {
        public List<Entity> powerUpsToRemove = new List<Entity>();
       
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager)
        {
            var activeComponents = componentManager.GetComponents<ActiveCollisionComponent>();
            foreach (KeyValuePair<Entity, IComponent> component in activeComponents)
            {
                var transform = componentManager.GetComponent<TransformComponent>(component.Key);
                var physics = componentManager.GetComponent<PhysicsComponent>(component.Key);
                var messages = messageManager.GetMessages(component.Key.ID);
                var powerUp = componentManager.GetComponent<PowerUpComponent>(component.Key);
                var movement = componentManager.GetComponent<MovementComponent>(component.Key);

                if (powerUp != null)
                {
                    powerUp.ActiveTime -= gameTime.ElapsedGameTime.Milliseconds;
                    if (powerUp.ActiveTime <= 0)
                    {
                        transform.Scale = new Vector3(1, 1, 1);
                        physics.Mass = 5;
                        componentManager.RemoveComponent<PowerUpComponent>(component.Key);
                    }
                }
                foreach (var msg in messages)
                {
                    if (msg.msg == "powerUp")
                    {
                        var rnd = new Random();
                        var power = rnd.Next(1, 3);

                        //transform.Scale = new Vector3(transform.Scale.X * 2, transform.Scale.Y, transform.Scale.Z * 2);
                        if (power == 1)
                        {
                            transform.Scale = new Vector3(2, 1, 2);

                            physics.Mass *= 2;
                            if (physics.Mass >= 10)
                                physics.Mass = 10;
                          
                            var powerupC = new PowerUpComponent { ActiveTime = 10000, PowerUpType = power };
                            componentManager.AddComponent(component.Key, powerupC);

                            var entity = componentManager.GetEntity(msg.sender);
                            powerUpsToRemove.Add(entity);
                        }
                        else if (power == 2)
                        {
                            transform.Scale = new Vector3(0.5f);

                            physics.Mass *= 0.5f;
                            if (physics.Mass >= 2.5f)
                                physics.Mass = 2.5f;
                           
                            var powerupC = new PowerUpComponent { ActiveTime = 10000, PowerUpType = power };
                            componentManager.AddComponent(component.Key, powerupC);

                            var entity = componentManager.GetEntity(msg.sender);
                            powerUpsToRemove.Add(entity);
                            //also make him faster
                        }
                    }
                }
            }
            RemovePowerUps(componentManager);
        }

        public void RemovePowerUps(ComponentManager componentManager)
        {
            foreach (var entity in powerUpsToRemove)
            {
                componentManager.RemoveAllComponents(entity);
                componentManager.RemoveEntity(entity);
            }
        }
    }
}
