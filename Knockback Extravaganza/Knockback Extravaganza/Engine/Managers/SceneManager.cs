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
        private Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();

        public void AddScene(Scene scene) {
            scene.SceneManager = new SceneManagerFacade(this);
            scenes.Add(scene.Name, scene);
        }

        // todo: Make so it will not crash the threadings.
        public void ChangeScene(string name) {
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
            var scene = currentScene;
            if(scene != null)
                scene.SystemManager.RunUpdateSystem();
        }

        public void RunSceneRenderSystem(GraphicsDevice graphicsDevice, GameTime gameTime) {
            var scene = currentScene;
            if(scene != null)
                scene.SystemManager.RunRenderSystem(graphicsDevice, gameTime);
        }
        
    }
}
