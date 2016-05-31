using ECS_Engine.Engine.Component.Interfaces;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine.Component {
    public class SongComponent : IComponent{
        Dictionary<string, SongPiece> songs = new Dictionary<string, SongPiece>();

        public Song OldActiveSong { get; set; }

        public Song NewActiveSong { get; set; }

        public Dictionary<string, SongPiece> GetSongDictionary() {
            return songs;
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

        public void PlaySong(Song song) {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public void StopSong() {
            MediaPlayer.Stop();
        }
    }
}
