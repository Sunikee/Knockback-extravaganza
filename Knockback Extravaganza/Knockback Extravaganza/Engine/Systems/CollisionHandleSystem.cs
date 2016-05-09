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
        private int count = 0;
        public void Update(GameTime gameTime, ComponentManager componentManager)
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
                ActiveCollisionComponent ActiveCollisionComponent = componentManager.GetComponent<ActiveCollisionComponent>(modelComponent.Key);
                TransformComponent transformComponent = componentManager.GetComponent<TransformComponent>(modelComponent.Key);

                if (ActiveCollisionComponent != null)
                {
                    foreach (KeyValuePair<Entity, bool> collided in ActiveCollisionComponent.GetCollision())
                    {
                        if (collided.Value == true)
                        {
                            count += 1;
                            Console.WriteLine(count);
                            transformComponent.Position += -transformComponent.Forward*(float) gameTime.ElapsedGameTime.TotalSeconds * 9.82f;
                            transformComponent.Position += transformComponent.Up*(float) gameTime.ElapsedGameTime.TotalSeconds * 9.82f*5f;
                        }
                    }
                }

            }
        }
    }
}
