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
                    MovementComponent mc = componentManager.GetComponent<MovementComponent>(comp.Key);
                    TransformComponent tc = componentManager.GetComponent<TransformComponent>(comp.Key);

                    if (pc.InAir == true)
                    {
                        tc.Position -= tc.Up * pc.Gravity * pc.GravityStrength * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    }

                }
            }
        }
    }
}
