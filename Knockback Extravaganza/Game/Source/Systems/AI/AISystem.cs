﻿using ECS_Engine.Engine.Systems.Interfaces;
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

            var platformComponents = componentManager.GetComponents<PassiveCollisionComponent>();

            foreach (var p in platformComponents)
            {

                var platformPassiveC = componentManager.GetComponent<PassiveCollisionComponent>(p.Key);

                if (aiEntities != null)
                {
                    foreach (var ai in aiEntities)
                    {
                        // Get AI data
                        var aiAiC = componentManager.GetComponent<AIComponent>(ai.Key);
                        var aiTransformC = componentManager.GetComponent<TransformComponent>(ai.Key);
                        var aiMovec = componentManager.GetComponent<MovementComponent>(ai.Key);
                        var aiMC = componentManager.GetComponent<AIManagerComponent>(ai.Key);
                        var aiActiveC = componentManager.GetComponent<ActiveCollisionComponent>(ai.Key);

                        // Get target data
                        var playerEntities = componentManager.GetComponents<PlayerComponent>();
                        var targetEntity = playerEntities.First().Key;
                        var targetTransC = componentManager.GetComponent<TransformComponent>(targetEntity);
                        aiAiC.Duration -= gametime.ElapsedGameTime.Milliseconds;

                        #region Update states

                        // Update states leave 200 to 400 to avoid hysteria.
                        var distance = Vector3.DistanceSquared(targetTransC.Position, aiTransformC.Position);

                        if ((distance > 400 * 400))
                            currentState = stopState;

                        else if (distance < 200 * 200)
                            currentState = followState;

                        if (currentState == stopState)
                            currentState = chargeState;

                        //if ((aiMC != null && aiTransformC != null) && (aiTransformC.Position.X < aiMC.spawnMin.X || aiTransformC.Position.X > aiMC.spawnMax.X || aiTransformC.Position.Z < aiMC.spawnMin.Z || aiTransformC.Position.Z > aiMC.spawnMax.X))
                        //{
                        //    aiMovec.Velocity = Vector3.Zero;
                        //    currentState = followState;
                        //}
                        currentState?.Run(targetTransC, aiTransformC, aiMovec);
                        // remove ai entity if it has fallen of platform
                        if (aiTransformC.Position.Y < -100)
                            componentManager.RemoveEntity(ai.Key);


                        #endregion
                    }
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
