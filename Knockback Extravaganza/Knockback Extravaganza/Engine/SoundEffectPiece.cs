using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine {
    public class SoundEffectPiece {
        public SoundEffect SoundEffect { get; set; }

        public float ActiveTime { get; set; }

        public float CurrentActiveTime { get; set; }

        public bool IsActive { get; set; }

        

        public SoundEffectPiece(SoundEffect soundEffect) {
            SoundEffect = soundEffect;
            IsActive = false;
        }

        public SoundEffectPiece(SoundEffect soundEffect, bool isActive) {
            SoundEffect = soundEffect;
            IsActive = isActive;
        }
    }
}
