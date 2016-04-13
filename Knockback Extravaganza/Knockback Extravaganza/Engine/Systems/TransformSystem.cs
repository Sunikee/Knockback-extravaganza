using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Component;

namespace ECS_Engine.Engine.Systems {
    public class TransformSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager) {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<TransformComponent>();
            if(components != null) {
                foreach(KeyValuePair<Entity, IComponent> component in components) {
                    TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                    Quaternion rotation = Quaternion.CreateFromYawPitchRoll(transform.Rotation.Y, transform.Rotation.X, transform.Rotation.Z);

                    transform.Forward = GetLocalDir(Vector3.Forward, rotation);
                    transform.Right = GetLocalDir(Vector3.Right, rotation);
                    transform.Up = GetLocalDir(Vector3.Up, rotation);

                }
            }
        }

        private Vector3 GetLocalDir(Vector3 dir, Quaternion rotation) {
            Vector3 result = Vector3.Transform(dir, rotation);
            result.Normalize();
            return result;
        }
    }
}
