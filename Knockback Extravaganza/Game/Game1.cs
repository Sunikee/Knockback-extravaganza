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

        Entity mesh = new Entity();

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
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
            chase.Target = mesh;
            chase.Offset = new Vector3(0, 25, 25f);

            componentManager.AddComponent(camera, chase);
            componentManager.AddComponent(camera, cameraC);
            componentManager.AddComponent(camera, tranformC);

            Entity chopper = new Entity();
            ModelComponent mc = new ModelComponent {
                Model = Content.Load<Model>("chopper")
            };
            TransformComponent tc = new TransformComponent {
                Position = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0),
                Scale = new Vector3(1, 1, 1)
            };

            KeyBoardComponent kbc = new KeyBoardComponent();
            kbc.AddKeyToAction("Down", Keys.W);
            kbc.AddKeyToAction("Up", Keys.S);
            kbc.AddKeyToAction("Right", Keys.D);
            kbc.AddKeyToAction("Left", Keys.A);
            kbc.AddKeyToAction("Move", Keys.Space);

            PhysicsComponent pc = new PhysicsComponent
            {
                InAir = true,
                GravityStrength = 5
            };

            MovementComponent mmc = new MovementComponent
            {
                Acceleration = 0,
                Speed = 1,
                Velocity = Vector3.Zero
            };


            componentManager.AddComponent(chopper, mmc);
            componentManager.AddComponent(chopper, pc);
            //componentManager.AddComponent(chopper, pc);
            componentManager.AddComponent(chopper, kbc);
            componentManager.AddComponent(chopper, mc);
            componentManager.AddComponent(chopper, tc);

            Entity model = new Entity();

            VertexPositionColor[] v = new VertexPositionColor[4];
            v[0] = new VertexPositionColor(new Vector3(-1, 1, 0), Color.Red);
            v[1] = new VertexPositionColor(new Vector3(1, 1, 0), Color.Blue);
            v[2] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Green);
            v[3] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Yellow);

            int[] i = new int[6];
            i[0] = 0;
            i[1] = 1;
            i[2] = 3;

            i[3] = 0;
            i[4] = 3;
            i[5] = 2;

            VertexIndexBufferComponent<VertexPositionColor> vert = new VertexIndexBufferComponent<VertexPositionColor>(v, i, graphics.GraphicsDevice);
            vert.Type = PrimitiveType.TriangleList;

            vert.Effect = new BasicEffect(graphics.GraphicsDevice);
            vert.Effect.VertexColorEnabled = true;
            vert.Effect.Projection = cameraC.Projection;

            TransformComponent tranformV = new TransformComponent();
            tranformV.Scale *= 3;
            componentManager.AddComponent(model, vert);
            componentManager.AddComponent(model, tranformV);

            TransformComponent tra = new TransformComponent();
            tra.Position = Vector3.Zero;
            componentManager.AddComponent(mesh, tra);

        

            systemManager.AddSystem(new TransformSystem());
            systemManager.AddSystem(new CameraSystem());
            systemManager.AddSystem(new VertexIndexBufferRenderSystem<VertexPositionColor>());
            systemManager.AddSystem(new ModelRenderSystem());
            systemManager.AddSystem(new MovementSystem());
            systemManager.AddSystem(new KeyBoardSystem());
            systemManager.AddSystem(new ChaseCameraSystem());

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ModelComponent m = new ModelComponent();
            m.Model = Content.Load<Model>("Player");
            ModelTransformComponent t = new ModelTransformComponent(m.Model);
            componentManager.AddComponent(mesh, t);
            componentManager.AddComponent(mesh, m);
        }

        protected override void UnloadContent()
        {
            
        }
    }
}
