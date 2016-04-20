 using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Systems
{
    public class CollisionDetectionSystem : IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager)
        {
            Dictionary<Entity, IComponent> activeComponents = componentManager.GetComponents<ActiveCollisionComponent>();
            Dictionary<Entity, IComponent> passiveComponents = componentManager.GetComponents<PassiveCollisionComponent>();

            if (activeComponents != null && passiveComponents != null)
            {

                foreach (KeyValuePair<Entity, IComponent> activeComp1 in activeComponents)
                {
                    ModelComponent activeModel1 = componentManager.GetComponent<ModelComponent>(activeComp1.Key);
                    ModelTransformComponent activeModelTrans1 = componentManager.GetComponent<ModelTransformComponent>(activeComp1.Key);
                    TransformComponent activeTrans1 = componentManager.GetComponent<TransformComponent>(activeComp1.Key);
                    PhysicsComponent activePC1 = componentManager.GetComponent<PhysicsComponent>(activeComp1.Key);
                    ActiveCollisionComponent aColl1 = componentManager.GetComponent<ActiveCollisionComponent>(activeComp1.Key);
                    Console.WriteLine(aColl1.boundingBox.Max);
                    foreach (KeyValuePair<Entity, IComponent> activeComp2 in activeComponents)
                    {
                        ModelComponent activeModel2 = componentManager.GetComponent<ModelComponent>(activeComp2.Key);
                        ModelTransformComponent activeModelTrans2 = componentManager.GetComponent<ModelTransformComponent>(activeComp1.Key);
                        TransformComponent activeTrans2 = componentManager.GetComponent<TransformComponent>(activeComp1.Key);
                        PhysicsComponent activePC2 = componentManager.GetComponent<PhysicsComponent>(activeComp1.Key);
                        ActiveCollisionComponent aColl2 = componentManager.GetComponent<ActiveCollisionComponent>(activeComp2.Key);

                        if (activeModel1 != activeModel2)
                        {
                            aColl1.boundingBox.Intersects(aColl2.boundingBox);
                        }
                    }
                    foreach (KeyValuePair<Entity, IComponent> passiveComp in passiveComponents)
                    {
                        //Måste göra om plattformars kollision så de inte omfattas av boudingspheres. Spheren vi har ligger bara i mitten av vår platform.
                        //Därför man faller igenom.
                        ModelComponent model2 = componentManager.GetComponent<ModelComponent>(passiveComp.Key);
                        ModelTransformComponent modelTrans2 = componentManager.GetComponent<ModelTransformComponent>(passiveComp.Key);
                        TransformComponent trans2 = componentManager.GetComponent<TransformComponent>(passiveComp.Key);
                        PhysicsComponent pc2 = componentManager.GetComponent<PhysicsComponent>(passiveComp.Key);
                        PassiveCollisionComponent passColl = componentManager.GetComponent<PassiveCollisionComponent>(passiveComp.Key);

                        Console.WriteLine(passColl.boundingBox.Max);
                        Console.WriteLine(aColl1.boundingBox.Max);
                        if (aColl1.boundingBox.Intersects(passColl.boundingBox))
                        {
                            Console.WriteLine("INterSECTS with pass");
                        }

                    }
                }
            }
        }
    }
}
/*for (int i = 0; i < activeModel1.Model.Meshes.Count; i++)
                           {
                               BoundingSphere sphere1 = activeModel1.Model.Meshes[i].BoundingSphere;
                               sphere1 = sphere1.Transform(activeTrans1.World);

                               for (int y = 0; y < activeModel2.Model.Meshes.Count; y++)
                               {
                                   BoundingSphere sphere2 = activeModel2.Model.Meshes[y].BoundingSphere;
                                   sphere2 = sphere2.Transform(activeTrans2.World);

                                   if (sphere1.Intersects(sphere2))
                                   {
                                       Console.WriteLine("intersects");
                                       activeTrans1.Position = new Vector3(0, 0, 0);
                                       Dictionary<Entity, IComponent> playercomponents = componentManager.GetComponents<PlayerComponent>();

                                       var playercomp1 = componentManager.GetComponent<PlayerComponent>(activeComp1.Key);
                                       var playercomp2 = componentManager.GetComponent<PlayerComponent>(activeComp2.Key);

                                       if (playercomp1 != null && activePC1.InJump == false)
                                       {
                                           activeTrans1.Position = new Vector3(activeTrans1.Position.X, 0, activeTrans1.Position.Z);
                                       }

                                       if (playercomp2 != null && activePC2.InJump == false)
                                       {
                                           activeTrans1.Position = new Vector3(activeTrans1.Position.X, 0, activeTrans1.Position.Z);
                                       }
                                       // continue;
                                   }
                               }
                           }
                       }
                   }*/

/*
                        for (int i = 0; i < activeModel1.Model.Meshes.Count; i++)
                            {
                                BoundingSphere sphere1 = activeModel1.Model.Meshes[i].BoundingSphere;
                                sphere1 = sphere1.Transform(activeTrans1.World);

                                for (int y = 0; y < model2.Model.Meshes.Count; y++)
                                {
                                    BoundingSphere sphere2 = model2.Model.Meshes[y].BoundingSphere;
                                    sphere2 = sphere2.Transform(trans2.World);

                                    if (sphere1.Intersects(sphere2))
                                    {
                                        Console.WriteLine("intersects");
                                        activeTrans1.Position = new Vector3(activeTrans1.Position.X , sphere2.Center.Y + sphere2.Radius, activeTrans1.Position.Z);
                                        Dictionary<Entity, IComponent> playercomponents = componentManager.GetComponents<PlayerComponent>();

                                        var playercomp1 = componentManager.GetComponent<PlayerComponent>(activeComp1.Key);
                                        var playercomp2 = componentManager.GetComponent <PlayerComponent>(activeComp1.Key);    

                                        if( playercomp1 != null && activePC1.InJump == false)
                                        {
                                            activeTrans1.Position = new Vector3(activeTrans1.Position.X, 0, activeTrans1.Position.Z);
                                        }

                                    //if (playercomp2 != null && pc2.InJump == false)
                                    //{
                                    //    trans2.Position = new Vector3(trans2.Position.X, 0, trans2.Position.Z);
                                    //}
                                    //// continue;
                                }*/
