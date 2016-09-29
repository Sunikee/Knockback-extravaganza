using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component
{
    public class GameOverComponent : IComponent
    {
        //En kommentar
        public int ActiveChoice { get; set; }
        public Color ActiveColor { get; set; }
        public Color InactiveColor { get; set; }

        public int MenuChoicesSpacing { get; set; }
    }
}
