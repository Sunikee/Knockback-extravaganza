 using System;
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
    /// <summary>
    /// Checks for collissions between active against active and active against passive. It sends messages about collissions that have happened through the
    /// messageManager.
    /// </summary>
    public class CollisionDetectionSystem : IUpdateSystem
    {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager)
        {
            var activeComponents = componentManager.GetComponents<ActiveCollisionComponent>();
            var passiveComponents = componentManager.GetComponents<PassiveCollisionComponent>();

            if (activeComponents != null && passiveComponents != null)
            {

                foreach (KeyValuePair<Entity, IComponent> activeComp1 in activeComponents)
                {

                    ModelComponent activeModel1 = componentManager.GetComponent<ModelComponent>(activeComp1.Key);
                    MovementComponent activeCompMovement = componentManager.GetComponent<MovementComponent>(activeComp1.Key);
                    ModelTransformComponent activeModelTrans1 = componentManager.GetComponent<ModelTransformComponent>(activeComp1.Key);
                    TransformComponent activeTrans1 = componentManager.GetComponent<TransformComponent>(activeComp1.Key);
                    PhysicsComponent activePC1 = componentManager.GetComponent<PhysicsComponent>(activeComp1.Key);
                    MovementComponent activeMC1 = componentManager.GetComponent<MovementComponent>(activeComp1.Key);
                    ActiveCollisionComponent aColl1 = componentManager.GetComponent<ActiveCollisionComponent>(activeComp1.Key);

                   

                    foreach (KeyValuePair<Entity, IComponent> activeComp2 in activeComponents)
                    {
                        ModelComponent activeModel2 = componentManager.GetComponent<ModelComponent>(activeComp2.Key);
                        ModelTransformComponent activeModelTrans2 = componentManager.GetComponent<ModelTransformComponent>(activeComp2.Key);
                        TransformComponent activeTrans2 = componentManager.GetComponent<TransformComponent>(activeComp2.Key);
                        PhysicsComponent activePC2 = componentManager.GetComponent<PhysicsComponent>(activeComp2.Key);
                        ActiveCollisionComponent aColl2 = componentManager.GetComponent<ActiveCollisionComponent>(activeComp2.Key);
                        UpdateCollisionComponent(aColl2, activeTrans2.GetWorld(activeTrans1.UpdateBuffer));
                        UpdateCollisionComponent(aColl1, activeTrans1.GetWorld(activeTrans1.UpdateBuffer));
                        if (activeModel1 != activeModel2)
                        {
                            if (aColl1.BoundingBox.Intersects(aColl2.BoundingBox))
                            {
                                switch (activeComp1.Key.Tag) 
                                {
                                    case "powerUp":
                                        messageManager.RegMessage(activeComp1.Key.ID, activeComp2.Key.ID, 0, "powerUp");
                                        break;
                                   
                                    default:
                                        messageManager.RegMessage(activeComp1.Key.ID, activeComp2.Key.ID, 0, "collision");
                                        break;
                                }                               
                            }
                        }
                    }
                    foreach (KeyValuePair<Entity, IComponent> passiveComp in passiveComponents)
                    {
                        ModelComponent passModel = componentManager.GetComponent<ModelComponent>(passiveComp.Key);
                        ModelTransformComponent passModelTrans = componentManager.GetComponent<ModelTransformComponent>(passiveComp.Key);
                        TransformComponent passTrans = componentManager.GetComponent<TransformComponent>(passiveComp.Key);
                        PhysicsComponent passPC = componentManager.GetComponent<PhysicsComponent>(passiveComp.Key);
                        PassiveCollisionComponent passColl = componentManager.GetComponent<PassiveCollisionComponent>(passiveComp.Key);
                        UpdateCollisionComponent(passColl, passTrans.GetWorld(passTrans.UpdateBuffer));
                        UpdateCollisionComponent(aColl1, activeTrans1.GetWorld(activeTrans1.UpdateBuffer));

                        if (aColl1.BoundingBox.Intersects(passColl.BoundingBox))
                        {
                            messageManager.RegMessage(passiveComp.Key.ID, activeComp1.Key.ID, 0, "collision");
                        }
                    }
                }
            }
        }
        public void UpdateCollisionComponent(CollisionComponent collisionComponent, Matrix world)
        {

            Vector3 positionMax = Vector3.Transform(collisionComponent.Maximum, world);
            Vector3 positionMin = Vector3.Transform(collisionComponent.Minimum, world);

            collisionComponent.BoundingBox = new BoundingBox(positionMin, positionMax);
        }
    }
}
