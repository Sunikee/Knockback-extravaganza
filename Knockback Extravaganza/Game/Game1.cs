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

        Entity playerEntity = new Entity();

        protected override void Initialize()
        {
            
            Entity camera = new Entity();
            CameraComponent cameraC = new CameraComponent();
            cameraC.FieldOfView = MathHelper.PiOver4;
            cameraC.AspectRatio = graphics.GraphicsDevice.DisplayMode.AspectRatio;
            cameraC.FarPlaneDistace = 100f;
            cameraC.NearPlaneDistace = 1f;
            cameraC.Target = Vector3.Zero;
            cameraC.Up = Vector3.Up;
            TransformComponent tranformC = new TransformComponent();
            tranformC.Position = new Vector3(0f, 0, 25f);
            ChaseCameraComponent chase = new ChaseCameraComponent();
            chase.Target = playerEntity;
            chase.Offset = new Vector3(0, 20, 50);

            componentManager.AddComponent(camera, chase);
            componentManager.AddComponent(camera, cameraC);
            componentManager.AddComponent(camera, tranformC);

            ModelComponent player = new ModelComponent();
            player.Model = Content.Load<Model>("Player");
            ModelTransformComponent t = new ModelTransformComponent(player.Model);
            componentManager.AddComponent(playerEntity, t);
            componentManager.AddComponent(playerEntity, player);

            TransformComponent tc = new TransformComponent()
            {
                Position = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0),
                Scale = new Vector3(.2f, .2f, .2f)
            };

            KeyBoardComponent kbc = new KeyBoardComponent();
            kbc.AddKeyToAction("Forward", Keys.W);
            kbc.AddKeyToAction("Backward", Keys.S);
            kbc.AddKeyToAction("Right", Keys.D);
            kbc.AddKeyToAction("Left", Keys.A);
            kbc.AddKeyToAction("Jump", Keys.Space);

            PhysicsComponent pc = new PhysicsComponent
            {
                InJump = true,
                GravityStrength = 2
            };

            MovementComponent moveC = new MovementComponent
            {
                Acceleration = 1.2f,
                Speed = 10,
            };

            ActiveCollisionComponent actColl = new ActiveCollisionComponent(player.Model, tc.World);


            componentManager.AddComponent(playerEntity, moveC);
            componentManager.AddComponent(playerEntity, pc);
            componentManager.AddComponent(playerEntity, kbc);
            componentManager.AddComponent(playerEntity, tc);
            componentManager.AddComponent(playerEntity, actColl);

            Entity platformEntity = new Entity();

            ModelComponent platformModelC = new ModelComponent
            {
                Model = Content.Load<Model>("platform"),
            };
            TransformComponent platformTransformC = new TransformComponent
            {
                Position = Vector3.Zero,
                Scale = Vector3.One
            };

            PassiveCollisionComponent passColl = new PassiveCollisionComponent(platformModelC.Model, platformTransformC.World);

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