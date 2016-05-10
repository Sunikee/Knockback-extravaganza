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

namespace Game
{

    public class Game1 : ECSEngine
    {

        public Game1() : base()
        {

        }

        Entity playerEntity1;
        Entity playerEntity2;

        protected override void Initialize()
        {
            playerEntity1 = componentManager.MakeEntity();
            playerEntity2 = componentManager.MakeEntity();

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
            chase.Offset = new Vector3(0f, 50f, 90f);
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
            
            TransformComponent tc1 = new TransformComponent()
            {
                Position = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0),
                Scale = Vector3.One
            };

            KeyBoardComponent kbc1 = new KeyBoardComponent();
            kbc1.AddKeyToAction("Forward", Keys.W);
            kbc1.AddKeyToAction("Backward", Keys.S);
            kbc1.AddKeyToAction("Right", Keys.D);
            kbc1.AddKeyToAction("Left", Keys.A);
            kbc1.AddKeyToAction("Jump", Keys.Space);
            kbc1.AddKeyToAction("Dash", Keys.Q);

            PhysicsComponent pc1 = new PhysicsComponent
            {
                InJump = false,
                GravityStrength = 2,
                Mass = 5
            };

            MovementComponent moveC1 = new MovementComponent
            {
                Acceleration = 1.2f,
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

            PhysicsComponent pc2 = new PhysicsComponent
            {
                InJump = false,
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
                Acceleration = 1.2f,
                Speed = 10,
                Velocity = Vector3.Zero,
                AirTime = 0f
            };

            ActiveCollisionComponent actColl = new ActiveCollisionComponent();
            ActiveCollisionComponent actColl2 = new ActiveCollisionComponent();

            AnimationComponent animationComponent1 = new AnimationComponent();
            AnimationComponent animationComponent2 = new AnimationComponent();

            componentManager.AddComponent(playerEntity1, moveC1);
            componentManager.AddComponent(playerEntity1, pc1);
            componentManager.AddComponent(playerEntity1, kbc1);
            componentManager.AddComponent(playerEntity1, tc1);
            componentManager.AddComponent(playerEntity1, actColl);
            componentManager.AddComponent(playerEntity1, t1);
            componentManager.AddComponent(playerEntity1, player1);
            componentManager.AddComponent(playerEntity1, animationComponent1);

            componentManager.AddComponent(playerEntity2, moveC2);
            componentManager.AddComponent(playerEntity2, pc2);
            componentManager.AddComponent(playerEntity2, tc2);
            componentManager.AddComponent(playerEntity2, actColl2);
            componentManager.AddComponent(playerEntity2, t2);
            componentManager.AddComponent(playerEntity2, player2);
            componentManager.AddComponent(playerEntity2, animationComponent2);


            componentManager.AddComponent(camera, moveCCamera);
            //componentManager.AddComponent(camera, kbc);

            Entity platformEntity = componentManager.MakeEntity();

            ModelComponent platformModelC = new ModelComponent {
                Model = Content.Load<Model>("platform"),
            };

            TransformComponent platformTransformC = new TransformComponent
            {
                Position = Vector3.Zero,
                Scale = Vector3.One
            };

            PassiveCollisionComponent passColl = new PassiveCollisionComponent();

            componentManager.AddComponent(platformEntity, platformModelC);
            componentManager.AddComponent(platformEntity, platformTransformC);
            componentManager.AddComponent(platformEntity, passColl);

            systemManager.AddSystem(new TransformSystem());
            systemManager.AddSystem(new CameraSystem());
            systemManager.AddSystem(new ModelRenderSystem());
            systemManager.AddSystem(new MovementSystem());
            systemManager.AddSystem(new KeyBoardSystem());
            systemManager.AddSystem(new ChaseCameraSystem());
            systemManager.AddSystem(new CollisionDetectionSystem());
            systemManager.AddSystem(new PhysicsSystem());
            systemManager.AddSystem(new MouseSystem());
            systemManager.AddSystem(new FreeCameraSystem());
            systemManager.AddSystem(new PlayerAnimationSystem());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

           
        }

        protected override void UnloadContent()
        {

        }
    }
}