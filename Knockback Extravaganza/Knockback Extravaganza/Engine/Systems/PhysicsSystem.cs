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
                }
            }
        }

        private void ApplyGravity(GameTime gameTime, PhysicsComponent pc, TransformComponent tc)
        {
            tc.Position -= Vector3.Up * pc.Gravity * pc.GravityStrength * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void ApplyKnockback(PhysicsComponent pc1, PhysicsComponent pc2, MovementComponent mc1, MovementComponent mc2) {
            mc1.Velocity = (mc1.Velocity * (pc1.Mass - pc2.Mass) + 2 * pc2.Mass * mc2.Velocity) / (pc1.Mass + pc2.Mass);
            mc2.Velocity = (mc2.Velocity * (pc2.Mass - pc1.Mass) + 2 * pc1.Mass * mc1.Velocity) / (pc1.Mass + pc2.Mass);
        }

    }
}
