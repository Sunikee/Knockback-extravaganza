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


            foreach (KeyValuePair<Entity, IComponent> activeCollisionComponent in activeCollisionComponents)
            {
                var msg = messageManager.GetMessages(activeCollisionComponent.Key.ID);
                TransformComponent transformComponent = componentManager.GetComponent<TransformComponent>(activeCollisionComponent.Key);
                
            }
        }
    }
}