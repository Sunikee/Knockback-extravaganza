using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Scenes {
    public class StartScreen : GameScene {

        public void Update(SpriteBatch sb, SpriteFont sf, Texture2D background, ref List<GameScene>activeScene) {
            sb.Begin();
            
            sb.Draw(background, new Vector2(50, 50), Color.Red);

            //VI TRYCKER PÅ SINGLEPLAYER
            //add singleplayer to activelist
            activeScene.Remove(this);


        }



    }
}
