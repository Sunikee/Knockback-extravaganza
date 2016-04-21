﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
 using System.Xml;
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
        public int counter = 0;
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

                    UpdateCollisionComponent(activeModel1.Model, aColl1, activeTrans1.World);

                    foreach (KeyValuePair<Entity, IComponent> activeComp2 in activeComponents)
                    {
                        ModelComponent activeModel2 = componentManager.GetComponent<ModelComponent>(activeComp2.Key);
                        ModelTransformComponent activeModelTrans2 = componentManager.GetComponent<ModelTransformComponent>(activeComp1.Key);
                        TransformComponent activeTrans2 = componentManager.GetComponent<TransformComponent>(activeComp1.Key);
                        PhysicsComponent activePC2 = componentManager.GetComponent<PhysicsComponent>(activeComp1.Key);
                        ActiveCollisionComponent aColl2 = componentManager.GetComponent<ActiveCollisionComponent>(activeComp2.Key);

                        if (activeModel1 != activeModel2)
                        {
                            
                        }
                    }
                    foreach (KeyValuePair<Entity, IComponent> passiveComp in passiveComponents)
                    {
                        ModelComponent model2 = componentManager.GetComponent<ModelComponent>(passiveComp.Key);
                        ModelTransformComponent modelTrans2 = componentManager.GetComponent<ModelTransformComponent>(passiveComp.Key);
                        TransformComponent trans2 = componentManager.GetComponent<TransformComponent>(passiveComp.Key);
                        PhysicsComponent pc2 = componentManager.GetComponent<PhysicsComponent>(passiveComp.Key);
                        PassiveCollisionComponent passColl = componentManager.GetComponent<PassiveCollisionComponent>(passiveComp.Key);


                        UpdateCollisionComponent(model2.Model, passColl, trans2.World);

                        Console.WriteLine(aColl1.BoundingBox);

                        if (aColl1.BoundingBox.Intersects(passColl.BoundingBox))
                        {
                            counter += 1;
                            Console.WriteLine(counter);
                        }

                    }
                }
            }
        }

        public void UpdateCollisionComponent(Model model, CollisionComponent collisionComponent, Matrix world)
        {
            collisionComponent.Minimum = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            collisionComponent.Maximum = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), world);

                        collisionComponent.Minimum = Vector3.Min(collisionComponent.Minimum, transformedPosition);
                        collisionComponent.Maximum = Vector3.Max(collisionComponent.Maximum, transformedPosition);
                    }
                }
            }
            collisionComponent.BoundingBox = new BoundingBox(collisionComponent.Minimum, collisionComponent.Maximum); 
        }
    }
}
