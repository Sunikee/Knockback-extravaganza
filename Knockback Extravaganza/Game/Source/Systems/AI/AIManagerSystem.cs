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
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager) {
            var components = componentManager.GetComponents<AIManagerComponent>();
            if (components == null) return;

            foreach(var component in components) {
                var aiManager = component.Value as AIManagerComponent;
                aiManager.spawnTimer += (float)gametime.ElapsedGameTime.TotalSeconds;

                if (!(aiManager.spawnAfterSeconds < aiManager.spawnTimer)) continue;
                aiManager.spawnTimer -= aiManager.spawnAfterSeconds;

                var newAIAgent = componentManager.MakeEntity();
                newAIAgent.Tag = "player";
                var aiAiC = new AIComponent();

                var rnd = new Random((int)gametime.TotalGameTime.TotalSeconds);
                var rndscale = new Random();
                var scale = new Vector3(rndscale.Next(1, 3));
                var tmpPos = scale == new Vector3(1) ? 15 : 25;
                var tmpMass = scale == new Vector3(1) ? 5 : 10;


                var pos = Vector3.Zero;
                pos.X += rnd.Next((int)aiManager.spawnMin.X, (int)aiManager.spawnMax.X);
                pos.Y += tmpPos;
                pos.Z += rnd.Next((int)aiManager.spawnMin.Z, (int)aiManager.spawnMax.Z);

                var aiTransformC = new TransformComponent {
                    Position = pos,
                    Scale = scale
                };
                var aimodelC = new ModelComponent {
                    Model = aiManager.AIModel
                };
                var aiPhysicsC = new PhysicsComponent {
                    GravityStrength = 0,
                    Mass = tmpMass,
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
