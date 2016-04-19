using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ECS_Engine.Engine.Systems
{
    public class PhysicsSystem : IUpdateSystem
    {
        public void Update(GameTime gameTime, ComponentManager componentManager)
        {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<PhysicsComponent>();
            if (components != null)
            {
                foreach (KeyValuePair<Entity, IComponent> comp in components)
                {
                    PhysicsComponent pc = componentManager.GetComponent<PhysicsComponent>(comp.Key);
                    TransformComponent tc = componentManager.GetComponent<TransformComponent>(comp.Key);
                    MovementComponent mc = componentManager.GetComponent<MovementComponent>(comp.Key);
                    PlayerComponent playC = componentManager.GetComponent<PlayerComponent>(comp.Key);

                    ApplyGravity(gameTime, pc, tc);
                    if (tc.Position.Y == 0 && playC != null)
                    {
                        pc.InJump = false;
                    }
                }
            }
        }

        private void ApplyGravity(GameTime gameTime, PhysicsComponent pc, TransformComponent tc)
        {
            tc.Position -= Vector3.Up * pc.Gravity * pc.GravityStrength * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        
    }
}
