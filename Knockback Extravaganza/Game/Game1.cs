using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ECS_Engine;
using ECS_Engine.Engine.Managers;
using ECS_Engine.Engine.Systems.Interfaces;
using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Systems;

namespace Game {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : ECSEngine {

        public Game1() : base(){
            
        }

        Entity mesh = new Entity();

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
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

            

            componentManager.AddComponent(camera, cameraC);
            componentManager.AddComponent(camera, tranformC);


            Entity model = new Entity();

            VertexPositionColor[] v = new VertexPositionColor[4];
            v[0] = new VertexPositionColor(new Vector3(-1, 1 ,0), Color.Red);
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

            systemManager.AddSystem(new CameraSystem());
            systemManager.AddSystem(new VertexIndexBufferRenderSystem<VertexPositionColor>());
            systemManager.AddSystem(new ModelRenderSystem());

            TransformComponent tra = new TransformComponent();
            componentManager.AddComponent(mesh, tra);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ModelComponent m = new ModelComponent();
            m.Model = Content.Load<Model>("Chopper");
            ModelTransformComponent t = new ModelTransformComponent(m.Model);
            componentManager.AddComponent(mesh, t);
            componentManager.AddComponent(mesh, m);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }


    }
}
