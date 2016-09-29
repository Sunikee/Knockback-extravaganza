using ECS_Engine.Engine;
using ECS_Engine.Engine._2D_Components;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Component.Interfaces;
using ECS_Engine.Engine.Scenes;
using ECS_Engine.Engine.Systems;
using Game.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Source.Scenes
{
    public class GameOverScene : Scene
    {
        public GameOverScene(string name, ContentManager content, GraphicsDeviceManager graphics) : base(name, content, graphics) { }

        public override void InitScene()
        {
            Entity background = ComponentManager.MakeEntity();
            var bgSprite = new Texture2DComponent()
            {
                Texture = Content.Load<Texture2D>("Scenes\\startBackground2"),
                Color = Color.White,
            };
            var pos = new Position2DComponent()
            {
                Postion = Vector2.Zero,
            };
            ComponentManager.AddComponent(background, bgSprite, pos);

            var goTextEntity = ComponentManager.MakeEntity();
          
            var goText = new SpriteTextComponent()
            {
                SpriteFont = Content.Load<SpriteFont>("Scenes\\Font1"),
                Text = "Game over",
                Color = Color.Red
            };
            var goPosText = new Position2DComponent { Postion = new Vector2 (400, 300) };

            ComponentManager.AddComponent(goTextEntity, goText, goPosText);

            SystemManager.AddSystem(new SpriteRenderSystem());
            SystemManager.AddSystem(new KeyBoardSystem());
            SystemManager.AddSystem(new MenuSystem());
            SystemManager.AddSystem(new GameOverSystem());

            base.InitScene();
        }

        public override void ResetScene()
        {
            throw new NotImplementedException();
        }

        public override void DestroyScene()
        {
            base.DestroyScene();
        }
    }
}
