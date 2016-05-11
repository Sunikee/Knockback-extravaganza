
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

namespace ECS_Engine.Engine.Systems
{
    public class PowerUpSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager)
        {
            Dictionary<Entity, IComponent> activeComponents = componentManager.GetComponents<ActiveCollisionComponent>();
            foreach (KeyValuePair<Entity, IComponent> component in activeComponents)
            {
                var transform = componentManager.GetComponent<TransformComponent>(component.Key);
                var physics = componentManager.GetComponent<PhysicsComponent>(component.Key);
                var messages = messageManager.GetMessages(component.Key.ID);

                foreach (var msg in messages)
                {
                    if (msg.msg == "Big")
                    {
                        //transform.Scale = new Vector3(transform.Scale.X * 2, transform.Scale.Y, transform.Scale.Z * 2);
                        transform.Scale = new Vector3(2, 1, 2);

                        physics.Mass *= 2;
                        if (physics.Mass >= 10)
                            physics.Mass = 10;
                        //also make him slower
                        var entity = componentManager.GetEntity(msg.receiver);
                    }
                    if (msg.msg == "Small")
                    {
                        //transform.Scale = new Vector3(transform.Scale.X * 0.5f, transform.Scale.Y * 0.5f, transform.Scale.Z * 0.5f);
                        transform.Scale = new Vector3(0.5f);

                        physics.Mass *= 0.5f;
                        if (physics.Mass >= 2.5f)
                            physics.Mass = 2.5f;
                        var entity = componentManager.GetEntity(msg.receiver);
                        //also make him faster
                    }
                }
            }
        }
    }
}
