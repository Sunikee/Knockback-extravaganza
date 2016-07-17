using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Managers {
    public class SceneManagerFacade {
        private SceneManager sceneManager;

        public SceneManagerFacade(SceneManager sceneManager) {
            this.sceneManager = sceneManager;
        }

        public void ChangeScene(string name) {
            sceneManager.ChangeScene(name);
        }

        public void UnloadScene(string name) {
            sceneManager.UnloadScene(name);
        }
    }
}
