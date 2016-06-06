using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using ECS_Engine.Engine;
using Game.Source.Components;

namespace Game.Source.Systems {
    /// <summary>
    /// Handles animations for the players.
    /// </summary>
    public class PlayerAnimationSystem : IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager)
        {
            var animationComponents = componentManager.GetComponents<AnimationComponent>();

            foreach (KeyValuePair<Entity, IComponent> component in animationComponents)
            {
                ModelComponent model = componentManager.GetComponent<ModelComponent>(component.Key);
                ModelTransformComponent meshTransforms =
                    componentManager.GetComponent<ModelTransformComponent>(component.Key);
                TransformComponent transform = componentManager.GetComponent<TransformComponent>(component.Key);
                MovementComponent movement = componentManager.GetComponent<MovementComponent>(component.Key);
                foreach (ModelMesh mesh in model.Model.Meshes)
                {
                    if (mesh.Name == "Left_Arm")
                    {
                        meshTransforms.GetTransform(mesh.Name).Rotation = Quaternion.CreateFromYawPitchRoll(0,
                            -MathHelper.Clamp(2*movement.Speed, -MathHelper.PiOver4, MathHelper.PiOver4), 0);
                    }
                    else if (mesh.Name == "Right_Arm")
                    {
                        meshTransforms.GetTransform(mesh.Name).Rotation = Quaternion.CreateFromYawPitchRoll(0,
                            -MathHelper.Clamp(2*movement.Speed, -MathHelper.PiOver4, MathHelper.PiOver4), 0);
                    }
                }
            }
        }
    }
}
