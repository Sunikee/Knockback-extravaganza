using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Systems.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ECS_Engine.Engine.Scenes;

namespace ECS_Engine.Engine.Managers {
    public class SceneManager {

        //Dictionary<string, List<Entity>> sceneList = new Dictionary<string, List<Entity>>();
        public List<GameScene> sceneList = new List<GameScene>();

        //Dictionary<Type, Dictionary<Entity, IComponent>> componentList = new Dictionary<Type, Dictionary<Entity, IComponent>>();
        //Dictionary<Type, Dictionary<Entity, IComponent>> activeComponents = new Dictionary<Type, Dictionary<Entity, IComponent>>();
        //Dictionary<Type, Dictionary<Entity, IComponent>> passiveComponents = new Dictionary<Type, Dictionary<Entity, IComponent>>();
        //List<IRenderSystem> renderSystems = new List<IRenderSystem>();
        //List<IUpdateSystem> updateSystems = new List<IUpdateSystem>();
        //List<IRenderSystem> systemsToRender = new List<IRenderSystem>();
        //List<IUpdateSystem> systemsToUpdate = new List<IUpdateSystem>();
        public SpriteBatch _spriteBatch;
        public SpriteFont _spriteFont;
        private List<GameScene> _activeScenes = new List<GameScene>();
        private Texture2D _BlankBackground;
        private StartScreen _startScreen = new StartScreen();
        
        

        public SceneManager() {
        
        }
        public void Initialize(SpriteBatch sb, SpriteFont sf, Texture2D bbg) {
            _spriteBatch = sb;
            _spriteFont = sf;
            _BlankBackground = bbg;
        }

        //Get one scene with entities
        public GameScene GetScene(string scene) {
            return sceneList.First(s => s.SceneName == scene);
        }

        //Get all scenes with entities
        public List<GameScene> GetAllScenes() {
            return sceneList;
        }

        //Add a scene with entities
        public void AddScene(GameScene scene) {
            sceneList.Add(scene);
        }

        //Update all active scenes
        public void Update() {

            foreach (var scene in _activeScenes) {

                switch (scene.SceneName) {

                    case "GameplayScene":
                    //Update(_spriteBatch, _spriteFont);
                    break;
                    case "PauseScene":
                    //scene.Update(_spriteBatch, _spriteFont);
                    break;
                    case "StartScene":
                        _startScreen.Update(_spriteBatch,_spriteFont, _BlankBackground, ref _activeScenes);
                        break;
                    case "HostScene":
                    //scene.Update(_spriteBatch, _spriteFont);
                    break;
                    case "JoinScene":
                    //scene.Update(_spriteBatch, _spriteFont);
                    break;
                    default:
                        break;
                }
            }
        }

        //Remove a scene and all its entities
        public void RemoveScene(string scene) {
            var sceneToRemove = sceneList.First(s => s.SceneName == scene);
            sceneList.Remove(sceneToRemove);
        }

        //Sets a scene to active
        public void SetActiveScene(string scene) {
            //foreach(var s in sceneList) {
            //    if(s.IsActive == true)
            //        _activeScenes.Add(s);
            //}
            _activeScenes.Add(sceneList.First(s => s.SceneName == scene));
        }
        public void RemoveActiveScene(string scene) {
            _activeScenes.Remove(sceneList.First(s => s.SceneName == scene));
        }
        //Adds an entity to a scene
        public void AddEntitiyToScene(string sceneName, Entity entity) 
        {
            sceneList.First(s => s.SceneName == sceneName).entities.Add(entity);
        }

        //Removes an entity from a scene
        public void RemoveEntityFromScene(string sceneName, Entity entity) {

            sceneList.First(s => s.SceneName == sceneName).entities.Remove(entity);
        }

        //Removes all entities from a scene
        public void RemoveAllEntitiesFromScene(string sceneName) {
            sceneList.First(s => s.SceneName == sceneName).entities.Clear();
        }
    }
}
