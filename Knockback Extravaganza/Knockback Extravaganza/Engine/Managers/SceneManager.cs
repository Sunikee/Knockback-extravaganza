using ECS_Engine.Engine.Scenes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Managers
{
    public class SceneManager
    {
        public Scene _currentScene;
        public List<Scene> scenes = new List<Scene>();
        public void SetCurrentScene(Scene sceneType)
        {
            _currentScene = sceneType;
        }
        public Scene GetCurrentScene()
        {
            return _currentScene;
        }
        public void AddScene(Scene scene)
        {
            if (!scenes.Contains(scene))
                scenes.Add(scene);
        }
    }
}
