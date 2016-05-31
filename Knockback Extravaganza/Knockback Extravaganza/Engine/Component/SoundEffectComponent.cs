using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class SoundEffectComponent : IComponent{
        Dictionary<string, SoundEffectPiece> soundEffects = new Dictionary<string, SoundEffectPiece>();

        public int ActiveTime { get; set; }

        public Dictionary<string, SoundEffectPiece> GetEffectDictionary() {
            return soundEffects;
        }

        public void AddSoundEffect(string key, SoundEffectPiece effect) {
            soundEffects.Add(key, effect);
        }

        public void RemoveSoundEffect(string key) {
            soundEffects.Remove(key);
        }

        public void RemoveAllSoundEffects() {
            soundEffects.Clear();
        }
        public void PlaySoundEffect(SoundEffect effect) {
            effect.CreateInstance().Play();
        }

        public void PlaySoundEffect(SoundEffect effect, float volume) {
            SoundEffectInstance instance = effect.CreateInstance();
            instance.Volume = volume;
            instance.Play();
        }
    }
}
