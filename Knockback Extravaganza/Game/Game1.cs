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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            Entity camera = new Entity("camera");
            CameraComponent cameraC = new CameraComponent();
            cameraC.FieldOfView = MathHelper.PiOver4;
            cameraC.AspectRatio = graphics.GraphicsDevice.DisplayMode.AspectRatio;
            cameraC.FarPlaneDistace = 100f;
            cameraC.NearPlaneDistace = 1f;
            cameraC.Target = Vector3.Zero;
            cameraC.Up = Vector3.Up;
            TransformComponent tranformC = new TransformComponent();
            tranformC.Position = new Vector3(0, 0, 5f);
            tranformC.Rotation = Vector3.Zero;
            tranformC.Scale = Vector3.One;
            BasicEffectComponent basic = new BasicEffectComponent();
            basic.Effect = new BasicEffect(graphics.GraphicsDevice);
            basic.Effect.VertexColorEnabled = true;
            basic.Effect.Projection = cameraC.Projection;

            componentManager.AddComponent(camera, cameraC);
            componentManager.AddComponent(camera, tranformC);
            componentManager.AddComponent(camera, basic);

            Entity model = new Entity("plane");
            VertexPosColorComponent vert = new VertexPosColorComponent();
            VertexPositionColor[] v = new VertexPositionColor[6];
            v[0] = new VertexPositionColor(new Vector3(-1, 1 ,0), Color.Green);
            v[1] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Green);
            v[2] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Green);

            v[3] = new VertexPositionColor(new Vector3(-1, 1, 0), Color.Red);
            v[4] = new VertexPositionColor(new Vector3(1, 1, 0), Color.Red);
            v[5] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Red);
            vert.Vertices = v;
            vert.Type = PrimitiveType.TriangleList;
            TransformComponent tranformV = new TransformComponent();
            tranformV.Position = Vector3.Zero;
            tranformV.Rotation = new Vector3(0, 0, 0);
            tranformV.Scale = new Vector3(2,2,2);
            componentManager.AddComponent(model, vert);
            componentManager.AddComponent(model, tranformV);

            systemManager.AddSystem(new CameraSystem());
            systemManager.AddSystem(new RenderVertexSystem());

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

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
