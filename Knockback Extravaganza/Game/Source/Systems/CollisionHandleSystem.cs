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
using ECS_Engine.Engine;

namespace Game.Source.Systems {
    /// <summary>
    /// Handles the collissions messages and parse them and send new messages depending on the collission types that happend.
    /// </summary>
    public class CollisionHandlingSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {
            
            //Make character moveback to prior frame when colliding
            HandleCollision(gameTime, componentManager, messageManager);
            //Implement type of collision and put on colliding units
            
        }

        public void HandleCollision(GameTime gameTime, ComponentManager componentManager, MessageManager messageManager)
        {
           var activeCollisionComponents = componentManager.GetComponents<ActiveCollisionComponent>();

            foreach (KeyValuePair<Entity, IComponent> aColl in activeCollisionComponents)
            {
                ActiveCollisionComponent activeColl = (ActiveCollisionComponent)aColl.Value;
                ModelComponent model = componentManager.GetComponent<ModelComponent>(aColl.Key);
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(aColl.Key);
                MovementComponent movement = componentManager.GetComponent<MovementComponent>(aColl.Key);
                PhysicsComponent physics = componentManager.GetComponent<PhysicsComponent>(aColl.Key);

                var msg = messageManager.GetMessages(aColl.Key.ID);
                foreach (Message message in msg)
                {
                    var collidedWith = componentManager.GetEntity(message.sender);
                    if (message.msg.ToLower() == "collision")
                    {
                        if (collidedWith.Tag.ToLower() == "platform")
                        {
                            var pass = componentManager.GetComponent<PassiveCollisionComponent>(collidedWith);
                            float dist = pass.BoundingBox.Max.Y + ((ActiveCollisionComponent)aColl.Value).BoundingBox.Min.Y;

                            if (physics.InAir && dist >= -0.1f) {
                                physics.InAir = false;
                                transform.Position = new Vector3(transform.Position.X, pass.BoundingBox.Max.Y, transform.Position.Z);//(physics.Velocity + movement.Velocity) * (float)gameTime.ElapsedGameTime.TotalSeconds; // new Vector3(0, 0.01f, 0);
                            }
                            else if (physics.InAir) {
                                transform.Position -= (physics.Velocity + movement.Velocity) * new Vector3(1, 0, 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
                                movement.Velocity = new Vector3(0, 0, 0);
                            }
                        }
                        if (collidedWith.Tag.ToLower() == "player")
                        {
                            messageManager.RegMessage(message.sender, message.receiver, 0, "knockback");
                            transform.Position -= (physics.Velocity + movement.Velocity) * new Vector3(1, 0, 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * 2;
                        }
                    }
                }
                if(msg.Count() == 0) {
                    physics.InAir = true;
                }
            }


        }
    }
}
