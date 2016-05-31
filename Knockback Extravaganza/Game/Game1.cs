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

namespace Game
{

    public class Game1 : ECSEngine
    {

        public Game1() : base()
        {

        }



        protected override void Initialize()
        {
            InitializeSystems();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            CreateEntities();
            spriteBatch = new SpriteBatch(GraphicsDevice);  
        }

        protected override void UnloadContent()
        {

        }
        /// <summary>
        /// Create all entities for the game
        /// </summary>
        public void CreateEntities() {
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
            chase.Offset = new Vector3(0f, 100, 200);
            FreeCameraComponent free = new FreeCameraComponent();
            free.GraphicsDevice = graphics.GraphicsDevice;
            free.Game = this;

            componentManager.AddComponent(camera, chase);
            componentManager.AddComponent(camera, cameraC);
            componentManager.AddComponent(camera, tranformC);
            componentManager.AddComponent(camera, mouse);

            Mouse.SetPosition(free.GraphicsDevice.Viewport.Width / 2, free.GraphicsDevice.Viewport.Height / 2);

            ModelComponent player1 = new ModelComponent();
            player1.Model = Content.Load<Model>("Player");
            ModelTransformComponent t1 = new ModelTransformComponent(player1.Model);

            TransformComponent tc1 = new TransformComponent() {
                Position = new Vector3(0, 0, 0),
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
            kbc1.AddKeyToAction("Jump", Keys.Space);
            kbc1.AddKeyToAction("Dash", Keys.Up);
            kbc1.AddKeyToAction("Reset", Keys.R);

            PhysicsComponent pc1 = new PhysicsComponent {
                InJump = false,
                GravityStrength = 2,
                Mass = 5
            };

            MovementComponent moveC1 = new MovementComponent {
                Acceleration = 2f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            ModelComponent player2 = new ModelComponent();
            player2.Model = Content.Load<Model>("Player");

            ModelTransformComponent t2 = new ModelTransformComponent(player2.Model);

            TransformComponent tc2 = new TransformComponent() {
                Position = new Vector3(10, 0, -50),
                Rotation = new Vector3(0, 0, 0),
                Scale = Vector3.One
            };

            PhysicsComponent pc2 = new PhysicsComponent {
                InJump = false,
                GravityStrength = 2,
                Mass = 5
            };

            MovementComponent moveC2 = new MovementComponent {
                Acceleration = 1.2f,
                Speed = 10,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            MovementComponent moveCCamera = new MovementComponent {
                Acceleration = 1.2f,
                Speed = 10,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            ActiveCollisionComponent actColl = new ActiveCollisionComponent(player1.Model, tc1.World);
            ActiveCollisionComponent actColl2 = new ActiveCollisionComponent(player2.Model, tc2.World);

            playerEntity1.Tag = "player";
            playerEntity2.Tag = "player";

            componentManager.AddComponent(playerEntity1, moveC1);
            componentManager.AddComponent(playerEntity1, pc1);
            componentManager.AddComponent(playerEntity1, kbc1);
            componentManager.AddComponent(playerEntity1, tc1);
            componentManager.AddComponent(playerEntity1, actColl);
            componentManager.AddComponent(playerEntity1, t1);
            componentManager.AddComponent(playerEntity1, player1);

            componentManager.AddComponent(playerEntity2, moveC2);
            componentManager.AddComponent(playerEntity2, pc2);
            componentManager.AddComponent(playerEntity2, tc2);
            componentManager.AddComponent(playerEntity2, actColl2);
            componentManager.AddComponent(playerEntity2, t2);
            componentManager.AddComponent(playerEntity2, player2);


            componentManager.AddComponent(camera, moveCCamera);
            //componentManager.AddComponent(camera, kbc);

            Entity platformEntity = componentManager.MakeEntity();
            platformEntity.Tag = "platform";

            ModelComponent platformModelC = new ModelComponent {
                Model = Content.Load<Model>("platform"),
            };

            TransformComponent platformTransformC = new TransformComponent {
                Position = Vector3.Zero,
                Scale = new Vector3(4, 4, 4)
            };

            PassiveCollisionComponent passColl = new PassiveCollisionComponent(platformModelC.Model, platformTransformC.World);

            componentManager.AddComponent(platformEntity, platformModelC);
            componentManager.AddComponent(platformEntity, platformTransformC);
            componentManager.AddComponent(platformEntity, passColl);

            //Test PowerupBig
            Entity powerUpBigEntity = componentManager.MakeEntity();
            powerUpBigEntity.Tag = "powerUp";
            var powerUpBigModelC = new ModelComponent {
                Model = Content.Load<Model>("box")
            };
            var powerUpBigTransC = new TransformComponent {
                Position = new Vector3(50, 60, -80),
                Rotation = Vector3.Zero,
                Scale = new Vector3(0.5f)
            };
            var powerUpBigPhysicsC = new PhysicsComponent {
                InJump = true,
                GravityStrength = 1,
            };

            var powerUpBigMovementC = new MovementComponent {
                Acceleration = 1f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };


            var powerUpBigPassiveCollC = new PassiveCollisionComponent(powerUpBigModelC.Model, powerUpBigTransC.World);

            componentManager.AddComponent(powerUpBigEntity, powerUpBigModelC);
            componentManager.AddComponent(powerUpBigEntity, powerUpBigTransC);
            componentManager.AddComponent(powerUpBigEntity, powerUpBigPhysicsC);
            componentManager.AddComponent(powerUpBigEntity, powerUpBigPassiveCollC);
            componentManager.AddComponent(powerUpBigEntity, powerUpBigMovementC);


            //Test PowerUpSmall
            Entity powerUpSmallEntity = componentManager.MakeEntity();
            powerUpSmallEntity.Tag = "powerUp";

            var powerUpSmallModelC = new ModelComponent {
                Model = Content.Load<Model>("box")
            };
            var powerUpSmallTransC = new TransformComponent {
                Position = new Vector3(-50, 60, -80),
                Rotation = Vector3.Zero,
                Scale = new Vector3(0.5f)
            };
            var powerUpSmallPhysicsC = new PhysicsComponent {
                InJump = true,
                GravityStrength = 1,
            };

            var powerUpSmallMovementC = new MovementComponent {
                Acceleration = 1f,
                Speed = 0,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            var powerUpSmallPassiveCollC = new PassiveCollisionComponent(powerUpSmallModelC.Model, powerUpSmallTransC.World);

            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallModelC);
            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallTransC);
            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallPhysicsC);
            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallPassiveCollC);
            componentManager.AddComponent(powerUpSmallEntity, powerUpSmallMovementC);
        
            //Test of particle on powerup

            //var smokeParticle = componentManager.MakeEntity();
            /*
                        var particleSettingsC = new ParticleSettings {
                            TextureName = "smoke",
                            MaxParticles = 600,
                            Duration = TimeSpan.FromSeconds(10),
                            MinHorizontalVelocity = 0,
                            MaxHorizontalVelocity = 15,
                            MinVerticalVelocity = 10,
                            MaxVerticalVelocity = 20,
                            Gravity = new Vector3(-20,-5,0),
                            EndVelocity = 0.75f,
                            MinRotateSpeed = -1,
                            MaxRotateSpeed = 1,
                            MinStartSize = 4,
                            MaxStartSize = 7,
                            MinEndSize = 35,
                            MaxEndSize = 140,
                            Type = ParticleType.Smoke,
                            MinColor = Color.White,
                            MaxColor = Color.White

                        };
                        var particleVertexC = new ParticleVertex { SizeInBytes = 40};
                        var particleProjectileC = new ParticleProjectile {
                            TrailParticlesPerSecond = 200,
                            NumExplosionParticles = 30,
                            NumExplosionSmokeParticles = 50,
                            ProjectileLifeSpan = 1.5f,
                            SidewaysVelocityRange = 60,
                            VerticalVelocityRange = 40,
                            Gravity = 15
                        };
                        var particleEmitterC = new ParticleEmitter {
                            ParticleSettings = particleSettingsC,
                            ParticleVertex = particleVertexC,
                            ParticlesPerSecond = 200,
                            Projectile = particleProjectileC,
                        };
                        particleEmitterC.TimeBetweenParticles = 1.0f / particleEmitterC.ParticlesPerSecond;
            };

            //var particleVertexC = new ParticleVertex { SizeInBytes = 40};
            //var particleProjectileC = new ParticleProjectile {
            //    TrailParticlesPerSecond = 200,
            //    NumExplosionParticles = 30,
            //    NumExplosionSmokeParticles = 50,
            //    ProjectileLifeSpan = 1.5f,
            //    SidewaysVelocityRange = 60,
            //    VerticalVelocityRange = 40,
            //    Gravity = 15
            //};
            //var particleEmitterC = new ParticleEmitter {
            //    ParticleSettings = particleSettingsC,
            //    ParticleVertex = particleVertexC,
            //    ParticlesPerSecond = 200,
            //    Projectile = particleProjectileC,
            //};
            //particleEmitterC.TimeBetweenParticles = 1.0f / particleEmitterC.ParticlesPerSecond;

            //componentManager.AddComponent(powerUpBigEntity, particleEmitterC);
            */
            Entity soundEntity = componentManager.MakeEntity();
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
            componentManager.AddComponent(playerEntity1, soundEffComp);
            componentManager.AddComponent(soundEntity, songComponent);
        }




        /// <summary>
        /// Initializes all needed systems
        /// </summary>
        public void InitializeSystems()
        {
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
            systemManager.AddSystem(new PowerUpSystem());
            systemManager.AddSystem(new SoundSystem());
        }
    }
}