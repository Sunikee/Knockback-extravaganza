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
using Game.Source.Components.AI;
using ECS_Engine.Engine;
using Game.Source.Components;

namespace Game.Source.Systems.AI {
    /// <summary>
    /// Updates the AI and their states to follow the players around.
    /// </summary>
    public class AISystem : IUpdateSystem
    {
        public List<Entity> aiEntitiesToRemove = new List<Entity>();
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {

            var aiEntities = componentManager.GetComponents<AIComponent>();

            if (aiEntities != null) {
                foreach (KeyValuePair<Entity, IComponent> ai in aiEntities) {

                    // Get AI data
                    var aiAiC = componentManager.GetComponent<AIComponent>(ai.Key);
                    var aiTransformC = componentManager.GetComponent<TransformComponent>(ai.Key);
                    var aiModelTransC = componentManager.GetComponent<ModelTransformComponent>(ai.Key);
                    var aiPhysicsC = componentManager.GetComponent<PhysicsComponent>(ai.Key);
                    var aiMovec = componentManager.GetComponent<MovementComponent>(ai.Key);

                    // Get target data
                    var playerEntities = componentManager.GetComponents<PlayerComponent>();
                    //Random rand = new Random();
                    //var targetEntity = playerEntities.Keys.ToArray()[rand.Next(0, playerEntities.Count())];
                    var targetEntity = playerEntities.First().Key;
                    var targetTransC = componentManager.GetComponent<TransformComponent>(targetEntity);
                    var targetMoveC = componentManager.GetComponent<MovementComponent>(targetEntity);
                    aiAiC.Duration -= gametime.ElapsedGameTime.Milliseconds;
                    //if (aiAiC.Duration <= 0)
                    //    aiEntitiesToRemove.Add(ai.Key);


                    //USE THIS WHEN VELOCITY WORKS!!!!!!!!!!!!!!
                    float distance = Vector3.DistanceSquared(targetTransC.Position, aiTransformC.Position);
                    if ((distance > 400 * 400 && aiAiC.State == AiState.Follow)) {
                        aiAiC.State = AiState.Stop;
                    }
                    else if (distance < 300 * 300) {
                        aiAiC.State = AiState.Follow;
                    }

                    if (aiAiC.State == AiState.Charge) {
                        var diff = targetTransC.Position - aiTransformC.Position;
                        diff.Normalize();
                        aiMovec.Velocity += diff * new Vector3(2, 0, 2);
                        //aiAiC.State = "follow";
                    }
                    else if (aiAiC.State == AiState.Stop) {
                        aiMovec.Velocity = aiMovec.Velocity * 0.1f;
                        aiAiC.State = AiState.Charge;
                    }
                    else if (aiAiC.State == AiState.Follow) {
                        var diff = targetTransC.Position - aiTransformC.Position;
                        diff.Normalize();
                        aiMovec.Velocity += diff * new Vector3(1, 0, 1);
                    }
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
