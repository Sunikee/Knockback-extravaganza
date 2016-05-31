using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Engine.Engine {
    public class SongPiece {
        public Song Song { get; set; }

        public bool AboutToStart { get; set; }

        public bool IsActive { get; set; } 

        public SongPiece(Song song) {
            Song = song;
            AboutToStart = false;
            IsActive = false;
        }

        public SongPiece(Song song, bool start) {
            Song = song;
            AboutToStart = start;
            IsActive = false;
        }
        public SongPiece(Song song, bool start, bool isActive) {
            Song = song;
            AboutToStart = start;
            IsActive = isActive;
        }
    }
}
