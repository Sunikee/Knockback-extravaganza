using Microsoft.Xna.Framework.Graphics;
using ECS_Engine;
using Game.Source.Scenes;

namespace Game
{

    public class Game1 : ECSEngine
    {
        SpriteFont startFont;
        Texture2D startBackground;
        Texture2D pauseBackground;
        public Game1() : base()
        {

        }

        protected override void Initialize()
        {
            base.Initialize();

            //Initialise Scenes

            //Init startmenu
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Scene content
            startBackground = Content.Load<Texture2D>("Scenes/startBackground2");
            startFont = Content.Load<SpriteFont>("Scenes/Font1");
            pauseBackground = Content.Load<Texture2D>("Scenes/pauseBackground");
            CreateEntities();
        }

        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Create all entities for the game
        /// </summary>
        public void CreateEntities()
        {
            SinglePlayerScene singelplayerScene = new SinglePlayerScene("SinglePlayer", Content, graphics);
            MainMenuScene mainMenuScene = new MainMenuScene("MainMenu", Content, graphics);
            MPHostScene mpHostScene = new MPHostScene("MPHost", Content, graphics);
            MPClientScene mpClientScene = new MPClientScene("MPClient", Content, graphics);
            GameOverScene gameOverScene = new GameOverScene("GameOver", Content, graphics);
            WonGameScene wonGameScene = new WonGameScene("Winner", Content, graphics);

            sceneManager.AddScene(singelplayerScene);
            sceneManager.AddScene(mainMenuScene);
            sceneManager.AddScene(mpHostScene);
            sceneManager.AddScene(mpClientScene);
            sceneManager.AddScene(wonGameScene);
            sceneManager.AddScene(gameOverScene);

            sceneManager.ChangeScene("MainMenu");
        }
    }
}