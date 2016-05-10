using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ECS_Engine.Engine.Systems
{
    public class PhysicsSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager)
        {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<PhysicsComponent>();
            if (components != null) 
            {
                foreach (KeyValuePair<Entity, IComponent> comp in components)
                {
                    var msg = messageManager.GetMessages(comp.Key.ID);
                    ActiveCollisionComponent activeCollisionComponent = componentManager.GetComponent<ActiveCollisionComponent>(comp.Key);
                    PhysicsComponent physicsComponent = componentManager.GetComponent<PhysicsComponent>(comp.Key);
                    TransformComponent transformComponent = componentManager.GetComponent<TransformComponent>(comp.Key);
                    MovementComponent movementComponent = componentManager.GetComponent<MovementComponent>(comp.Key);
                    PlayerComponent playerComponent = componentManager.GetComponent<PlayerComponent>(comp.Key);
                    ApplyGravity(gameTime, physicsComponent, transformComponent, movementComponent);
                }
            }
        }

        private void ApplyGravity(GameTime gameTime, PhysicsComponent physicsComponent, TransformComponent transformComponent, MovementComponent movementComponent)
        {
            if (physicsComponent.InJump)
                movementComponent.AirTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(movementComponent.AirTime >= 5f)
                movementComponent.AirTime = 5f;
            float distance = movementComponent.AirTime * movementComponent.Speed + physicsComponent.Gravity * physicsComponent.GravityStrength * movementComponent.AirTime * movementComponent.AirTime * 0.5f;
            transformComponent.Position -= new Vector3(0, distance, 0);
        }

        private void ApplyKnockback(PhysicsComponent physicsComponent1, PhysicsComponent physicsComponent2, MovementComponent movementComponent1, MovementComponent movementComponent2) {
            movementComponent1.Velocity = (movementComponent1.Velocity * (physicsComponent1.Mass - physicsComponent2.Mass) + 2 * physicsComponent2.Mass * movementComponent2.Velocity) / (physicsComponent1.Mass + physicsComponent2.Mass);
            movementComponent2.Velocity = (movementComponent2.Velocity * (physicsComponent2.Mass - physicsComponent1.Mass) + 2 * physicsComponent1.Mass * movementComponent1.Velocity) / (physicsComponent1.Mass + physicsComponent2.Mass);
        }

    }
}
