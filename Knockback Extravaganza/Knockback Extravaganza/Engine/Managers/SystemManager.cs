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

        public ComponentManager ComponentManager { get; set; }
        public MessageManager MessageManager { get; set; }
        public SceneManager SceneManager { get; set; }
        public GameTime GameTime { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        private int bufferOne = 0;
        private int bufferTwo = 1;

        public void flipBuffer() {
            int s = bufferOne;
            bufferOne = bufferTwo;
            bufferTwo = s;
        }

        List<IRenderSystem> renderSystems = new List<IRenderSystem>();
        List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();

        public SystemManager() {

        }

        public void AddSystem(ISystem system) {
            Type type = system.GetType();
            if (system is  IRenderSystem) {
                AddSystemToList<IRenderSystem>(renderSystems, system);
            }
            else if(system is IUpdateSystem) {
                AddSystemToList<IUpdateSystem>(updateSystems, system);
            }
        }

        public void RemoveSystem(ISystem system) {
            Type type = system.GetType();
            if (system is IRenderSystem) {
                RemoveSystemFromList<IRenderSystem>(renderSystems, system);
            }
            else if (system is IUpdateSystem) {
                RemoveSystemFromList<IUpdateSystem>(updateSystems, system);
            }

        }

        public void RunUpdateSystem() {
            
            MessageManager.Begin(GameTime);
            if(updateSystems.Count > 0) {
                foreach (IUpdateSystem system in updateSystems) {
                    
                    system.Update(GameTime, ComponentManager, MessageManager, SceneManager);     
                }
                
            }
            MessageManager.End();
        }

        public void RunRenderSystem() {
            if (renderSystems.Count > 0) {
                foreach(IRenderSystem system in renderSystems) {
                    system.Render(GameTime, GraphicsDevice, ComponentManager, SceneManager);
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
