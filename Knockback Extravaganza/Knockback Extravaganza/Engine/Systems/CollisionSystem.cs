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

namespace ECS_Engine.Engine.Systems
{
    public class CollisionDetectionSystem : IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager)
        {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<CollisionComponent>();
            if (components != null)
            {
                foreach (KeyValuePair<Entity, IComponent> comp1 in components)
                {
                    ModelComponent model1 = componentManager.GetComponent<ModelComponent>(comp1.Key);
                    TransformComponent world1 = componentManager.GetComponent<TransformComponent>(comp1.Key);
                    PhysicsComponent pc1 = componentManager.GetComponent<PhysicsComponent>(comp1.Key);

                    foreach (KeyValuePair<Entity, IComponent> comp2 in components)
                    {
                        ModelComponent model2 = componentManager.GetComponent<ModelComponent>(comp2.Key);
                        TransformComponent world2 = componentManager.GetComponent<TransformComponent>(comp2.Key);
                        PhysicsComponent pc2 = componentManager.GetComponent<PhysicsComponent>(comp2.Key);
                        if (model1 != model2)
                        {
                            for (int meshIndex1 = 0; meshIndex1 < model1.Model.Meshes.Count; meshIndex1++)
                            {
                                BoundingSphere sphere1 = model1.Model.Meshes[meshIndex1].BoundingSphere;
                                sphere1 = sphere1.Transform(world1.World);
                                for (int meshIndex2 = 0; meshIndex2 < model2.Model.Meshes.Count; meshIndex2++)
                                {
                                    BoundingSphere sphere2 = model2.Model.Meshes[meshIndex2].BoundingSphere;
                                    sphere2 = sphere2.Transform(world2.World);

                                    if (sphere1.Intersects(sphere2))
                                    {
                                        Console.WriteLine("intersects");
                                        //Kollision mellan två modeller med collisionscomponenter har skett.
                                        //Hantering av collision
                                        Dictionary<Entity, IComponent> playercomponents = componentManager.GetComponents<PlayerComponent>();

                                        var playercomp1 = componentManager.GetComponent<PlayerComponent>(comp1.Key);
                                        var playercomp2 = componentManager.GetComponent <PlayerComponent>(comp2.Key);    

                                        if( playercomp1 != null && pc1.InJump == false)
                                        {
                                            world1.Position = new Vector3(world1.Position.X, 0, world1.Position.Z);
                                        }

                                        if (playercomp2 != null && pc2.InJump == false)
                                        {
                                            world2.Position = new Vector3(world2.Position.X, 0, world2.Position.Z);
                                        }
                                        // continue;
                                    }

                                }
                            }   
                        }
                    }
                }
            }
        }
    }
}
