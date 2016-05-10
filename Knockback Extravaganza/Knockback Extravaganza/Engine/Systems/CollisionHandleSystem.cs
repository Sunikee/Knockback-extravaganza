using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;

namespace ECS_Engine.Engine.Systems
{
    public class CollisionHandleSystem:IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager)
        {
            //Make character moveback to prior frame when colliding
            HandleCollision(gameTime, componentManager);
            //Implement type of collision and put on colliding units
        }

        public void HandleCollision(GameTime gameTime, ComponentManager componentManager)
        {
            Dictionary<Entity, IComponent> modelComponents = componentManager.GetComponents<ModelComponent>();

            foreach (KeyValuePair<Entity, IComponent> modelComponent in modelComponents)
            {
                ActiveCollisionComponent activeCollisionComponent = componentManager.GetComponent<ActiveCollisionComponent>(modelComponent.Key);
                TransformComponent transformComponent = componentManager.GetComponent<TransformComponent>(modelComponent.Key);
                PhysicsComponent physicsComponent = componentManager.GetComponent<PhysicsComponent>(modelComponent.Key);
                MovementComponent movementComponent = componentManager.GetComponent<MovementComponent>(modelComponent.Key);

                if (activeCollisionComponent != null)
                {
                    foreach (KeyValuePair<Entity, bool> collided in activeCollisionComponent.GetCollision())
                    {
                        if (collided.Value == true)
                        {
                            physicsComponent.InJump = false;
                            movementComponent.AirTime = 0;
                            transformComponent.Position += -transformComponent.Forward * (float)gameTime.ElapsedGameTime.TotalSeconds * physicsComponent.Gravity;
                            transformComponent.Position += transformComponent.Up * (float)gameTime.ElapsedGameTime.TotalSeconds * physicsComponent.Gravity * physicsComponent.GravityStrength;
                        }
                    }
                }

            }
        }
    }
}
