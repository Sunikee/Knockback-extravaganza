using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    
    public class SoundComponent : IComponent {
        Dictionary<string, SoundEffectPiece> soundEffects = new Dictionary<string, SoundEffectPiece>();
        Dictionary<string, SongPiece> songs = new Dictionary<string, SongPiece>();

        public SoundComponent() {

        }

        public Dictionary<string, SoundEffectPiece> GetEffectDictionary() {
            return soundEffects;
        }

        public Dictionary<string, SongPiece> GetSongDictionary() {
            return songs;
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

        public void AddSong(string key, SongPiece song) {
            songs.Add(key, song);
        }

        public void RemoveSong(string key) {
            songs.Remove(key);
        }

        public void RemoveAllSongs() {
            songs.Clear();
        }

        public void PlaySoundEffect(SoundEffect effect) {
            effect.CreateInstance().Play(); 
        }

        public void PlaySong(Song song) {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public void StopSong() {
            MediaPlayer.Stop();
        }

    }
}
