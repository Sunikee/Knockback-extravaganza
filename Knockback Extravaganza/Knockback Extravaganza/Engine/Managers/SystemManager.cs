using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Threading;

namespace ECS_Engine.Engine.Managers {
    public class SystemManager {

        public ComponentManager ComponentManager { get; set; }
        public MessageManager MessageManager { get; set; }
        public SceneManagerFacade SceneManager { get; set; }

        private bool changeRenderBuffer = false;

        //FPS Counter variables
        public bool EnableFrameCount { get; set; }

        public int frameRateUpdate = 0;
        double elapsedTimeUpdate = 0;
        int frameCountUpdate = 0;

        public int frameRateRender = 0;
        double elapsedTimeRender = 0;
        int frameCountRender = 0;

        //System storage
        List<IRenderSystem> renderSystems = new List<IRenderSystem>();
        List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();

        public SystemManager() {
            EnableFrameCount = false;
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
            Stopwatch watch = new Stopwatch();
            watch.Start();
            GameTime updateGameTime = new GameTime();
            TimeSpan start;

            var runSteps = 1000.0 / 1000.0;
            var currentSteps = 0.0;

            while (true) {
                start = watch.Elapsed;
                
                if (currentSteps > runSteps) {
                    //Console.WriteLine(currentSteps);
                    updateGameTime.ElapsedGameTime = TimeSpan.FromMilliseconds(currentSteps);
                    updateGameTime.TotalGameTime += TimeSpan.FromMilliseconds(currentSteps);
                    currentSteps = 0;

                    //FPS Counter
                    if (EnableFrameCount) {
                        elapsedTimeUpdate += updateGameTime.ElapsedGameTime.TotalSeconds;
                        if (elapsedTimeUpdate > 1) {
                            elapsedTimeUpdate -= 1;
                            frameRateUpdate = frameCountUpdate;
                            frameCountUpdate = 0;
                        }
                        frameCountUpdate++;
                    }

                    MessageManager.Begin(updateGameTime);
                    if (updateSystems.Count > 0) {
                        foreach (IUpdateSystem system in updateSystems) {
                            system.Update(updateGameTime, ComponentManager, MessageManager, SceneManager);
                        }
                    }
                    MessageManager.End();

                    if (changeRenderBuffer == false) {
                        var threadedComps = ComponentManager.GetThreadedComponents();
                        Parallel.ForEach(threadedComps, comp => {
                            comp.CopyThreadedData(comp.IdleRenderBuffer);
                        });
                        changeRenderBuffer = true;
                    }
                }

                //Updates GameTime
                TimeSpan elapsed = watch.Elapsed - start;
                currentSteps += elapsed.TotalMilliseconds;
  
            }
        }

        public void RunRenderSystem(GraphicsDevice graphicsDevice, GameTime gameTime) {
            graphicsDevice.Clear(Color.CornflowerBlue);
            if (renderSystems.Count > 0) {
                foreach (IRenderSystem system in renderSystems) {
                    system.Render(gameTime, graphicsDevice, ComponentManager, SceneManager);
                }
            }

            if (changeRenderBuffer) {
                var threadedComps = ComponentManager.GetThreadedComponents();
                Parallel.ForEach(threadedComps, comp => {
                    comp.ChangeRenderBuffer();
                });
                changeRenderBuffer = false;
            }

            //FPS Counter
            if (EnableFrameCount) {
                elapsedTimeRender += gameTime.ElapsedGameTime.TotalSeconds;
                if (elapsedTimeRender > 1) {
                    elapsedTimeRender -= 1;
                    frameRateRender = frameCountRender;
                    frameCountRender = 0;
                }
                frameCountRender++;
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
