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
            InitializeSystems();
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

            sceneManager.AddScene(singelplayerScene);
            sceneManager.AddScene(mainMenuScene);

            sceneManager.ChangeScene("MainMenu");

            #region old initialization
            /*
            //Creates a menu entity
            Entity menuEntity = componentManager.MakeEntity();
            var menuC = new MenuComponent
            {
                ActiveChoice = 0,
                ActiveColor = Color.Yellow,
                InactiveColor = Color.Red,
                MenuChoicesSpacing = 150
            };
            KeyBoardComponent menukeysC = new KeyBoardComponent();
            menukeysC.AddKeyToAction("Up", Keys.Up);
            menukeysC.AddKeyToAction("Down", Keys.Down);
            menukeysC.AddKeyToAction("Select", Keys.Enter);
            componentManager.AddComponent(menuEntity, menuC);
            componentManager.AddComponent(menuEntity, menukeysC);

            Entity playerEntity1 = componentManager.MakeEntity();

            //Camera
            Entity camera = componentManager.MakeEntity();
            MouseComponent mouse = new MouseComponent();
            CameraComponent cameraC = new CameraComponent();
            cameraC.FieldOfView = MathHelper.PiOver4;
            cameraC.AspectRatio = graphics.GraphicsDevice.DisplayMode.AspectRatio;
            cameraC.FarPlaneDistace = 10000f;
            cameraC.NearPlaneDistace = .1f;
            cameraC.Target = Vector3.Zero;
            cameraC.Up = Vector3.Up;
            TransformComponent tranformC = new TransformComponent();
            tranformC.Position = new Vector3(0f, 0f, 25f);
            ChaseCameraComponent chase = new ChaseCameraComponent();
            chase.Target = playerEntity1;
            chase.Offset = new Vector3(0f, 200, 400);
            FreeCameraComponent free = new FreeCameraComponent();
            free.GraphicsDevice = graphics.GraphicsDevice;
            free.Game = this;

            componentManager.AddComponent(camera, chase);
            componentManager.AddComponent(camera, cameraC);
            componentManager.AddComponent(camera, tranformC);
            componentManager.AddComponent(camera, mouse);

            Mouse.SetPosition(free.GraphicsDevice.Viewport.Width/2, free.GraphicsDevice.Viewport.Height/2);


            //Platform
            Entity platformEntity = componentManager.MakeEntity();
            platformEntity.Tag = "platform";

            ModelComponent platformModelC = new ModelComponent
            {
                Model = Content.Load<Model>("platform"),
            };

            TransformComponent platformTransformC = new TransformComponent
            {
                Position = Vector3.Zero,
                Scale = new Vector3(4)
            };

            PassiveCollisionComponent passColl = new PassiveCollisionComponent(platformModelC.Model,
                platformTransformC.GetWorld(platformTransformC.UpdateBuffer));

            componentManager.AddComponent(platformEntity, platformModelC);
            componentManager.AddComponent(platformEntity, platformTransformC);
            componentManager.AddComponent(platformEntity, passColl);

            var powerUpSettingsEntity = componentManager.MakeEntity();
            var powerUpSettingsComponent = new PowerUpSettingsComponent
            {
                maxCoordX = passColl.BoundingBox.Max.X,
                minCoordX = passColl.BoundingBox.Min.X,
                maxCoordZ = passColl.BoundingBox.Max.Z,
                minCoordZ = passColl.BoundingBox.Min.Z,
                random = new Random(),
                powerUpSpawnTimer = 0
            };

            componentManager.AddComponent(powerUpSettingsEntity, powerUpSettingsComponent);

            // Player
            ModelComponent player1 = new ModelComponent();
            player1.Model = Content.Load<Model>("Player");
            ModelTransformComponent t1 = new ModelTransformComponent(player1.Model);

            TransformComponent tc1 = new TransformComponent()
            {
                Position = new Vector3(0, 10, 0),
                Rotation = new Vector3(0, 0, 0),
                Scale = Vector3.One
            };

            KeyBoardComponent kbc1 = new KeyBoardComponent();
            kbc1.AddKeyToAction("Forward", Keys.W);
            kbc1.AddKeyToAction("Left", Keys.A);
            kbc1.AddKeyToAction("Backward", Keys.S);
            kbc1.AddKeyToAction("Right", Keys.D);
            kbc1.AddKeyToAction("RotateLeft", Keys.Left);
            kbc1.AddKeyToAction("RotateRight", Keys.Right);
            kbc1.AddKeyToAction("Dash", Keys.Space);
            kbc1.AddKeyToAction("Reset", Keys.R);
            kbc1.AddKeyToAction("Pause", Keys.Escape);

            PhysicsComponent pc1 = new PhysicsComponent
            {
                InAir = true,
                GravityStrength = 4,
                Mass = 5
            };

            MovementComponent moveC1 = new MovementComponent
            {
                Acceleration = 2f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };


            ActiveCollisionComponent actColl = new ActiveCollisionComponent(player1.Model,
                tc1.GetWorld(tc1.UpdateBuffer));

            playerEntity1.Tag = "player";

            var player1C = new PlayerComponent {knockBackResistance = 100};

            componentManager.AddComponent(playerEntity1, moveC1);
            componentManager.AddComponent(playerEntity1, pc1);
            componentManager.AddComponent(playerEntity1, kbc1);
            componentManager.AddComponent(playerEntity1, tc1);
            componentManager.AddComponent(playerEntity1, actColl);
            componentManager.AddComponent(playerEntity1, t1);
            componentManager.AddComponent(playerEntity1, player1);
            componentManager.AddComponent(playerEntity1, player1C);


            // AI
            Entity AIManagerEntity = componentManager.MakeEntity();
            AIManagerComponent AIManagerC = new AIManagerComponent() {
                AIModel = Content.Load<Model>("albin_sphere"),
                spawnAfterSeconds = 5,
                spawnMin = passColl.BoundingBox.Min,
                spawnMax = passColl.BoundingBox.Max,
                spawnTimer = 0
            };
            componentManager.AddComponent(AIManagerEntity, AIManagerC);

            Entity soundEntity = componentManager.MakeEntity();
            SoundEffectComponent soundEffComp = new SoundEffectComponent();

            SoundEffectPiece soundEffect1 = new SoundEffectPiece(Content.Load<SoundEffect>("Sound/fs_general_dirt_01"))
            {
                ActiveTime = 50f,
                CurrentActiveTime = 0f

            };
            soundEffComp.AddSoundEffect("footstep1", soundEffect1);
            SoundEffectPiece soundEffect2 = new SoundEffectPiece(Content.Load<SoundEffect>("Sound/fs_general_dirt_03"))
            {
                ActiveTime = 50f,
                CurrentActiveTime = 50f
            };
            soundEffComp.AddSoundEffect("footstep2", soundEffect2);
            SongComponent songComponent = new SongComponent
            {
                NewActiveSong = Content.Load<Song>("Sound/AfternoonAmbienceSimple_01")
            };
            SongPiece song = new SongPiece(Content.Load<Song>("Sound/AfternoonAmbienceSimple_01"), true);
            songComponent.AddSong("background", song);
            componentManager.AddComponent(playerEntity1, soundEffComp);
            componentManager.AddComponent(soundEntity, songComponent);


            //Particles
            Entity particleSystem = componentManager.MakeEntity();
            ParticleComponent particleComponent = new ParticleComponent(Content, graphics.GraphicsDevice);


            componentManager.AddComponent(particleSystem, particleComponent);
            ParticleComponent.ParticleSystemSettings smoke = new ParticleComponent.ParticleSystemSettings(Content, graphics.GraphicsDevice, "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke2 = new ParticleComponent.ParticleSystemSettings(Content, graphics.GraphicsDevice, "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke3 = new ParticleComponent.ParticleSystemSettings(Content, graphics.GraphicsDevice,  "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke4 = new ParticleComponent.ParticleSystemSettings(Content, graphics.GraphicsDevice,  "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke5 = new ParticleComponent.ParticleSystemSettings(Content, graphics.GraphicsDevice,  "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke6 = new ParticleComponent.ParticleSystemSettings(Content, graphics.GraphicsDevice,  "smoke", "smoke");

            smoke.IsActive = true;
            smoke2.IsActive = true;
            smoke3.IsActive = true;
            smoke4.IsActive = true;
            smoke5.IsActive = true;
            smoke6.IsActive = true;

            smoke.ParticleSettings.Position = new Vector3(650, 20, 700);
            smoke2.ParticleSettings.Position = new Vector3(-700, 20, 650);
            smoke3.ParticleSettings.Position = new Vector3(550, 20, -700);
            smoke4.ParticleSettings.Position = new Vector3(-700, 20, -450);
            smoke5.ParticleSettings.Position = new Vector3(0, 20, -300);
            smoke6.ParticleSettings.Position = new Vector3(0, 20, 300);

            particleComponent.SmokeParticlesList.Add(smoke);
            particleComponent.SmokeParticlesList.Add(smoke2);
            particleComponent.SmokeParticlesList.Add(smoke3);
            particleComponent.SmokeParticlesList.Add(smoke4);
            particleComponent.SmokeParticlesList.Add(smoke5);
            particleComponent.SmokeParticlesList.Add(smoke6);
            */
        }

        public void InitializeSystems()
        {
            /*
            var powerUpSystem = new PowerUpSystem();
            powerUpSystem.content = Content;
            systemManager.EnableFrameCount = true;
            systemManager.AddSystem(new TransformSystem());
            systemManager.AddSystem(new CameraSystem());
            systemManager.AddSystem(new RenderSystem());
            systemManager.AddSystem(new MovementSystem());
            systemManager.AddSystem(new KeyBoardSystem());
            systemManager.AddSystem(new ChaseCameraSystem());
            systemManager.AddSystem(new CollisionDetectionSystem());
            systemManager.AddSystem(new CollisionHandlingSystem());
            systemManager.AddSystem(new PhysicsSystem());
            systemManager.AddSystem(new MouseSystem());
            systemManager.AddSystem(new FreeCameraSystem());
            systemManager.AddSystem(powerUpSystem);
            systemManager.AddSystem(new AIManagerSystem());
            systemManager.AddSystem(new SoundSystem());
            systemManager.AddSystem(new MenuSystem());
            systemManager.AddSystem(new AISystem());
            systemManager.AddSystem(new GameStateSystem());
            systemManager.AddSystem(new ParticleSystem());
            systemManager.AddSystem(new ParticleRenderSystem());
            */
        }
        #endregion
    }
}