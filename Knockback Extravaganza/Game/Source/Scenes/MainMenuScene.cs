using ECS_Engine.Engine;
using ECS_Engine.Engine.Component;
using ECS_Engine.Engine.Scenes;
using ECS_Engine.Engine.Systems;
using Game.Source.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS_Engine.Engine._2D_Components;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Source.Scenes {
    public class MainMenuScene : Scene {

        public MainMenuScene(string name, ContentManager content, GraphicsDeviceManager graphics) : base(name, content, graphics) { }

        public override void InitScene() {
            
            Entity menuEntity = ComponentManager.MakeEntity();
            var menuC = new MenuComponent {
                ActiveChoice = 0,
                ActiveColor = Color.Yellow,
                InactiveColor = Color.Red,
                MenuChoicesSpacing = 150
            };
            KeyBoardComponent menukeysC = new KeyBoardComponent();
            menukeysC.AddKeyToAction("Up", Keys.Up);
            menukeysC.AddKeyToAction("Down", Keys.Down);
            menukeysC.AddKeyToAction("Select", Keys.Enter);
            ComponentManager.AddComponent(menuEntity, menuC);
            ComponentManager.AddComponent(menuEntity, menukeysC);


            Entity background = ComponentManager.MakeEntity();
            var bgSprite = new Texture2DComponent() {
                Texture = Content.Load<Texture2D>("Scenes\\startBackground2"),
                Color = Color.White,
            };
            var pos = new Position2DComponent() {
                Postion = Vector2.Zero,
            };
            ComponentManager.AddComponent(background, bgSprite, pos);

            Entity text = ComponentManager.MakeEntity();
            var txt = new SpriteTextComponent() {
                SpriteFont = Content.Load<SpriteFont>("Scenes\\Font1"),
                Text = "Press Enter To Play Singleplayer",
                Color = Color.Red,
            };
            var postext = new Position2DComponent() {
                Postion = new Vector2(200, 100),
            };

            ComponentManager.AddComponent(text, txt, postext);

            Entity text2 = ComponentManager.MakeEntity();
            var txt2 = new SpriteTextComponent()
            {
                SpriteFont = Content.Load<SpriteFont>("Scenes\\Font1"),
                Text = "Press UpArrow To Play Multiplayer",
                Color = Color.Red,
            };
            var postext2 = new Position2DComponent()
            {
                Postion = new Vector2(200, 200),
            };

            ComponentManager.AddComponent(text2, txt2, postext2);

            Entity text3 = ComponentManager.MakeEntity();
            var txt3 = new SpriteTextComponent()
            {
                SpriteFont = Content.Load<SpriteFont>("Scenes\\Font1"),
                Text = "Press DownArrow To Host Game",
                Color = Color.Red,
            };
            var postext3 = new Position2DComponent()
            {
                Postion = new Vector2(200, 300),
            };

            ComponentManager.AddComponent(text3, txt3, postext3);

            SystemManager.AddSystem(new SpriteRenderSystem());
            SystemManager.AddSystem(new KeyBoardSystem());
            SystemManager.AddSystem(new MenuSystem());

            base.InitScene();
        }

        public override void ResetScene() {
            throw new NotImplementedException();
        }

        public override void DestroyScene() {
            base.DestroyScene();
        }
    }
}
