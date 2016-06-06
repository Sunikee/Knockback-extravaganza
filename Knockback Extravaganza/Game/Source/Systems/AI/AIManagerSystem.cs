using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using Game.Source.Components.AI;
using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;

namespace Game.Source.Systems.AI {
    /// <summary>
    /// Spawns new AI agents according to the time intervals given from the components and sets them to standards settings.
    /// </summary>
    class AIManagerSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager) {
            var components = componentManager.GetComponents<AIManagerComponent>();
            if(components != null) {
                foreach(var component in components) {
                    AIManagerComponent aiManager = component.Value as AIManagerComponent;
                    aiManager.spawnTimer += (float)gametime.ElapsedGameTime.TotalSeconds;

                    if(aiManager.spawnAfterSeconds < aiManager.spawnTimer) {
                        aiManager.spawnTimer -= aiManager.spawnAfterSeconds;

                        Entity newAIAgent = componentManager.MakeEntity();
                        newAIAgent.Tag = "player";
                        var aiAiC = new AIComponent();

                        Random rnd = new Random((int)gametime.TotalGameTime.TotalSeconds);

                        Vector3 pos = Vector3.Zero;
                        pos.X += rnd.Next((int)aiManager.spawnMin.X, (int)aiManager.spawnMax.X);
                        pos.Y += 15;
                        pos.Z += rnd.Next((int)aiManager.spawnMin.Z, (int)aiManager.spawnMax.Z);

                        var aiTransformC = new TransformComponent {
                            Position = pos,
                        };
                        var aimodelC = new ModelComponent {
                            Model = aiManager.AIModel
                        };
                        var aiPhysicsC = new PhysicsComponent {
                            GravityStrength = 0,
                            Mass = 5,
                            InAir = false
                        };
                        var aimoveC = new MovementComponent();

                        var aiActiveCompC = new ActiveCollisionComponent(aimodelC.Model,
                            aiTransformC.GetWorld(aiTransformC.UpdateBuffer));

                        componentManager.AddComponent(newAIAgent, aiAiC, aiActiveCompC, aiTransformC, aimoveC, aimodelC, aiPhysicsC);
                    }
                }
            }
        }
    }
}
