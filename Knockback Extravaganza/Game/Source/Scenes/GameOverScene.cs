using ECS_Engine.Engine;
using ECS_Engine.Engine._2D_Components;
using ECS_Engine.Engine.Component;
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

            Entity gameOverEntity = ComponentManager.MakeEntity();
            var menuC = new GameOverComponent
            {
                ActiveChoice = 0,
                ActiveColor = Color.Red,
                InactiveColor = Color.White,
                MenuChoicesSpacing = 150
            };
            gameOverEntity.Tag = "gameover";

            KeyBoardComponent menukeysC = new KeyBoardComponent();
            menukeysC.AddKeyToAction("Quit", Keys.Escape);
            menukeysC.AddKeyToAction("Restart", Keys.Enter);
            ComponentManager.AddComponent(gameOverEntity, menuC);
            ComponentManager.AddComponent(gameOverEntity, menukeysC);


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

            Entity text1 = ComponentManager.MakeEntity();
            var txt = new SpriteTextComponent()
            {
                SpriteFont = Content.Load<SpriteFont>("Scenes\\Font1"),
                Text = "Press Enter to Restart Game",
                Color = Color.Red,
            };
            var postext = new Position2DComponent()
            {
                Postion = new Vector2(200, 100),
            };

            Entity text2 = ComponentManager.MakeEntity();
            var txt2 = new SpriteTextComponent()
            {
                SpriteFont = Content.Load<SpriteFont>("Scenes\\Font1"),
                Text = "Press Escape to Quit Game",
                Color = Color.Red,
            };
            var postext2 = new Position2DComponent()
            {
                Postion = new Vector2(200, 200),
            };

            ComponentManager.AddComponent(text1, txt, postext);
            ComponentManager.AddComponent(text2, txt2, postext2);

            SystemManager.AddSystem(new SpriteRenderSystem());
            SystemManager.AddSystem(new KeyBoardSystem());
            SystemManager.AddSystem(new MenuSystem());

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
