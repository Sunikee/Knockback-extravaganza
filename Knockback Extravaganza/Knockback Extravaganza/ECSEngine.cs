using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using ECS_Engine.Engine.Managers;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace ECS_Engine {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public abstract class ECSEngine : Game {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;
        protected SceneManager sceneManager;

        Thread updateThread;
        Task[] tasks = new Task[2];

        double elapsedTime = 0;
        int frameCounter = 0;
        int frameRate = 0;

        public ECSEngine() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            //graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            sceneManager = new SceneManager();
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here

            
            graphics.SynchronizeWithVerticalRetrace = true;
            this.IsFixedTimeStep = true;
            graphics.ApplyChanges();

            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        bool runOnce = true;
        protected override void Update(GameTime gameTime) {
            // TODO: Add your update logic here

            // Display FPS
            /*
            if (systemManager.EnableFrameCount) {
                this.Window.Title = "Update fps: " + systemManager.frameRateUpdate + ", Render fps: " + systemManager.frameRateRender;
            }
            */

            //systemManager.RunUpdateSystem();
            if (runOnce) {
                updateThread = new Thread(sceneManager.RunSceneUpdateSystem);
                updateThread.Start();
                runOnce = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            // TODO: Add your drawing code here

            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            sceneManager.RunSceneRenderSystem(GraphicsDevice, gameTime);

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args) {
            updateThread.Abort();
            base.OnExiting(sender, args);
        }
    }
}
