using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Scenes;
using ECS_Engine.Engine.Systems;
using Game.Source.Systems.AI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Scenes {
    public class SinglePlayerScene : Scene {

        public SinglePlayerScene(string name, ContentManager content, GraphicsDeviceManager graphics) : base(name, content, graphics) { }

        public override void InitScene() {
            base.InitScene();

            #region Component Initialisation

            #region Camera

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

            ComponentManager.AddComponent(camera, cameraC);
            ComponentManager.AddComponent(camera, tranformC);

            #endregion

            #region Platform

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

            #endregion



            #endregion


            #region System Initialisation

            SystemManager.AddSystem(new TransformSystem());
            SystemManager.AddSystem(new CameraSystem());
            SystemManager.AddSystem(new ModelRenderSystem());
            SystemManager.EnableFrameCount = true;

            #endregion
        }

        public override void ResetScene() {
            throw new NotImplementedException();
        }
    }
}
