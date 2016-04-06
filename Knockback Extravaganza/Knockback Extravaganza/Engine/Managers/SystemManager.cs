using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ECS_Engine.Engine.Managers {
    public class SystemManager {

        List<IRenderSystem> renderSystems = new List<IRenderSystem>();
        List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();

        public SystemManager() {

        }

        public void AddSystem(ISystem system) {
            Type type = system.GetType();
            if(type == typeof(IRenderSystem)) {
                AddSystemToList<IRenderSystem>(renderSystems, system);
            }
            else if(type == typeof(IUpdateSystem)) {
                AddSystemToList<IUpdateSystem>(updateSystems, system);
            }
        }

        public void RemoveSystem(ISystem system) {
            Type type = system.GetType();
            if (type == typeof(IRenderSystem)) {
                RemoveSystemFromList<IRenderSystem>(renderSystems, system);
            }
            else if (type == typeof(IUpdateSystem)) {
                RemoveSystemFromList<IUpdateSystem>(updateSystems, system);
            }

        }

        public void RunUpdateSystem(GameTime gameTime) {
            if(updateSystems.Count > 0) {
                foreach(IUpdateSystem system in updateSystems) {
                    system.Update(gameTime);
                }
            }
        }

        public void RunRenderSystem(GameTime gameTime, GraphicsDeviceManager graphics) {
            if(renderSystems.Count > 0) {
                foreach(IRenderSystem system in renderSystems) {
                    system.Render(gameTime, graphics);
                }
            }
        }


        private void AddSystemToList<T>(List<T> list, ISystem system) {
            if (!list.Contains((T)system)) {
                list.Add((T)system);
            }
        }

        private void RemoveSystemFromList<T>(List<T> list, ISystem system) {
            if(list != null) {
                if (list.Contains((T)system)) {
                    list.Remove((T)system);
                }
            }
        }

    }
}
