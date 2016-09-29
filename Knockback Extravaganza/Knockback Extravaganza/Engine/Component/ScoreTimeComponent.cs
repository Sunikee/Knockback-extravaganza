using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component
{
    public class ScoreTimeComponent : IComponent
    {
        public decimal Score { get; set; }
        public int TotalScore { get; set; }
        public float ElapsedTime { get; set; }
        public float TimerDuration { get; set; }
        public Stopwatch stopWatch { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        public SpriteFont spriteFont { get; set; }
        public Texture2D texture { get; set; }
    }
}
