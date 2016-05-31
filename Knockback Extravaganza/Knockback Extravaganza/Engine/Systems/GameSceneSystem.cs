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

namespace ECS_Engine.Engine.Systems {
    public class GameSceneSystem : IUpdateSystem {
        public void Update(GameTime gametime, ComponentManager componentManager, MessageManager messageManager) {
            Dictionary<Entity, IComponent> components = componentManager.GetComponents<GameSceneComponent>();
            if (components != null) {
                foreach (KeyValuePair<Entity, IComponent> comp in components) {

                }
            }
        }
    }
}
