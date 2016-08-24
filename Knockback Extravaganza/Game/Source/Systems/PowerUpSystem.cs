using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Game.Source.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Game.Source.Components.AI;

namespace Game.Source.Systems
{
    /// <summary>
    /// System Description
    /// This system handles all power ups. When a player picks up a power up, a message is saved.
    /// This system gets that message and applies a random power. It removes the
    /// powerup from the map and starts a powerup timer for the player. When the timer hits 0,
    /// the player loses the power up.
    /// </summary>
    public class PowerUpSystem : IUpdateSystem
    {
        public ContentManager content;
        public List<Entity> powerUpsToRemove = new List<Entity>();
    
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {

            var powerUpSettingsEntities = componentManager.GetComponents<PowerUpSettingsComponent>();
            var powerUpSettingsEntity = powerUpSettingsEntities.First().Key;
            var powerUpSettingsComponent = componentManager.GetComponent<PowerUpSettingsComponent>(powerUpSettingsEntity);

            if (powerUpSettingsComponent.powerUpSpawnTimer == 0)
                powerUpSettingsComponent.randomSpawnTimerInt = powerUpSettingsComponent.random.Next(1000, 2000);

            if (powerUpSettingsComponent.powerUpSpawnTimer >= powerUpSettingsComponent.randomSpawnTimerInt)
            {
                //Spawna powerUpp på random position på platformen.
                SpawnPowerUp(componentManager, powerUpSettingsComponent, messageManager);
                powerUpSettingsComponent.randomSpawnTimerInt = powerUpSettingsComponent.random.Next(1000, 2000);
                powerUpSettingsComponent.powerUpSpawnTimer = 0;
            }
            powerUpSettingsComponent.powerUpSpawnTimer += 0.5f;

            var activeComponents = componentManager.GetComponents<ActiveCollisionComponent>();
            foreach (KeyValuePair<Entity, IComponent> component in activeComponents)
            {
                var transform = componentManager.GetComponent<TransformComponent>(component.Key);
                var physics = componentManager.GetComponent<PhysicsComponent>(component.Key);
                var messages = messageManager.GetMessages(component.Key.ID);
                var powerUp = componentManager.GetComponent<PowerUpComponent>(component.Key);
                var movement = componentManager.GetComponent<MovementComponent>(component.Key);
                var aiC = componentManager.GetComponent<AIComponent>(component.Key);

                if (powerUp != null)
                {
                    powerUp.ActiveTime -= gameTime.ElapsedGameTime.Milliseconds;
                    if (powerUp.ActiveTime <= 0)
                    {
                        transform.Scale = new Vector3(1, 1, 1);
                        physics.Mass = 15;
                        componentManager.RemoveComponent<PowerUpComponent>(component.Key);
                    }
                }
                #region Read Powerup messages

                foreach (var msg in messages)
                {
                    if (msg.msg != "powerUp" || aiC != null) continue;
                    var rnd = new Random();
                    var power = rnd.Next(1, 3);

                    switch (power)
                    {
                        case 1:
                        {
                            transform.Scale = new Vector3(2, 1, 2);

                            physics.Mass *= 2;
                            if (physics.Mass >= 10)
                                physics.Mass = 10;

                            var powerupC = new PowerUpComponent {ActiveTime = 10000, PowerUpType = power};
                            componentManager.AddComponent(component.Key, powerupC);

                            var entity = componentManager.GetEntity(msg.sender);
                            powerUpsToRemove.Add(entity);
                        }
                            break;
                        case 2:
                        {
                            transform.Scale = new Vector3(0.5f);

                            physics.Mass *= 0.5f;
                            if (physics.Mass >= 2.5f)
                                physics.Mass = 2.5f;

                            var powerupC = new PowerUpComponent {ActiveTime = 10000, PowerUpType = power};
                            componentManager.AddComponent(component.Key, powerupC);

                            var entity = componentManager.GetEntity(msg.sender);
                            powerUpsToRemove.Add(entity);
                            //also make him faster
                        }
                            break;
                    }
                }
                #endregion
            }
            RemovePowerUps(componentManager, messageManager);
        }
        public void SpawnPowerUp(ComponentManager componentManager, PowerUpSettingsComponent powerUpSettingsComponent, MessageManager messageManager)
        {
            var rnd = new Random();
            var X = (rnd.Next((int)powerUpSettingsComponent.minCoordX, (int)powerUpSettingsComponent.maxCoordX));
            var Z = (rnd.Next((int)powerUpSettingsComponent.minCoordZ, (int)powerUpSettingsComponent.maxCoordZ));

            var newPowerUpEntity = componentManager.MakeEntity();
            newPowerUpEntity.Tag = "powerUp";
            var newTransformComponent = new TransformComponent
            {
                Position = new Vector3(X, 150, Z),
                Rotation = Vector3.Zero,
                Scale = new Vector3(0.5f)
            };

            var newModelComponent = new ModelComponent
            {
                Model = content.Load<Model>("box")
            };

            var newPhysicsComponent = new PhysicsComponent
            {
                InAir = true,
                GravityStrength = 0.7f
            };
            var newMovementComponent = new MovementComponent
            {
                Acceleration = 1f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };
            var newActivecollisionComponent = new ActiveCollisionComponent(newModelComponent.Model, newTransformComponent.GetWorld(newTransformComponent.UpdateBuffer));
            componentManager.AddComponent(newPowerUpEntity, newMovementComponent, newTransformComponent, newPhysicsComponent, newActivecollisionComponent, newModelComponent);
        }

        public void RemovePowerUps(ComponentManager componentManager, MessageManager messageManager)
        {
            foreach (var entity in powerUpsToRemove)
            {
                componentManager.RemoveAllComponents(entity);
                componentManager.RemoveEntity(entity);

            }
        }
    }
}