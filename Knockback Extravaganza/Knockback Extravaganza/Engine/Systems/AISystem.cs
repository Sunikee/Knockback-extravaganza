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

namespace ECS_Engine.Engine.Systems
{
    public class AISystem : IUpdateSystem
    {
        public List<Entity> aiEntitiesToRemove = new List<Entity>();
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager)
        {

            var aiEntities = componentManager.GetComponents<AIComponent>();
     
            if (aiEntities != null && sceneManager.GetCurrentScene().Name == "singlePlayerScene")
            {
                foreach (KeyValuePair<Entity, IComponent> ai in aiEntities)
                {
        
                    // Get AI data
                    var aiAiC = componentManager.GetComponent<AIComponent>(ai.Key);
                    var aiTransformC = componentManager.GetComponent<TransformComponent>(ai.Key);
                    var aiModelTransC = componentManager.GetComponent<ModelTransformComponent>(ai.Key);
                    var aiPhysicsC = componentManager.GetComponent<PhysicsComponent>(ai.Key);
                    var aiMovec = componentManager.GetComponent<MovementComponent>(ai.Key);

                    // Get target data
                    var playerEntities = componentManager.GetComponents<PlayerComponent>();
                    Random rand = new Random();
                    var targetEntity = playerEntities.Keys.ToArray()[rand.Next(0, playerEntities.Count())];
                    var targetTransC = componentManager.GetComponent<TransformComponent>(targetEntity);

                    aiAiC.Duration -= gametime.ElapsedGameTime.Milliseconds;
                    if (aiAiC.Duration <= 0)
                        aiEntitiesToRemove.Add(ai.Key);


                    //USE THIS WHEN VELOCITY WORKS!!!!!!!!!!!!!!

                    //var diff = targetTransC.Position - aiTransformC.Position;
                    //diff.Normalize();
                    //aiMovec.Velocity += diff;




                    //Chase in Pos X and pos Z
                    if (aiTransformC.Position.X <= targetTransC.Position.X && aiTransformC.Position.Z <= targetTransC.Position.Z)
                        aiTransformC.Position = new Vector3(aiTransformC.Position.X + 0.2f, aiTransformC.Position.Y, aiTransformC.Position.Z + 0.2f);

                    // Chase in neg X and neg Z
                    else if (aiTransformC.Position.X >= targetTransC.Position.X && aiTransformC.Position.Z >= targetTransC.Position.Z)
                        aiTransformC.Position = new Vector3(aiTransformC.Position.X - 0.2f, aiTransformC.Position.Y, aiTransformC.Position.Z - 0.2f);

                    // Chase in Pos X and neg Z
                    else if (aiTransformC.Position.X <= targetTransC.Position.X && aiTransformC.Position.Z >= targetTransC.Position.Z)
                        aiTransformC.Position = new Vector3(aiTransformC.Position.X + 0.2f, aiTransformC.Position.Y, aiTransformC.Position.Z - 0.2f);

                    // Chase in neg X and pos Z
                    else if (aiTransformC.Position.X >= targetTransC.Position.X && aiTransformC.Position.Z <= targetTransC.Position.Z)
                        aiTransformC.Position = new Vector3(aiTransformC.Position.X - 0.2f, aiTransformC.Position.Y, aiTransformC.Position.Z + 0.2f);
                }
            }
            RemoveAIEntity(componentManager);
        }
        public void RemoveAIEntity(ComponentManager componentManager)
        {
            foreach( var e in aiEntitiesToRemove)
            { 
            componentManager.RemoveEntity(e);
            }
            //aiEntitiesToRemove.Clear();
        }
    }
}
