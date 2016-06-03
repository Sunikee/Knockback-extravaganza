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
        private Scene _currentScene;
        private List<Scene> _scenes = new List<Scene>();
        public void SetCurrentScene(string sceneName)
        {
            _currentScene = _scenes.First(s => s.Name == sceneName);
        }

        public Scene GetCurrentScene()
        {
            return _currentScene;
        }

        public void AddScene(Scene scene)
        {
            if (!_scenes.Contains(scene))
                _scenes.Add(scene);
        }

        public Scene GetScene(string name)
        {
                return _scenes.First(s => s.Name == name);
        }

        public List<Scene> GetAllScenes()
        {
            return _scenes;
        }
    }
}
