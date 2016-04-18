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

                    if (pc.InAir == true)
                    {
                        ApplyGravity(gameTime, pc, tc);
                    }
                    else
                        ApplyFriction(gameTime, mc, tc, pc);
                }
            }
        }

        private void ApplyGravity(GameTime gameTime, PhysicsComponent pc, TransformComponent tc)
        {
            tc.Position -= Vector3.Up * pc.Gravity * pc.GravityStrength * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        private void ApplyFriction(GameTime gameTime, MovementComponent mc, TransformComponent tc, PhysicsComponent pc)
        {
            tc.Position += tc.Forward * pc.Friction * mc.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
