using ECS_Engine.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Managers
{
    public class SceneManager
    {
        private Scene currentScene = null;
        private bool changedScene = false;
        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

        public void AddScene(Scene scene) {
            scene.SceneManager = new SceneManagerFacade(this);
            scenes.Add(scene.Name, scene);
        }

        // todo: Make so it will not crash the threadings.
        public void ChangeScene(string name) {
            if (currentScene != null)
                changedScene = true;

            scenes.TryGetValue(name, out currentScene);
            if (currentScene.IsSceneInitialised == false) {
                currentScene.InitScene();
            }
        }

        public void UnloadScene(string name) {
            Scene tempScene;
            scenes.TryGetValue(name, out tempScene);
            if (tempScene != currentScene && tempScene.IsSceneInitialised) {
                tempScene.DestroyScene();
            }
        }

        public void RunSceneUpdateSystem() {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            GameTime updateGameTime = new GameTime();
            TimeSpan start;

            var runSteps = 1000.0 / 1000.0;
            var currentSteps = 0.0;

            while (true) {
                start = watch.Elapsed;

                if (currentSteps > runSteps) {
                    updateGameTime.ElapsedGameTime = TimeSpan.FromMilliseconds(currentSteps);
                    updateGameTime.TotalGameTime += TimeSpan.FromMilliseconds(currentSteps);
                    currentSteps = 0;

                    var scene = currentScene;
                    if (scene != null && scene.IsSceneInitialised) {
                        scene.SystemManager.RunUpdateSystem(updateGameTime);
                    }
                }
                
                TimeSpan elapsed = watch.Elapsed - start;
                currentSteps += elapsed.TotalMilliseconds;
            }
        }

        public void RunSceneRenderSystem(GraphicsDevice graphicsDevice, GameTime gameTime) {
            var scene = currentScene;
            if (scene != null && scene.IsSceneInitialised) {
                scene.SystemManager.RunRenderSystem(graphicsDevice, gameTime);
            }
        }
        
    }
}
