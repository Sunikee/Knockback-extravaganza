using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Systems {
    public class ModelRenderSystem : IRenderSystem{

        public void Render(GameTime gameTime, GraphicsDeviceManager graphicsDevice, ComponentManager componentManager) {
            Dictionary<Entity, IComponent> cam = componentManager.GetComponents<CameraComponent>();
            CameraComponent camera = (CameraComponent)cam.First().Value;

            Dictionary<Entity, IComponent> components = componentManager.GetComponents<ModelComponent>();
            foreach (KeyValuePair<Entity, IComponent> component in components) {
                ModelComponent model = (ModelComponent)component.Value;
                ModelTransformComponent transform = componentManager.GetComponent<ModelTransformComponent>(component.Key);
                if (transform != default(ModelTransformComponent)) {

                    Matrix[] transforms = new Matrix[model.Model.Bones.Count()];
                    model.Model.CopyAbsoluteBoneTransformsTo(transforms);
                    foreach (ModelMesh mesh in model.Model.Meshes) {
                        //System.Console.WriteLine(mesh.Name);
                        if(mesh.Name == "Main_Rotor") {
                            transform.GetTranform(mesh.Name).Rotation += new Vector3(0, 20f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                        }
                        else if(mesh.Name == "Bakc_Rotor_transform") {
                            transform.GetTranform(mesh.Name).Rotation += new Vector3(0, 100f * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);
                        }
                        foreach (BasicEffect effect in mesh.Effects) {
                            effect.EnableDefaultLighting();
                            effect.View = camera.View;
                            effect.Projection = camera.Projection;
                            effect.World = transform.GetTranform(mesh.Name).World * transforms[mesh.ParentBone.Index];
                        }
                        mesh.Draw();
                    }
                }
            }
        }

    }
}
