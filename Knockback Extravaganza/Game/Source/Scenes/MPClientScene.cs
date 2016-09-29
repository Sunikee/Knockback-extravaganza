using ECS_Engine.Engine.Network;
using ECS_Engine.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using ECS_Engine.Engine.Component.Network;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine;
using Game.Source.Components;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Game.Source.Systems.Network;
using ECS_Engine.Engine.Systems;
using ECS_Engine.Engine.Systems.ParticleSystems;
using Game.Source.Systems.AI;
using Game.Source.Systems;
using ECS_Engine.Engine.Systems.Network;

namespace Game.Source.Scenes {
    class MPClientScene : Scene {

        public MPClientScene(string name, ContentManager content, GraphicsDeviceManager graphics) : base(name, content, graphics) { }

        public override void InitScene() {      

            var clientEntiry = ComponentManager.MakeEntity();

            var client = new NetworkClientComponent();
            client.Client.Connect("localhost", 14242);

            ComponentManager.AddComponent(clientEntiry, client);

            var dataEntity = ComponentManager.MakeEntity();

            var data = new NetworkDataComponent();
            ComponentManager.AddComponent(dataEntity, data);

            SystemManager.AddSystem(new NetworkClientSystem());


            var entity = ComponentManager.MakeEntity();
            entity.Tag = "player";
            ModelComponent player1 = new ModelComponent();
            player1.Model = Content.Load<Model>("Player");

            ModelTransformComponent t1 = new ModelTransformComponent(player1.Model);

            TransformComponent tc1 = new TransformComponent() {
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

            PhysicsComponent pc1 = new PhysicsComponent {
                InAir = false,
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

            ComponentManager.AddComponent(entity, moveC1);
            ComponentManager.AddComponent(entity, pc1);
            ComponentManager.AddComponent(entity, kbc1);
            ComponentManager.AddComponent(entity, tc1);
            ComponentManager.AddComponent(entity, actColl);
            ComponentManager.AddComponent(entity, t1);
            ComponentManager.AddComponent(entity, player1);
            ComponentManager.AddComponent(entity, player1C);

            ComponentManager.AddComponent(entity, new NetworkSendTransformComponent());

            SystemManager.AddSystem(new NetworkClientSystem());


            Entity camera = ComponentManager.MakeEntity();

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
            chase.Target = entity;
            chase.Offset = new Vector3(0f, 200, 400);

            ComponentManager.AddComponent(camera, cameraC, tranformC, chase);

            Entity platformEntity = ComponentManager.MakeEntity();
            platformEntity.Tag = "platform";

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



            var powerUpSystem = new PowerUpSystem();
            powerUpSystem.content = Content;
            SystemManager.EnableFrameCount = true;

            SystemManager.AddSystem(new TransformSystem());
            SystemManager.AddSystem(new CameraSystem());
            SystemManager.AddSystem(new ModelRenderSystem());
            SystemManager.AddSystem(new MovementSystem());
            SystemManager.AddSystem(new KeyBoardSystem());
            SystemManager.AddSystem(new ChaseCameraSystem());
            //SystemManager.AddSystem(new CollisionDetectionSystem());
            //SystemManager.AddSystem(new CollisionHandlingSystem());
            SystemManager.AddSystem(new PhysicsSystem());
            //SystemManager.AddSystem(powerUpSystem);
            //SystemManager.AddSystem(new AIManagerSystem());
            SystemManager.AddSystem(new SoundSystem());
            //SystemManager.AddSystem(new AISystem());
            //SystemManager.AddSystem(new ParticleSystem());
            //SystemManager.AddSystem(new ParticleRenderSystem());
            
            SystemManager.AddSystem(new NetworkHandelerSystem());
            SystemManager.AddSystem(new NetworkTransformSystem());

            base.InitScene();
        }

        public override void ResetScene() {

        }
    }
}
