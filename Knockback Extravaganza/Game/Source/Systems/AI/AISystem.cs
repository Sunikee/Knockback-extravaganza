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
using Game.Source.Systems.AI.AIStates;

namespace Game.Source.Systems.AI
{
    /// <summary>
    /// Updates the AI and their states to follow the players around.
    /// </summary>
    public class AISystem : IUpdateSystem
    {
        public List<Entity> aiEntitiesToRemove = new List<Entity>();
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager)
        {
            var aiEntities = componentManager.GetComponents<AIComponent>();

            #region Initialize states

            var chargeState = new AICharge();
            var stopState = new AIStop();
            var followState = new AIFollow();

            IAiStates currentState = followState;

            #endregion

            if (aiEntities != null)
            {
                foreach (var ai in aiEntities)
                {
                    // Get AI data
                    var aiAiC = componentManager.GetComponent<AIComponent>(ai.Key);
                    var aiTransformC = componentManager.GetComponent<TransformComponent>(ai.Key);
                    var aiMovec = componentManager.GetComponent<MovementComponent>(ai.Key);

                    // Get target data
                    var playerEntities = componentManager.GetComponents<PlayerComponent>();
                    var targetEntity = playerEntities.First().Key;
                    var targetTransC = componentManager.GetComponent<TransformComponent>(targetEntity);
                    aiAiC.Duration -= gametime.ElapsedGameTime.Milliseconds;

                    // Update states leave 300 to 400 to avoid hysteria.
                    var distance = Vector3.DistanceSquared(targetTransC.Position, aiTransformC.Position);

                    if ((distance > 400 * 400 && currentState == followState))
                        currentState = stopState;

                    else if (distance < 300 * 300)
                        currentState = followState;

                    if (currentState == stopState)
                        currentState = chargeState;

                    currentState?.Run(targetTransC, aiTransformC, aiMovec);
                }
            }
            RemoveAIEntity(componentManager);
        }

        #region helpers

        public void RemoveAIEntity(ComponentManager componentManager)
        {
            foreach (var e in aiEntitiesToRemove)
            {
                componentManager.RemoveEntity(e);
            }
        }

        #endregion
    }
}
