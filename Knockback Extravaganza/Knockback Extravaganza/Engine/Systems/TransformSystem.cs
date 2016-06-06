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
using ECS_Engine.Engine.Component.Abstract_Classes;

namespace ECS_Engine.Engine.Systems {
    /// <summary>
    /// Updates all the transforms components their World and directional vectors.
    /// </summary>
    public class TransformSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager) {

            var components = componentManager.GetComponents<TransformComponent>();
            if(components != null && sceneManager.GetCurrentScene().Name == "singlePlayerScene")
            {
                foreach(var component in components) {
                    var threadData = component.Value as ThreadedComponent;
                    TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                    Quaternion rotation = Quaternion.CreateFromYawPitchRoll(transform.Rotation.Y, transform.Rotation.X, transform.Rotation.Z);
                    PhysicsComponent physics = componentManager.GetComponent<PhysicsComponent>(component.Key);
                    MovementComponent movement = componentManager.GetComponent<MovementComponent>(component.Key);
                    transform.SetWorld(threadData.UpdateBuffer, Matrix.CreateScale(transform.Scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(transform.Position));
                    transform.Forward = GetLocalDir(Vector3.Forward, rotation);
                    transform.Right = GetLocalDir(Vector3.Right, rotation);
                    transform.Up = GetLocalDir(Vector3.Up, rotation);
                }
            }

            var ModelComponents = componentManager.GetComponents<ModelTransformComponent>();
            if (components != null) {
                foreach (var component in ModelComponents) {
                    ModelTransformComponent transform = componentManager.GetComponent<ModelTransformComponent>(component.Key);
                    foreach (MeshTransform mesh in transform.GetTransforms().Values) {
                        Quaternion rotation = Quaternion.CreateFromAxisAngle(mesh.Right, mesh.Rotation.X) * 
                            Quaternion.CreateFromAxisAngle(mesh.Up, mesh.Rotation.Y) * 
                            Quaternion.CreateFromAxisAngle(mesh.Forward, mesh.Rotation.Z);

                        Matrix rot = Matrix.CreateTranslation(-mesh.ParentBone.Translation) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(mesh.ParentBone.Translation);

                        mesh.SetWorld(transform.UpdateBuffer, Matrix.CreateScale(mesh.Scale) * rot * Matrix.CreateTranslation(mesh.Position));
  
                        mesh.Forward = GetLocalDir(Vector3.Forward, rotation);
                        mesh.Right = GetLocalDir(Vector3.Right, rotation);
                        mesh.Up = GetLocalDir(Vector3.Up, rotation);
                    }
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
