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
    public class CollisionHandlingSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager)
        {
            //Make character moveback to prior frame when colliding
            HandleCollision(gameTime, componentManager, messageManager);
            //Implement type of collision and put on colliding units
        }

        public void HandleCollision(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager)
        {
            Dictionary<Entity, IComponent> activeCollisionComponents = componentManager.GetComponents<ActiveCollisionComponent>();

            foreach (KeyValuePair<Entity, IComponent> aColl in activeCollisionComponents)
            {
                ActiveCollisionComponent activeColl = (ActiveCollisionComponent)aColl.Value;
                ModelComponent model = componentManager.GetComponent<ModelComponent>(aColl.Key);
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(aColl.Key);
                MovementComponent movement = componentManager.GetComponent<MovementComponent>(aColl.Key);
                PhysicsComponent physics = componentManager.GetComponent<PhysicsComponent>(aColl.Key);

                foreach (Message message in messageManager.GetMessages(aColl.Key.ID))
                {
                    var collidedWith = componentManager.GetEntity(message.receiver);
                    if (message.msg.ToLower() == "collision")
                    {
                        if (collidedWith.Tag.ToLower() == "platform")
                        {
                            PassiveCollisionComponent passiveColl = componentManager.GetComponent<PassiveCollisionComponent>(collidedWith);

                        }
                        if (collidedWith.Tag.ToLower() == "player")
                        {

                        }
                    }
                }
            }


        }
    }
}
