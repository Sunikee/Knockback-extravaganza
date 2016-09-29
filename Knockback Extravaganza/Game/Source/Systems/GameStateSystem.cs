using ECS_Engine.Engine.Systems.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using Game.Source.Components;
using ECS_Engine.Engine;

namespace Game.Source.Systems {
    /// <summary>
    /// Handles the different states that game can end in.
    /// </summary>
    public class GameStateSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManagerFacade sceneManager) {

            var components = componentManager.GetComponents<PlayerComponent>();
            var platformComponents = componentManager.GetComponents<ModelComponent>();
            
            foreach (KeyValuePair<Entity, IComponent> component in components) {
                var playerC = componentManager.GetComponent<TransformComponent>(component.Key);
                // TODO: Handle Game states.
                if(playerC.Position.Y < -60)
                {
                    sceneManager.ChangeScene("GameOver");
                }
            }

        }
    }
}
