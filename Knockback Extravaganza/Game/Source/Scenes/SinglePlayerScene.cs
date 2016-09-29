using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Scenes;
using ECS_Engine.Engine.Systems;
using ECS_Engine.Engine.Systems.ParticleSystems;
using Game.Source.Components;
using Game.Source.Components.AI;
using Game.Source.Systems;
using Game.Source.Systems.AI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Scenes {
    public class SinglePlayerScene : Scene {

        public SinglePlayerScene(string name, ContentManager content, GraphicsDeviceManager graphics) : base(name, content, graphics) { }

        public override void InitScene() {
            

            #region Entity Initialisation

            Entity playerEntity1 = ComponentManager.MakeEntity();
            playerEntity1.Tag = "player";

            Entity camera = ComponentManager.MakeEntity();

            Entity platformEntity = ComponentManager.MakeEntity();
            platformEntity.Tag = "platform";

            Entity AIManagerEntity = ComponentManager.MakeEntity();
            Entity powerUpSettingsEntity = ComponentManager.MakeEntity();

            Entity particleSystem = ComponentManager.MakeEntity();

            Entity soundEntity = ComponentManager.MakeEntity();

            #endregion


            #region Component Initialisation

            #region Camera


            CameraComponent cameraC = new CameraComponent();
            cameraC.FieldOfView = MathHelper.PiOver4;
            cameraC.AspectRatio = Graphics.GraphicsDevice.DisplayMode.AspectRatio;
            cameraC.FarPlaneDistace = 10000f;
            cameraC.NearPlaneDistace = .1f;
            cameraC.Target = Vector3.Zero;
            cameraC.Up = Vector3.Up;

            TransformComponent tranformC = new TransformComponent();
            tranformC.Position = new Vector3(0f, 2000f, 2500f);

            ChaseCameraComponent chase = new ChaseCameraComponent();
            chase.Target = playerEntity1;
            chase.Offset = new Vector3(0f, 200, 400);

            ComponentManager.AddComponent(camera, cameraC, tranformC, chase);

            #endregion

            #region Platform

            ModelComponent platformModelC = new ModelComponent {
                Model = Content.Load<Model>("platform"),
            };
            TransformComponent platformTransformC = new TransformComponent {
                Position = Vector3.Zero,
                Scale = new Vector3(4)
            };
            PassiveCollisionComponent passColl = new PassiveCollisionComponent(platformModelC.Model,
                platformTransformC.GetWorld(platformTransformC.UpdateBuffer));
            ComponentManager.AddComponent(platformEntity, platformModelC);
            ComponentManager.AddComponent(platformEntity, platformTransformC);
            ComponentManager.AddComponent(platformEntity, passColl);

            #endregion

            #region Player

            // Player
            ModelComponent player1 = new ModelComponent();
            player1.Model = Content.Load<Model>("Player");
            ModelTransformComponent t1 = new ModelTransformComponent(player1.Model);

            TransformComponent tc1 = new TransformComponent() {
                Position = new Vector3(0, 10, 0),
                Rotation = new Vector3(0, 0, 0),
                Scale = Vector3.One
            };

            ScoreTimeComponent st1 = new ScoreTimeComponent()
            {
                Score = 0,
                ElapsedTime = 0f,
                spriteBatch = new SpriteBatch(Graphics.GraphicsDevice),
                spriteFont = Content.Load<SpriteFont>("score"),
                stopWatch = new Stopwatch(),
                texture = Content.Load<Texture2D>("lava_texture"),
                TimerDuration = 1f,
            };
            st1.stopWatch.Start();

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

            PhysicsComponent pc1 = new PhysicsComponent {
                InAir = true,
                GravityStrength = 4,
                Mass = 150
            };

            MovementComponent moveC1 = new MovementComponent {
                Acceleration = 2f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };


            ActiveCollisionComponent actColl = new ActiveCollisionComponent(player1.Model,
                tc1.GetWorld(tc1.UpdateBuffer));

            

            var player1C = new PlayerComponent { knockBackResistance = 100 };

            ComponentManager.AddComponent(playerEntity1, st1);
            ComponentManager.AddComponent(playerEntity1, moveC1);
            ComponentManager.AddComponent(playerEntity1, pc1);
            ComponentManager.AddComponent(playerEntity1, kbc1);
            ComponentManager.AddComponent(playerEntity1, tc1);
            ComponentManager.AddComponent(playerEntity1, actColl);
            ComponentManager.AddComponent(playerEntity1, t1);
            ComponentManager.AddComponent(playerEntity1, player1);
            ComponentManager.AddComponent(playerEntity1, player1C);

            #endregion

            #region AI Agent

            var aiManagerC = new AIManagerComponent() {
                AIModel = Content.Load<Model>("albin_sphere"),
                spawnAfterSeconds = 3,
                spawnMin = passColl.BoundingBox.Min *4,
                spawnMax = passColl.BoundingBox.Max * 4,
                spawnTimer = 0,
           
            };
            ComponentManager.AddComponent(AIManagerEntity, aiManagerC);

            #endregion

            #region PowerUp

            var powerUpSettingsComponent = new PowerUpSettingsComponent {
                maxCoordX = passColl.BoundingBox.Max.X,
                minCoordX = passColl.BoundingBox.Min.X,
                maxCoordZ = passColl.BoundingBox.Max.Z,
                minCoordZ = passColl.BoundingBox.Min.Z,
                random = new Random(),
                powerUpSpawnTimer = 0
            };

            ComponentManager.AddComponent(powerUpSettingsEntity, powerUpSettingsComponent);

            #endregion

            #region Sound

            SoundEffectComponent soundEffComp = new SoundEffectComponent();

            SoundEffectPiece soundEffect1 = new SoundEffectPiece(Content.Load<SoundEffect>("Sound/fs_general_dirt_01")) {
                ActiveTime = 50f,
                CurrentActiveTime = 0f

            };
            soundEffComp.AddSoundEffect("footstep1", soundEffect1);
            SoundEffectPiece soundEffect2 = new SoundEffectPiece(Content.Load<SoundEffect>("Sound/fs_general_dirt_03")) {
                ActiveTime = 50f,
                CurrentActiveTime = 50f
            };
            soundEffComp.AddSoundEffect("footstep2", soundEffect2);
            SongComponent songComponent = new SongComponent {
                NewActiveSong = Content.Load<Song>("Sound/AfternoonAmbienceSimple_01")
            };
            SongPiece song = new SongPiece(Content.Load<Song>("Sound/AfternoonAmbienceSimple_01"), true);
            songComponent.AddSong("background", song);
            ComponentManager.AddComponent(playerEntity1, soundEffComp);
            ComponentManager.AddComponent(soundEntity, songComponent);

            #endregion

            #region Particles

            ParticleComponent particleComponent = new ParticleComponent(Content, Graphics.GraphicsDevice);

            ComponentManager.AddComponent(particleSystem, particleComponent);
            ParticleComponent.ParticleSystemSettings smoke = new ParticleComponent.ParticleSystemSettings(Content, Graphics.GraphicsDevice, "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke2 = new ParticleComponent.ParticleSystemSettings(Content, Graphics.GraphicsDevice, "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke3 = new ParticleComponent.ParticleSystemSettings(Content, Graphics.GraphicsDevice, "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke4 = new ParticleComponent.ParticleSystemSettings(Content, Graphics.GraphicsDevice, "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke5 = new ParticleComponent.ParticleSystemSettings(Content, Graphics.GraphicsDevice, "smoke", "smoke");
            ParticleComponent.ParticleSystemSettings smoke6 = new ParticleComponent.ParticleSystemSettings(Content, Graphics.GraphicsDevice, "smoke", "smoke");

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

            #endregion

            #endregion


            #region System Initialisation

            var powerUpSystem = new PowerUpSystem();
            powerUpSystem.content = Content;
            SystemManager.EnableFrameCount = true;

            SystemManager.AddSystem(new GameStateSystem());
            SystemManager.AddSystem(new GameOverSystem());
            SystemManager.AddSystem(new ScoreTimeSystem());
            SystemManager.AddSystem(new TransformSystem());
            SystemManager.AddSystem(new CameraSystem());
            SystemManager.AddSystem(new ModelRenderSystem());
            SystemManager.AddSystem(new MovementSystem());
            SystemManager.AddSystem(new KeyBoardSystem());
            SystemManager.AddSystem(new ChaseCameraSystem());
            SystemManager.AddSystem(new CollisionDetectionSystem());
            SystemManager.AddSystem(new CollisionHandlingSystem());
            //SystemManager.AddSystem(new PhysicsSystem());
            SystemManager.AddSystem(powerUpSystem);
            SystemManager.AddSystem(new AIManagerSystem());
            SystemManager.AddSystem(new SoundSystem());
            SystemManager.AddSystem(new AISystem());
            SystemManager.AddSystem(new ParticleSystem());
            SystemManager.AddSystem(new ParticleRenderSystem());

            #endregion

            base.InitScene();
        }

        public override void ResetScene() {
            throw new NotImplementedException();
        }
    }
}
