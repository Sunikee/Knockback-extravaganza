using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace ECS_Engine.Engine.Systems
{
    /// <summary>
    /// System Description
    /// This system handles the physics of the game, more specifically the
    /// different forces that acts on the game.
    /// </summary>
    public class PhysicsSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager)
        {
            if (sceneManager.GetCurrentScene().Name == "singlePlayerScene")
            {
                var components = componentManager.GetComponents<PhysicsComponent>();
                if (components != null)
                {
                    foreach (KeyValuePair<Entity, IComponent> comp in components)
                    {
                        var msg = messageManager.GetMessages(comp.Key.ID);
                        float gameSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                            ActiveCollisionComponent activeCollisionComponent = componentManager.GetComponent<ActiveCollisionComponent>(comp.Key);
                            PhysicsComponent physicsComponent = componentManager.GetComponent<PhysicsComponent>(comp.Key);
                            TransformComponent transformComponent = componentManager.GetComponent<TransformComponent>(comp.Key);
                            MovementComponent movementComponent = componentManager.GetComponent<MovementComponent>(comp.Key);
                            //PlayerComponent playerComponent = componentManager.GetComponent<PlayerComponent>(comp.Key);

                            ApplyGravity(gameTime, physicsComponent, transformComponent, movementComponent);
                    

                        transformComponent.Position += (physicsComponent.Velocity + movementComponent.Velocity) * gameSpeed;

                        physicsComponent.Velocity -= physicsComponent.Velocity * new Vector3(1,0,1) * gameSpeed;

                        foreach(var message in msg) {
                            if(message.msg == "knockback") {
                                var senderPhysics = componentManager.GetComponent<PhysicsComponent>(componentManager.GetEntity(message.sender));
                                var senderMovement = componentManager.GetComponent<MovementComponent>(componentManager.GetEntity(message.sender));
                                ApplyKnockback(physicsComponent, senderPhysics, movementComponent, senderMovement);
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Function name: ApplyGravity(GameTime, PhysicsComponent, TransformComponent, MovementComponent)
        /// This function applies gravity to a moveable object using the gravity varible of the game 
        /// as well as the speed and air time of the falling object.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="physicsComponent"></param>
        /// <param name="transformComponent"></param>
        /// <param name="movementComponent"></param>
        private void ApplyGravity(GameTime gameTime, PhysicsComponent physicsComponent, TransformComponent transformComponent, MovementComponent movementComponent)
        {
            float gravity = 0;
            if (physicsComponent.InAir) {
                gravity = physicsComponent.Gravity * physicsComponent.GravityStrength;
                physicsComponent.Velocity += new Vector3(0, -gravity * (float)gameTime.ElapsedGameTime.TotalSeconds * 2, 0);
            }
            else {
                physicsComponent.Velocity = new Vector3(physicsComponent.Velocity.X, 0, physicsComponent.Velocity.Z);
            }
            
        }

        /// <summary>
        /// Function name: ApplyKnockback(PhysicsComponent, PhysicsComponent, MovementComponent, MovementComponent)
        /// This function receives the mass and velocity for two objects that just collided and calculates
        /// the end velocity for both of the objects.
        /// </summary>
        /// <param name="physicsComponent1"></param>
        /// <param name="physicsComponent2"></param>
        /// <param name="movementComponent1"></param>
        /// <param name="movementComponent2"></param>
        private void ApplyKnockback(PhysicsComponent physicsComponent1, PhysicsComponent physicsComponent2, MovementComponent movementComponent1, MovementComponent movementComponent2) {
            
            physicsComponent1.Velocity = (movementComponent1.Velocity * (physicsComponent1.Mass - physicsComponent2.Mass) + 2 * physicsComponent2.Mass * movementComponent2.Velocity) / (physicsComponent1.Mass + physicsComponent2.Mass);
            physicsComponent2.Velocity = (movementComponent2.Velocity * (physicsComponent2.Mass - physicsComponent1.Mass) + 2 * physicsComponent1.Mass * movementComponent1.Velocity) / (physicsComponent1.Mass + physicsComponent2.Mass);
            physicsComponent1.Velocity *= 2;
            physicsComponent2.Velocity *= 2;
            
        }

    }
}
