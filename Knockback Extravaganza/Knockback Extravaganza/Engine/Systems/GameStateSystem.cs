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

namespace ECS_Engine.Engine.Systems {
    public class GameStateSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager, SceneManager sceneManager) {

            var components = componentManager.GetComponents<PlayerComponent>();
            foreach (KeyValuePair<Entity, IComponent> component in components) {
                var playerC = componentManager.GetComponent<TransformComponent>(component.Key);

                if (playerC.Position.Y < -100)
                    sceneManager.SetCurrentScene("endScene");
            }

        }
    }
}
