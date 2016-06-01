using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Scenes {

    //public enum IsActive{
    //    Active,
    //    NonActive
    //}
    public abstract class GameScene {

        public bool IsActive { get; set; } = false;
        public bool IsPopup { get; set; } 
        public bool IsBackground { get; set; }
        public List <Entity> entities { get; set; }
        public string SceneName { get; set; }



    }
}
