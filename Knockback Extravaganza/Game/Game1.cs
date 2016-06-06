using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ECS_Engine;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Systems;
using System.Collections.Generic;
using ECS_Engine.Engine.Component.Interfaces;
using System;
using GameEngine;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using ECS_Engine.Engine.Scenes;
using ECS_Engine.Engine.Systems.ParticleSystems;

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
            var startScene = new Scene { Name = "startScene", Font = startFont, Background = startBackground, SpriteBatch = spriteBatch, menuChoices = new List<string> { "Start Game", "Host Game", "Single Player", "Multiplayer" } };
            sceneManager.AddScene(startScene);

            //Init multiplayer
            var multiplayerScene = new Scene { Name = "multiplayerScene", Font = startFont, Background = startBackground };
            sceneManager.AddScene(multiplayerScene);

            //Init pause
            var pauseScene = new Scene { Name = "pauseScene", Font = startFont, Background = pauseBackground, SpriteBatch = spriteBatch, menuChoices = new List<string> { "Continue", "Settings", "Exit to main menu" } };
            sceneManager.AddScene(pauseScene);

            //Init hostScene
            var hostScene = new Scene { Name = "connectionScene", Background = pauseBackground, SpriteBatch = spriteBatch, Font = startFont, menuChoices = new List<string> { "Host Game", "Join Game", "Exit to main menu" }};
            sceneManager.AddScene(hostScene);

            //Set start scene
            sceneManager.SetCurrentScene("connectionScene");
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
            Entity playerEntity2 = componentManager.MakeEntity();

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
            //tranformC.Position = new Vector3(0f, 20, -10f);
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

            Entity platformEntity = componentManager.MakeEntity();
            platformEntity.Tag = "platform";

            ModelComponent platformModelC = new ModelComponent
            {
                Model = Content.Load<Model>("platform"),
            };

            TransformComponent platformTransformC = new TransformComponent
            {
                Position = Vector3.Zero,
                Scale = new Vector3(4, 4, 4)
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
            kbc1.AddKeyToAction("Dash", Keys.Up);
            kbc1.AddKeyToAction("Reset", Keys.R);
            kbc1.AddKeyToAction("Pause", Keys.Escape);

            PhysicsComponent pc1 = new PhysicsComponent
            {
                InAir = true,
                GravityStrength = 2,
                Mass = 5
            };

            MovementComponent moveC1 = new MovementComponent
            {
                Acceleration = 2f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            ModelComponent player2 = new ModelComponent();
            player2.Model = Content.Load<Model>("Player");

            ModelTransformComponent t2 = new ModelTransformComponent(player2.Model);

            TransformComponent tc2 = new TransformComponent()
            {
                Position = new Vector3(10, 10, -100),
                Rotation = new Vector3(0, 0, 0),
                Scale = Vector3.One
            };

            PhysicsComponent pc2 = new PhysicsComponent
            {
                InAir = false,
                GravityStrength = 2,
                Mass = 5
            };

            MovementComponent moveC2 = new MovementComponent
            {
                Acceleration = 1.2f,
                Speed = 10,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            MovementComponent moveCCamera = new MovementComponent
            {
                Acceleration = 50f,
                Speed = 10,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            ActiveCollisionComponent actColl = new ActiveCollisionComponent(player1.Model,
                tc1.GetWorld(tc1.UpdateBuffer));
            ActiveCollisionComponent actColl2 = new ActiveCollisionComponent(player2.Model,
                tc2.GetWorld(tc2.UpdateBuffer));

            playerEntity1.Tag = "player";
            playerEntity2.Tag = "player";

            var player1C = new PlayerComponent {knockBackResistance = 100};
            var player2C = new PlayerComponent {knockBackResistance = 100};

            componentManager.AddComponent(playerEntity1, moveC1);
            componentManager.AddComponent(playerEntity1, pc1);
            componentManager.AddComponent(playerEntity1, kbc1);
            componentManager.AddComponent(playerEntity1, tc1);
            componentManager.AddComponent(playerEntity1, actColl);
            componentManager.AddComponent(playerEntity1, t1);
            componentManager.AddComponent(playerEntity1, player1);
            componentManager.AddComponent(playerEntity1, player1C);

            componentManager.AddComponent(playerEntity2, moveC2);
            componentManager.AddComponent(playerEntity2, pc2);
            componentManager.AddComponent(playerEntity2, tc2);
            componentManager.AddComponent(playerEntity2, actColl2);
            componentManager.AddComponent(playerEntity2, t2);
            componentManager.AddComponent(playerEntity2, player2);
            componentManager.AddComponent(playerEntity2, player2C);


            componentManager.AddComponent(camera, moveCCamera);
            //componentManager.AddComponent(camera, kbc);



            //Test PowerupBig
            Entity powerUpBigEntity = componentManager.MakeEntity();
            powerUpBigEntity.Tag = "powerUp";
            var powerUpBigModelC = new ModelComponent
            {
                Model = Content.Load<Model>("box")
            };
            var powerUpBigTransC = new TransformComponent
            {
                Position = new Vector3(50, 60, -80),
                Rotation = Vector3.Zero,
                Scale = new Vector3(0.5f)
            };
            var powerUpBigPhysicsC = new PhysicsComponent
            {
                InAir = true,
                GravityStrength = 1,
            };

            var powerUpBigMovementC = new MovementComponent
            {
                Acceleration = 1f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            var powerUpBigPassiveCollC = new ActiveCollisionComponent(powerUpBigModelC.Model,
                powerUpBigTransC.GetWorld(powerUpBigTransC.UpdateBuffer));

            componentManager.AddComponent(powerUpBigEntity, powerUpBigModelC);
            componentManager.AddComponent(powerUpBigEntity, powerUpBigTransC);
            componentManager.AddComponent(powerUpBigEntity, powerUpBigPhysicsC);
            componentManager.AddComponent(powerUpBigEntity, powerUpBigPassiveCollC);
            componentManager.AddComponent(powerUpBigEntity, powerUpBigMovementC);


            //Test PowerUpSmall
            Entity powerUpSmallEntity = componentManager.MakeEntity();
            powerUpSmallEntity.Tag = "powerUp";

            var powerUpSmallModelC = new ModelComponent
            {
                Model = Content.Load<Model>("box")
            };
            var powerUpSmallTransC = new TransformComponent
            {
                Position = new Vector3(-50, 60, -80),
                Rotation = Vector3.Zero,
                Scale = new Vector3(0.5f)
            };
            var powerUpSmallPhysicsC = new PhysicsComponent
            {
                InAir = true,
                GravityStrength = 1,
            };

            var powerUpSmallMovementC = new MovementComponent
            {
                Acceleration = 1f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            var powerUpSmallPassiveCollC = new ActiveCollisionComponent(powerUpSmallModelC.Model,
                powerUpSmallTransC.GetWorld(powerUpSmallTransC.UpdateBuffer));

            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallModelC);
            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallTransC);
            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallPhysicsC);
            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallPassiveCollC);
            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallMovementC);

            var aiEntity = componentManager.MakeEntity();
            var aiAiC = new AIComponent {Duration = 10000, Target = 1};
            var aiTransformC = new TransformComponent
            {
                Position = new Vector3(-0, 60, -200),
                Rotation = Vector3.Zero,
                Scale = new Vector3(1)
            };
            var aiModelTransC = new ModelComponent
            {
                Model = Content.Load<Model>("albin_sphere")
            };
            var aiPhysicsC = new PhysicsComponent
            {
                GravityStrength = 0.5f,
                Mass = 5,
                InAir = true
            };
            var aimoveC = new MovementComponent
            {
                //Acceleration = 0f,
                //Speed = 5,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };


            componentManager.AddComponent(aiEntity, aiAiC, aiTransformC, aimoveC, aiModelTransC, aiPhysicsC);

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

            Entity particleSystem = componentManager.MakeEntity();
            ParticleComponent particleComponent = new ParticleComponent(Content, graphics.GraphicsDevice);

            componentManager.AddComponent(particleSystem, particleComponent);
            particleComponent.ActivateSmoke = true;
        }

        public void InitializeSystems()
        {
            var powerUpSystem = new PowerUpSystem();
            powerUpSystem.content = Content;
            systemManager.EnableFrameCount = true;
            systemManager.AddSystem(new TransformSystem());
            systemManager.AddSystem(new CameraSystem());
            systemManager.AddSystem(new ModelRenderSystem());
            systemManager.AddSystem(new MovementSystem());
            systemManager.AddSystem(new KeyBoardSystem());
            systemManager.AddSystem(new ChaseCameraSystem());
            systemManager.AddSystem(new CollisionDetectionSystem());
            systemManager.AddSystem(new CollisionHandlingSystem());
            systemManager.AddSystem(new PhysicsSystem());
            systemManager.AddSystem(new MouseSystem());
            systemManager.AddSystem(new FreeCameraSystem());
            systemManager.AddSystem(powerUpSystem);
            systemManager.AddSystem(new SoundSystem());
            systemManager.AddSystem(new MenuSystem());
            systemManager.AddSystem(new AISystem());
            systemManager.AddSystem(new ParticleSystem());
            systemManager.AddSystem(new ParticleRenderSystem());
        }
    }
}