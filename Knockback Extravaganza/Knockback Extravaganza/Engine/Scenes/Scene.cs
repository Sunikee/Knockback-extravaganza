using ECS_Engine.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Scenes
{
    public abstract class Scene
    {
        private SceneManagerFacade sceneManager = null;

        public bool IsSceneInitialised { get; private set; } = false;
        public ComponentManager ComponentManager { get; private set; }
        public SystemManager SystemManager { get; private set; }
        public MessageManager MessageManager { get; private set; }
        public ContentManager Content;
        public GraphicsDeviceManager Graphics;
        public SceneManagerFacade SceneManager
        {
            get
            {
                return sceneManager;
            }
            set
            {
                if (sceneManager == null) {
                    sceneManager = value;
                    SystemManager.SceneManager = SceneManager;
                }
            }
        }

        public string Name { get; private set; }

        private Scene() { }

        public Scene(string name, ContentManager content, GraphicsDeviceManager graphics) {
            Name = name;
            MessageManager = new MessageManager();
            ComponentManager = new ComponentManager();
            SystemManager = new SystemManager();
            Content = content;
            Graphics = graphics;

            SystemManager.ComponentManager = ComponentManager;
            SystemManager.MessageManager = MessageManager;
        }

        public virtual void InitScene() {
            IsSceneInitialised = true;
        }
        
        public virtual void DestroyScene() {
            IsSceneInitialised = false;
        }

        //Abstract methods
        public abstract void ResetScene();

    }
}
